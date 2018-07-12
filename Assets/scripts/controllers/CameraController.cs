using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Camera Components")]
    public GameObject camStartPoint;
    public GameObject camEndPoint;

    [Header("Camera Control Variables")]
    public float camDollyUpSpeed = 0f;
    public float camDollyDownSpeed = 0f;
    public float camZoomInSpeed = 150f;
    public float camZoomOutSpeed = 10f;

    private void LateUpdate()
    {
        CamMove();
        //CamFOVControl();
    }

    //Not working look at fixing with a simple rotational offset on a gameobject parented to the player
    void CamMove()
    {
        /*
        float x = transform.rotation.x;

        if (PlayerMovement.isFalling == false)
        {
            transform.position = Vector3.Lerp(transform.position, camStartPoint.transform.position, Time.time / camDollyDownSpeed);
            //x = Mathf.Lerp(camEndPoint.transform.rotation.x, camStartPoint.transform.rotation.x, Time.time / camDollyDownSpeed);
        }
        else if (PlayerMovement.isFalling == true)
        {
            transform.position = Vector3.Lerp(transform.position, camEndPoint.transform.position, Time.time / camDollyUpSpeed);
            //x = Mathf.Lerp(camStartPoint.transform.rotation.x, camEndPoint.transform.rotation.x, Time.time / camDollyDownSpeed);
        }
        */
    }

    void CamFOVControl()
    {
        Camera mainCam = Camera.main;;
        
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
