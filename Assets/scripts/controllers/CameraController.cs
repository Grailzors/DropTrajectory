using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float camZoomInSpeed = 150f;
    public float camZoomOutSpeed = 10f;

    private Camera mainCam;
    private GameObject player;
    private Vector3 camOffset;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        mainCam = Camera.main;
        camOffset = mainCam.transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        CamMove();
        CamFOVControl();
    }

    void CamMove()
    {
        //Need to make this lerp between these positions using Vector3.lerp
        if (PlayerMovement.isFalling == false)
        {
            mainCam.transform.position = player.transform.position + camOffset;
        }
        else
        {
            mainCam.transform.position = player.transform.position + camOffset + new Vector3(0f, 30f, 0f);
        }
        
    }

    void CamFOVControl()
    {
        if (PlayerMovement.isFalling == false)
        {
            mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView - (1f * Time.deltaTime) * camZoomInSpeed, 60f, 90f);
        }
        else
        {
            mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView + (1f * Time.deltaTime) * camZoomOutSpeed, 60f, 90f);
        }
    }
}
