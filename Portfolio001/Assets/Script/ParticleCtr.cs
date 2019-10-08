using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCtr : MonoBehaviour
{
    public GameObject instace;
    public float lifeTime = 0.5f;

    private void Update()
    {
        Destroy(instace, lifeTime);
    }
}
