using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRoomMuchineCtr : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject eventCamera;

    public Text eventText;

    public string eventchat;

    public BossRoomEvent bossroomenvet;

    private bool putbutton = false;

    // Start is called before the first frame update
    void Start()
    {
        bossroomenvet = GetComponent<BossRoomEvent>();

        eventCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            putbutton = true;
            bossroomenvet.goEnvet = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!putbutton)
            {
                eventText.text = eventchat;
                eventText.color = Color.green;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!putbutton)
            {
                eventText.text = "";
            }
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
