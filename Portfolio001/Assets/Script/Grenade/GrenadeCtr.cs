using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeCtr : MonoBehaviour
{
    public float throwFroce = 40f;
    public GameObject grenadePrefad;
    public int grenadeCount = 1;
    public Transform grenadeposition;

    bool hasgrenade = true;

    // Update is called once per frame
    void Update()
    {
        if (grenadeCount == 0)
        {
            hasgrenade = false;
        }

        if (Input.GetKeyDown(KeyCode.G) && hasgrenade)
        {
            grenadeCount--;
            TrowGreande();          
        }
    }

    void TrowGreande()
    {
        GameObject grenade = Instantiate(grenadePrefad, transform.position, transform.rotation);
        Rigidbody rigid = grenade.GetComponent<Rigidbody>();
        rigid.AddForce(grenadeposition.forward * throwFroce, ForceMode.Impulse);
    }
}
