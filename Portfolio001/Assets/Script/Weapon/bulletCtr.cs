using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCtr : MonoBehaviour
{
	public static int damage = 5;

    void Update()
    {
        Destroy(gameObject, 5f);
    }

}
