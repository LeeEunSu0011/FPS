using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtr : MonoBehaviour
{
    public int PlayerHP = 30;                                   //Playerヒットポイント
    public Slider HPbar;

    public float walkSpeed;                                     //移動速度
    public float runSpeed;                                      //走る速度

    private float applySpeed;                                   //適用される速度

	//状態変数
	public bool canWalk = true;
    public bool isWalk = false;
    public bool isRun = false;

    public float lookSensitivity;                               //カメラ回転速度
    public float cameraRotationLimit;                           //カメラ角度制限
    private float currenCameraRotationX = 0f;                   //カメラXの変数

    //必要なコンポネント
    public Camera theCamera;                  //カメラ追加
    private Rigidbody rigid;                                    //リジット追加
    [SerializeField] private GunCtr[] gunCtr;                     //GunCtrスクリプト追加
    [SerializeField] private CrossHair crosshair;            //CrossHairスクリプト追加
	[SerializeField] private WeaponManager weaponManager;

    void Start()
    {
        HPbar.maxValue = PlayerHP;
        
        rigid = GetComponent<Rigidbody>();

        applySpeed = walkSpeed;                                 //適用される速度
    }

    void Update()
    {
		if (canWalk)
		{
			CharacterRotation();
			CameraRotation();
			MoveTry();
			RunTry();
		}

        HPbar.value = PlayerHP;
    }

    //歩き試し
    #region MoveTry
    private void MoveTry()
    {
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            isWalk = true;
            Move();
        }
        else
        {
            isWalk = false;
            isRun = false;
        }
        //歩く時のクロスヘア
        crosshair.Walk_CrossHair(isWalk);
        gunCtr[weaponManager.selectedweapon].currentGun.anim.SetBool("isWalk", isWalk);
    }
    #endregion MoveTry

    //歩き
    #region Move
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        rigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }
    #endregion Move

    //走り試し
    #region RunTry
    private void RunTry()
    {
        //走る時のクロスヘア
        crosshair.Run_CrossHair(isRun);

        //shiftキーを押していて照準状態ではなかったら
        if (isWalk && Input.GetKey(KeyCode.LeftShift) && !gunCtr[weaponManager.selectedweapon].isFineSightMode)
        {
            //これを呼び出す、走れる
            Running();
            isRun = true;
        }
        //shiftキーを押していないと
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            //これを呼び出す、走れない
            RunningCancel();
            isRun = false;
        }
        //走っている時、走ってない時のクロスヘアのアニメーション再生
        gunCtr[weaponManager.selectedweapon].currentGun.anim.SetBool("isRun", isRun);
    }
    #endregion RunTry

    //走り速度適用
    private void Running()
    {
        applySpeed = runSpeed;
    }

    //歩き速度適用
    private void RunningCancel()
    {
        applySpeed = walkSpeed;
    }

    //キャラー回転
    #region CharactreRotation
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _charaterRotationY = new Vector3(0, _yRotation, 0) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(_charaterRotationY));
    }
    #endregion CharactreRotation

    //カメラ回転
    #region CameraRotaion
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _rotationCamera = _xRotation * lookSensitivity;
        currenCameraRotationX -= _rotationCamera;

        //カメラ固定
        currenCameraRotationX = Mathf.Clamp(currenCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        //カメラ適用
        theCamera.transform.localEulerAngles = new Vector3(currenCameraRotationX, 0f, 0f);
    }
    #endregion CameraRotaion

    #region bulletCollider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            Debug.Log("damage");
            PlayerHP -= bulletCtr.damage;
            Destroy(other.gameObject);
        }
    }
	#endregion bulletCollider
}
