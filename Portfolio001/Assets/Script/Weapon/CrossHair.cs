using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
	//必要なコンポネント
	[SerializeField] private Animator anim;

    private float gunAccuracy;

    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private GunCtr[] gunCtr;

    //歩く時のクロスヘア
    public void Walk_CrossHair(bool _type)
	{
		anim.SetBool("Walk", _type);
	}

	//走る時のクロスヘア
	public void Run_CrossHair(bool _type)
	{
		anim.SetBool("Run", _type);
	}

	//照準時のクロスヘア
	public void FineSight_In_CrossHair(bool _type)
	{
		anim.SetBool("FindSight", _type);
	}

	//歩きながら発射時のクロスヘア
	public void Walk_Fire_CrossHair()
	{
		anim.Play("WalkFire_CrossHair",0,0);
	}

	//発射時のクロスヘア
	public void Fire_CrossHair()
	{
		anim.Play("FireCrossHair", 0, 0);
	}


    public float Get_GunAccuracy()
    {
        if (anim.GetBool("isWalk"))
            gunAccuracy = 0.08f;
        else if (gunCtr[weaponManager.selectedweapon].Get_FinesightMode())
            gunAccuracy = 0.0001f;
        else
            gunAccuracy = 0.01f;

        Debug.Log(gunAccuracy);
        return gunAccuracy;
    }
}
