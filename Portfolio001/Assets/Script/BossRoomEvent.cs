using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEvent : MonoBehaviour
{
    public bool goEnvet = false;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goEnvet)
            anim.SetTrigger("GoBossRoomEvent");
    }
}
