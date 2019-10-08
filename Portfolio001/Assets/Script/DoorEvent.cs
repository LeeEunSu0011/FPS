using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEvent : MonoBehaviour
{
	public GameObject playerCamera;
	public GameObject eventCamera;

	public Animator anim;
	public Door door;

	private DoorCtr doorctr;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("Main Camera");
        eventCamera = GameObject.Find("DoorEventCamera");

        eventCamera.SetActive(false);

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
        playerCamera.SetActive(false);
        eventCamera.SetActive(true);
	}

	void LastCameraCtr()
	{
        playerCamera.SetActive(true);
        eventCamera.SetActive(false);
	}
}
