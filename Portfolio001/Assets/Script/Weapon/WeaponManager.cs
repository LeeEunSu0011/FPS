using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	//武器選択
    public int selectedweapon;

    //武器スクリプトを取得
    public GunCtr[] gunCtr;

    void Start()
    {
		//武器選択関数を最初に実行
        selectWeapon();
    }

    
    void Update()
    {
		//任意の変数設定
        int previousWeaponSelected = selectedweapon;

		//上スクロールで武器交換
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
			//選択した武器がHolderの子の数より多かったら
            if (selectedweapon >= transform.childCount - 1)
                selectedweapon = 0;
			//選択した武器がHolderの子の数より小さかったら
			else
				selectedweapon++;
        }

		//下スクロールで武器交換
		if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
			//選択した武器が0より小さかったら
			if (selectedweapon <= 0)
                selectedweapon = transform.childCount - 1;
			//選択した武器が0より多かったら
			else
				selectedweapon--;
        }

		//キーボード１で武器交換
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedweapon = 0;
        }

		//キーボード２で武器交換
		if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedweapon = 1;
        }

		//選択した武器が２番だったら
		if (selectedweapon != 0)
		{
			gunCtr[0].CancelFineSightMode();
		}
		//選択した武器が１番だったら
		else if (selectedweapon != 1)
		{
			gunCtr[1].CancelFineSightMode();
		}

		//もし任意の武器変数が武器選択変数と違ったら
		if (previousWeaponSelected != selectedweapon)
        {
			//リロード状態をキャンセル
            gunCtr[selectedweapon].CancelReload();
            //武器選択関数を実行
            selectWeapon();
        }
    }

    //武器選択関数
    #region selectWeapon
    private void selectWeapon()
    {
		//Holderから子の数を取得
        int holderChild = 0;

        foreach (Transform Holder in transform)
        {
			//もしHolderの子が選択した武器と同じだったら
			if (holderChild == selectedweapon)
			{
				//その子を活性化する
				Holder.gameObject.SetActive(true);
			}
			//もしHolderの子が選択した武器と同じじゃなかったら
			else
			{
				//その子を非活性化する
				Holder.gameObject.SetActive(false);
			}
			holderChild++;
        }
	}
    #endregion selectWeapon
}
