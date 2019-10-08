using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunCtr : MonoBehaviour
{
	private RaycastHit hitinfo;                             //当たった情報取得
	private float lastFired;                                //最後のTime.timeを取得

	private float ReloadMode;                               //リロード状態

    //状態変数	
    public bool canFire = false;
	public bool isFineSightMode = false;                    //照準状態
	private bool isReload = false;                          //リロード状態
	private bool isFire = false;                            //発射状態

	//必要なコンポネント
	private Vector3 originPos;                               //元の位置
	public Gun currentGun;               //Gunスクリプト追加
	[SerializeField] private PlayerCtr playerCtr;            //PlayerCtrスクリプト追加
	[SerializeField] private CrossHair crosshair;            //CrossHairスクリプト追加
	public GameObject hitEffectPrefab;                       //hitEffect追加
	public GameObject pilot_HitEffectPrefab;                 //被弾の痕跡
	public Text CarryAmmor;                                  //持っている弾丸
	public Text Reload_CurrentAmmor;                         //リロードする弾丸とロードしている弾丸
	public int zoom_In;                                      //カメラ拡大
	public int zoom_Out;                                     //カメラ縮小

	// Start is called before the first frame update
	void Start()
	{
		//元の位置を初期化
		originPos = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
	{
        if (canFire)
        {
            FireTry();
            TryFineSight();
            ReloadTry();
            CarryAmmor.text = "" + currentGun.carryBulletCount;
            Reload_CurrentAmmor.text = currentGun.reloadBulletCount + "　/　" + currentGun.currentBulletCount;
        }
	}

	//発射試し
	#region FireTry
	private void FireTry()
	{
		//入力されたら
		if (Input.GetMouseButton(0) && currentGun.currentBulletCount > 0 && !isReload && !playerCtr.isRun)
		{
			//これを呼び出す
			Fire();
		}
	}
	#endregion FireTry

	//発射
	#region Fire
	private void Fire()
	{
		//連射具現
		if (Time.time - lastFired > 1 / currentGun.fireRate)
		{
			lastFired = Time.time;                  //Time.time 取得
			currentGun.anim.Play("Fire", 0, 0f);    //発射アニメーション実行

			//もし照準状態ではなかったら
			if (isFineSightMode == false)
			{
				//もし歩かない状態で発射したら
				if (!playerCtr.isWalk)
				{
					crosshair.Fire_CrossHair();         //発射クロスヘアアニメーション実行
				}
				//もし歩く中に発射したら
				else
				{
					crosshair.Walk_Fire_CrossHair();            //発射クロスヘアアニメーション実行
				}
			}

			currentGun.fireMuzzleFlash.Play();      //エフェクト	
			currentGun.currentBulletCount--;        //発射したので弾丸マイナス
													//currentGun.fireSound.Play();			//SEPlay
			Hit();                                  //当たったものの情報取得
		}
	}
	#endregion Fire

	//Raycastで当たった情報取得
	#region Hit
	private void Hit()
	{
        //弾丸が自分に当たってしまうことが起きるのでPlayerのlayerだけ抜ける
		int layerMask = ~(1 << 10);

		//playerにコンポネントされたcameraから取得
		//射撃に正確度のためクロスヘアから値を取得後、銃の正確度を引く又は足す
		if (Physics.Raycast(playerCtr.theCamera.transform.position, playerCtr.theCamera.transform.forward +
			//状態による正確度
			new Vector3(Random.Range(-crosshair.Get_GunAccuracy() - currentGun.accuracy, crosshair.Get_GunAccuracy() + currentGun.accuracy),
			Random.Range(-crosshair.Get_GunAccuracy() - currentGun.accuracy, crosshair.Get_GunAccuracy() + currentGun.accuracy),
			0f)
			, out hitinfo, currentGun.range, layerMask))
		{
            #region BulletEffect
            //弾丸エフェクト
            GameObject Bullet = Instantiate(hitEffectPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
			//弾丸エフェクト消す
			Destroy(Bullet, 0.2f);
            #endregion BulletEffect

            #region 痕跡
            //当たった物に弾丸の痕跡を残す
            GameObject _object = Instantiate(pilot_HitEffectPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
            //当たった物に弾丸の痕跡を残す
            _object.transform.SetParent(hitinfo.transform);
            //痕跡を２秒後に消す
            Destroy(_object, 1f);
            #endregion 痕跡

            #region targetDamage
            //弾が当たったターゲットの情報のやり取りのため取得する
            TargetCtr targetCtr = hitinfo.transform.GetComponent<TargetCtr>();

            //もしターゲットnullではなかった場合
            if (targetCtr != null)
                //ターゲットコントロールスクリプトに銃のダメージの値を渡す
                targetCtr.Take_Target_Damege(currentGun.damage);
            #endregion targetDamage

            #region enemyDamage
            //敵の情報取得
            EnemyCtr enemyCtr = hitinfo.transform.GetComponent<EnemyCtr>();

            //もしエネミーがnullではなかった場合
            if (enemyCtr != null)
                enemyCtr.Take_Damage(currentGun.damage);
            #endregion enemyDamage

            Debug.Log(hitinfo.transform.name);
		}
	}
	#endregion Hit

	//照準試し
	#region TryFineSight 
	private void TryFineSight()
	{
		//マウスの右クリックされたら
		if (Input.GetButtonDown("Fire2") && !isReload)
		{
            //これを呼び出す(状態変数をもらう)
            FineSight();
		}
	}
	#endregion TryFineSight

	//照準
	#region FindSight
	private void FineSight()
	{
        //照準の状態変数を変える
        isFineSightMode = !isFineSightMode;
        //アニメーション実行
        currentGun.anim.SetBool("FindSight", isFineSightMode);
		//照準時、照準解除のクロスヘアのアニメーション再生
		crosshair.FineSight_In_CrossHair(isFineSightMode);


		//照準状態変数に従って実行
		if (isFineSightMode)
		{
			//一度全てのコルーチンを停止
			StopAllCoroutines();
			//コルーチン実行
			StartCoroutine(FineSightActiviteCoroutine());
		}
		else
		{
			//一度全てのコルーチンを停止
			StopAllCoroutines();
			//コルーチン実行
			StartCoroutine(FineSightDeActiviteCoroutine());
		}
	}
	#endregion FindSight

	//正照準活性化
	#region FineSightActiviteCoroutine
	IEnumerator FineSightActiviteCoroutine()
	{
		//照準
		//元の位置と照準移置が違ったら
		while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
		{
			//元の位置を照準移置まで移動させる
			currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
			//カメラも拡大させる
			playerCtr.theCamera.fieldOfView = Mathf.Lerp(zoom_Out, zoom_In, 2f);
			yield return null;
		}
	}
	#endregion FindSightActiviteCoroutine

	//正照準非活性化
	#region FineSightDeActiviteCoroutine
	IEnumerator FineSightDeActiviteCoroutine()
	{
		//非照準
		//照準の位置と元の位置が違ったら
		while (currentGun.transform.localPosition != originPos)
		{
			//照準した位置を元の位置に戻す
			currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
			//カメラも縮小させる
			playerCtr.theCamera.fieldOfView = Mathf.Lerp(zoom_In, zoom_Out, 2f);
			yield return null;
		}
	}
	#endregion FineSightDeActiviteCoroutine

	//リロード試し
	#region Reload
	private void ReloadTry()
	{
		//入力されたら
		if (Input.GetKeyDown(KeyCode.R) && currentGun.currentBulletCount < currentGun.reloadBulletCount && !isReload)
		{
            if(isFineSightMode)
                //照準に適用
                FineSight();
			//コルーテン実行
			StartCoroutine(ReloadCoroutine());
		}
	}
	#endregion Reload

    //リロード
	#region ReloadCoroutine
	IEnumerator ReloadCoroutine()
	{
		//もし持っている弾丸が0じゃなかったら
		if (currentGun.carryBulletCount > 0)
		{
			//状態変数を変更
			isReload = true;

			//ロードされた弾丸が0だったら
			if (currentGun.currentBulletCount <= 0)
			{
				//リロードアニメーション実行
				currentGun.anim.SetTrigger("Reload_Ammor");
				//リロードモード
				ReloadMode = currentGun.Ammor_Reload_Time;
			}
			//ロードされた弾丸が0じゃなかったら
			else
			{
				//リロードアニメーション実行
				currentGun.anim.SetTrigger("Reload_Left_Ammor");
				//リロードモード
				ReloadMode = currentGun.left_Ammor_Reload_Time;
			}

			//持っている弾丸にロードされた弾丸を足す
			currentGun.carryBulletCount += currentGun.currentBulletCount;
			//ロードされた弾丸を0にする
			currentGun.currentBulletCount = 0;

			//待機
			yield return new WaitForSeconds(ReloadMode);

			//持っている弾丸がリロードされる弾丸より大きかったら
			if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
			{
				//弾丸をリロードする
				currentGun.currentBulletCount = currentGun.reloadBulletCount;
				//持っている弾丸からリロードされる弾丸を引く
				currentGun.carryBulletCount -= currentGun.reloadBulletCount;
			}
			//持っている弾丸がリロードされる弾丸より小さかったら
			else
			{
				//弾丸をリロードする
				currentGun.currentBulletCount = currentGun.carryBulletCount;
				//持っている弾丸を0にする
				currentGun.carryBulletCount = 0;
			}

			//状態変数を変更
			isReload = false;
		}
		//もし持っている弾丸が0だったら
		else
		{
			Debug.Log("Not Have CarryBullet");
		}
	}
    #endregion ReloadCoroutine

    //武器を変えるときに使うリロードをキャンセルさせる関数
    #region CancelReload
    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }
    #endregion CancelReload

    //全てのアクションを止める
    #region CancelAction
    public void CancelAction()
	{
		StopAllCoroutines();
	}
	#endregion CancelAction

	//照準状態をもらう
	#region Get_FinesightMode
	public bool Get_FinesightMode()
	{
		return isFineSightMode;
	}
    #endregion Get_FinesightMode

    //照準状態を初期化する
    #region CancelFineSightMode
    public void CancelFineSightMode()
    {
        if (isFineSightMode)
            FineSight();
		//武器の座標を元の位置に初期化
		currentGun.transform.localPosition = Vector3.zero;
		Debug.Log(isFineSightMode);
    }
    #endregion CancelFineSightMode
}