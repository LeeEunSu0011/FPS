using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEvent : MonoBehaviour
{
	public GameObject PlayerCamera;
	public GameObject DoorEventCamera;

	public Animator anim;
	public Door door;

	private DoorCtr doorctr;

    // Start is called before the first frame update
    void Start()
    {
		PlayerCamera = GameObject.Find("Main Camera");
		DoorEventCamera = GameObject.Find("DoorEventCamera");

		DoorEventCamera.SetActive(false);

		anim = GetComponent<Animator>();
		doorctr = GetComponent<DoorCtr>();
    }

	void Update()
	{
		if (doorctr.putButton)
		{
			anim.SetTrigger("DoorEventOn");
		}
	}

	void FirstCameraChange()
	{
		PlayerCamera.SetActive(false);
		DoorEventCamera.SetActive(true);
	}

	void LastCameraCtr()
	{
		PlayerCamera.SetActive(true);
		DoorEventCamera.SetActive(false);
	}
}
