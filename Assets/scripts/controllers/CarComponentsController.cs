using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarComponentsController : MonoBehaviour {

    [Header("Car Componant Parts")]
    public GameObject carObject;
    public GameObject carFrontPivot;
    public CarComponents carPart;

    [Header("Car Componant Controls")]
    [Header("Car Object Control")]
    public float carFallAngle = 0f;
    public float carFallRotationSpeed = 0f;
    public float carRotationAngle = 0f;
    public float carRotationSpeed = 0f;

    public float test;

    [Header("Chasis")]
    public float chasisSpeed = 0f;
    public float chasisRotationMultiplier = 0f;
    public float chasisXRotation = 0f;
    public float chasisZRotation = 0f;
    public float chasisTiltBack = 0f;

    [Header("Doors")]
    public float openThreshold = 0f;
    public float openAngle = 0f;

    [Header("Wheels")]
    public float wheelTurnAngle = 0f;
    public float wheelSpinSpeed = 0f;

    private float accelerateX = 0f;
    private float fallX;


    // Update is called once per frame
    void Update ()
    {
        ChasisController();
        CarController();
	}

    void CarController()
    {
        carFrontPivot.transform.rotation = Quaternion.Euler(new Vector3(0f, test, 0f));
        print(carFrontPivot.transform.rotation);

        if (PlayerMovement.isFalling == true)
        {
            //Roatates the carObject to an angle to look like it is falling
            fallX -= (Time.deltaTime * -1) * carFallRotationSpeed;
            fallX = Mathf.Clamp(fallX, 0f, carFallAngle);

            carObject.transform.rotation = Quaternion.Euler(new Vector3(fallX, 0f, 0f));
        }
        else
        {
            //Falling controls
            fallX -= 900 * Time.deltaTime;
            fallX = Mathf.Clamp(fallX, 0f, carFallAngle);

            carObject.transform.rotation = Quaternion.Euler(new Vector3(fallX, 0f, 0f));

            //Rotate the car around the carFrontPivot gameObject
            //carFrontPivot.transform.rotation = carFrontPivot.transform.localRotation //Quaternion.Euler(new Vector3(0f, PlayerMovement.h * carRotationSpeed, 0f));
        }
    }

    void ChasisController()
    {
        float x = Mathf.PingPong(Time.time * chasisSpeed, chasisXRotation);
        float z = Mathf.PingPong(Time.time * chasisSpeed, chasisZRotation + 0.5f);

        //Add an acceleration tilt to the chasis
        if (PlayerMovement.isIgnition == true)
        {
            //Remove chasis wobble when car is driving
            x = 0f;
            z = 0f;

            if (accelerateX > chasisTiltBack)
            {
               
                accelerateX -= Time.deltaTime * 5f;
            }
        }

        for (int i = 0; i < carPart.chasis.Length; i++)
        {
            //Take the chasis geo and give it an engine wobble
            carPart.chasis[i].transform.localRotation = Quaternion.Euler(new Vector3(accelerateX + x, 0f, z) * chasisRotationMultiplier);
        }

    }

    void DoorsController()
    {

    }

    void WheelsController()
    {

    }
}

[System.Serializable]
public class CarComponents{

    //This class allows me to construct the components of the car to individualy control
    //them for procedural animation
    public GameObject[] chasis;
    public GameObject[] doors;
    public GameObject[] rotatingWheels;
    public GameObject[] staticWheels;
}