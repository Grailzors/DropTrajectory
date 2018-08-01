using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Camera Components")]
    public GameObject camPoint;
    public GameObject camRotatePoint;

    [Header("Camera Control Variables")]
    public float camRotateMax = 20f;
    public float camRotateMin = 0f;
    public float camDollyUpSpeed = 0f;
    public float camDollyDownSpeed = 0f;
    public float camZoomInSpeed = 150f;
    public float camZoomOutSpeed = 10f;
    public float camFovMin = 60f;
    public float camFovMax = 100f;


    private void Update()
    {
        CamMove();
        CamFOVControl();
    }

    void CamMove()
    {
        float angle;
        Vector3 axis;

        if (PlayerMovement.isFalling == true)
        {
            //camRotatePoint.transform.rotation = ;
        }
        else if (PlayerMovement.isFalling == false)
        {

        }

        camRotatePoint.transform.rotation.ToAngleAxis(out angle, out axis);

        angle = Mathf.Clamp(angle, camRotateMin, camRotateMax);

        transform.position = camPoint.transform.position;
        transform.rotation = camPoint.transform.rotation;
    }

    void CamFOVControl()
    {
        Camera mainCam = Camera.main;;
        
        if (PlayerMovement.isFalling == false)
        {
            mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView - (1f * Time.deltaTime) * camZoomInSpeed, camFovMin, camFovMax);
        }
        else
        {
            mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView + (1f * Time.deltaTime) * camZoomOutSpeed, camFovMin, camFovMax);
        }
    }
}
