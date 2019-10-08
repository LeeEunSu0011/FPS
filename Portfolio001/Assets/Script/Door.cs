using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
	public Animator anim;
	public Text text;

	public bool canopenDoor;

    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponentInChildren<Animator>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (canopenDoor)
		{
			if (other.gameObject.tag == "Player")
			{
				anim.SetBool("character_nearby", true);
			}
		}
		else
		{
			text.text = "このドアを開くためにはカギとなる機械を操作しなければなりません";
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (canopenDoor)
		{
			if (other.gameObject.tag == "Player")
			{
				anim.SetBool("character_nearby", false);
			}
		}
		else
		{
			text.text = "";
		}
	}
}
