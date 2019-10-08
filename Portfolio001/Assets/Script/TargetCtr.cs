using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetCtr : MonoBehaviour
{
	public float target_HP;			//ターゲットのヒットポイント(HP)
	public int resetTime;			//リセットさせる時間
	public Slider target_HP_Bar;    //ターゲットのヒットポイント(HP)のスライダー

	private bool isDaed = false;    //ターゲットのHPが0以上がのための変数

	private float timer;			//タイマー変数

	//必要なコンポネント
	private Animator anim;			//アニメーション
	

	void Start()
	{
		//ターゲットのHPのスライダーのマックスの設定 
		target_HP_Bar.maxValue = target_HP;

		//アニメーションコンポネント
		anim = GetComponent<Animator>();
	}

    void Update()
    {
        //また戻すためにタイマーを設定
        if (isDaed)
        {
            //タイマーの値設定
            timer += Time.deltaTime;
            //もしtimeをresetTimeで割った残りが0だったら
            if (timer >= resetTime)
            {
                //タイマー初期化
                timer = 0;
                //ターゲットのHPを元に戻す
                //現在のHPをスライダーの最大値にする
                target_HP = target_HP_Bar.maxValue;
                //スライダーの値を現在のHPを適用する
                target_HP_Bar.value = target_HP;
                //状態変数変更
                isDaed = false;
                //アニメーション関数に状態変数を渡す
                target_Down(isDaed);
            }
        }
    }

    //ダメージ受けたらここで処理する
    #region Take_Target_Damege

    public void Take_Target_Damege(int Damage)
	{
		//ターゲットのHPを受けた攻撃力で削る
		target_HP -= Damage;
		//HPのスライダーに適用
		target_HP_Bar.value = target_HP;

		//もしターゲットのHPが0だったら
		if (target_HP <= 0)
		{
			//状態変数変更
			isDaed = true;
			//アニメーションに状態変数を渡す
			target_Down(isDaed);
		}

	}
    #endregion Take_Target_Damege

    //ターゲットのアニメーションの処理
    #region target_Donw
    private void target_Down(bool state)
	{
		anim.SetBool("Dead", state);
	}
    #endregion target_Donw 
}
