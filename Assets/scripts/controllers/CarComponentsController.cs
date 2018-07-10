using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public float carBankAmount = 0f;

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
    public float wheelSpinSpeed = 0f;
    public float wheelTurnAngle = 0f;
    public float wheelTurnSpeed = 0f;

    private float accelerateX = 0f;
    private float fallX;
    private GameObject[] allWheels;

    private void Start()
    {
        allWheels = carPart.rotatingWheels.Concat(carPart.staticWheels).ToArray();
    }

    // Update is called once per frame
    void Update ()
    {
        WheelsController();
        ChasisController();
        CarController();
	}

    void CarController()
    {
        //Rotate the car around the carFrontPivot gameObject
        carFrontPivot.transform.localRotation = Quaternion.Euler(new Vector3(0f, Mathf.Clamp(PlayerMovement.h * carRotationAngle, carRotationAngle * -1, carRotationAngle), 0f));

        if (PlayerMovement.isFalling == true)
        {
            //Roatates the carObject to an angle to look like it is falling
            fallX -= (Time.deltaTime * -1) * carFallRotationSpeed;
            fallX = Mathf.Clamp(fallX, 0f, carFallAngle);

            //Rotate the car on the z axis as it falls with carBankAmount
            carObject.transform.localRotation = Quaternion.Euler(new Vector3(fallX, 0f, (PlayerMovement.h * -1) * carBankAmount));
        }
        else
        {
            //stop falling rotation controls
            fallX -= 900 * Time.deltaTime;
            fallX = Mathf.Clamp(fallX, 0f, carFallAngle);

            carObject.transform.localRotation = Quaternion.Euler(new Vector3(fallX, 0f, 0f));
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
        if (PlayerMovement.isIgnition == true)
        {            
            //Rotate all wheels at the same speed in the same direction
            foreach (GameObject wheel in allWheels)
            {
                wheel.transform.localRotation *= Quaternion.Euler(new Vector3(Time.deltaTime * wheelSpinSpeed, 0f, 0f));
            }
        }

        //This controls the turn angle of the turning wheels on the vehicle
        foreach (GameObject turnWheel in carPart.rotatingWheels)
        {
            float turn = PlayerMovement.h * wheelTurnSpeed;
            turn = Mathf.Clamp(turn, wheelTurnAngle * -1, wheelTurnAngle);

            turnWheel.transform.localRotation = Quaternion.Euler(new Vector3(0f, turn, 0f));
        }
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