using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportCtr : MonoBehaviour
{
	public Transform teleportTarget;

	public GameObject Player;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Player.transform.position = teleportTarget.transform.position;
		}
	}
}
