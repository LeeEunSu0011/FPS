using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorCtr : MonoBehaviour
{
	public Text text;

	public bool putButton = false;
	private bool textEvent = false;

	public Door doorctr;

	// Update is called once per frame
	void Update()
	{
		if (textEvent)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				text.text = "";
				putButton = true;
				doorctr.canopenDoor = true;
			}
		}
    }

	void OnTriggerEnter(Collider other)
	{
		if (!putButton)
		{
			if (other.gameObject.tag == "Player")
			{
				textEvent = true;
				text.text = "PressE";
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		text.text = "";
	}
}
