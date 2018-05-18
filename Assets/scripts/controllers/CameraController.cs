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
        mainCam.transform.position = player.transform.position + camOffset;
        CamFOVControl();
        //Test
    }

    void CamFOVControl()
    {
        float x = player.transform.rotation.x;

        if (x <= 0f)
        {
            mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView - (1f * Time.deltaTime) * camZoomInSpeed, 60f, 90f);
        }
        else if (x >= 0)
        {
            mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView + (1f * Time.deltaTime) * camZoomOutSpeed, 60f, 90f);
        }
    }
}
