using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Camera Components")]
    public GameObject camPoint;
    public GameObject camRotatePoint;

    [Header("Camera Control Variables")]
    public float camRotateDelay = 0.5f;
    public float camRotateSpeed = 0f;
    public float camRotateMax = 20f;
    public float camRotateMin = 0f;
    public float camDollyUp = 0f;
    public float camDollyDown = 0f;
    public float camZoomInSpeed = 150f;
    public float camZoomOutSpeed = 10f;
    public float camFovMin = 60f;
    public float camFovMax = 100f;

    private float moveCounter;
    private float rotCounter;


    private void Update()
    {
        CamMove();
        CamFOVControl();
    }

    void CamMove()
    {       
        //Create and set the rotation on the camRotatePoint

        if (PlayerMovement.isFalling == true)
        {
            moveCounter += Time.deltaTime;
        }

        if (PlayerMovement.isFalling == true && moveCounter > camRotateDelay)
        {
            rotCounter += camDollyUp * Time.deltaTime;
        }
        else if (PlayerMovement.isFalling == false)
        {
            rotCounter -= camDollyDown * Time.deltaTime;
            moveCounter = 0f;
        }
        
        //Clamp the value
        rotCounter = Mathf.Clamp(rotCounter, camRotateMin, camRotateMax);

        //Normailize the rotation value so i can give it some easing
        float smoothStep = Mathf.InverseLerp(camRotateMin, camRotateMax, rotCounter);
        //Currently using cubic easing with the easing
        float smoothStart = smoothStep * smoothStep * smoothStep;
        //Flip the smoothStart
        float smoothStop = 1 - ( (1 - smoothStep) * (1 - smoothStep) * (1 - smoothStep) );
        
        //The amount top blend between both smooth variables (0.5f = 50% blend) 
        float blend = 0.5f;

        float smoothInOut = (1 - blend) * smoothStart + (blend) * smoothStop;

        camRotatePoint.transform.rotation = Quaternion.Euler(Mathf.Clamp(smoothInOut * camRotateSpeed, camRotateMin, camRotateMax), 0f, 0f);

        //Set the cameras position and rotate to follow the player
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