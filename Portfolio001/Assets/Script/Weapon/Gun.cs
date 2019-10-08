using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
	public string weaponName;					//武器名前
	public float range;							//射手距離
	public float fireRate;						//連射速度
	public float accuracy;						//正確度

	public int damage;							//ダメージ

	public int currentBulletCount;				//残った弾丸数
	public int reloadBulletCount;				//リーロドされる弾丸数
	public int carryBulletCount;				//持っている弾丸数
	public int maxBulletCount;                  //最大まで入る弾丸数

	public float left_Ammor_Reload_Time;		//弾丸が残った状態のリロードする時間
	public float Ammor_Reload_Time;				//リロードする時間

	public Vector3 fineSightOriginPos;			//照準移置
	public Animator anim;						//アニメーション
	public ParticleSystem fireMuzzleFlash;      //銃口のイフェクト
	public AudioSource fireSound;				//発射SE

    public GunCtr gunctr;

    public void cantshootting()
    {
        gunctr.canFire = false;
    }

    public void canshootting()
    {
        gunctr.canFire = true;
    }
}
