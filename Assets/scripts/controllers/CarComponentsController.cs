﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarComponentsController : MonoBehaviour {

    [Header("Car Componant Parts")]
    public GameObject carObject;
    public GameObject carFrontPivot;
    public GameObject StuntPivot;
    public CarComponents carPart;

    [Header("Car Componant Controls")]
    [Header("Car Object Control")]
    public float carFallAngle = 0f;
    public float carFallRotationSpeed = 0f;
    public float carRotationAngle = 0f;
    public float carRotationSpeed = 0f;
    public float carBankAmount = 0f;
    public float stuntCounter = 0f;

    [Header("Chasis")]
    public float chasisSpeed = 0f;
    public float chasisRotationMultiplier = 0f;
    public float chasisXRotation = 0f;
    public float chasisZRotation = 0f;
    public float chasisTiltBack = 0f;
    public float chasisTiltSpeed = 0f;

    [Header("Doors")]
    public float openThreshold = 0f;
    public float openAngle = 0f;

    [Header("Wheels")]
    public float wheelSpinSpeed = 0f;
    public float wheelTurnAngle = 0f;
    public float wheelTurnSpeed = 0f;

    [Header("Stunt Controls")]
    public float flipSpeed = 100f;
    public float rollSpeed = 100f;

    private float accelerateX = 0f;
    private float fallX;


    // Update is called once per frame
    void Update ()
    {
        WheelsController();
        ChasisController();
        CarController();
        TrickAnimations();
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
                accelerateX -= Time.deltaTime * chasisTiltSpeed;
            }
        }

        for (int i = 0; i < carPart.chasis.Length; i++)
        {
            //Take the chasis geo and give it an engine wobble
            carPart.chasis[i].transform.localRotation = Quaternion.Euler(new Vector3(accelerateX + x, 0f, z) * chasisRotationMultiplier);
        }

    }

    void TrickAnimations()
    {
        if (PlayerMovement.stuntID == 0)
        {
            stuntCounter = 0;
        }
        else if (PlayerMovement.stuntID > 0)
        {
            stuntCounter += 1 * Time.deltaTime;
        }
        

        

        switch (PlayerMovement.stuntID)
        {
            case 4:
                StartCoroutine(Flip(stuntCounter, 4));
                print("BackFlip");
                break;
            case 3:
                StartCoroutine(Flip(stuntCounter, 3));
                print("FrontFlip");
                break;
            case 2:
                print("RightRoll");
                break;
            case 1:
                print("LefttRoll");
                break;
        }
    }


    IEnumerator Flip(float counter, int value)
    {
        print("Running Coroutine");

        if (value == 3)
        {
            counter = counter * flipSpeed;
            Mathf.Clamp(counter, 0f, 360f);
        }
        else if (value == 4)
        {
            print("YAAAAAAAAAAAAAAAAAAAAAY");
            counter = (counter * flipSpeed) * -1;
            Mathf.Clamp(counter, -360f, 0f);
        }

        StuntPivot.transform.localRotation = Quaternion.Euler(new Vector3(counter, 0f, 0f));

        print(counter);

        while (PlayerMovement.stuntID > 0)
        {
            yield return new WaitForSeconds(1);
            counter = 0;
        }  
    }
    

    void DoorsController()
    {

    }

    void WheelsController()
    {
        float spin = Time.deltaTime * wheelSpinSpeed;

        if (PlayerMovement.isIgnition == false)
        {
            spin = 0f;
        }        

        /*
         *Look into usins deligates to optimize these for loops?
        */

        //Rotate all wheels at the same speed in the same direction
        foreach (GameObject wheel in carPart.allWheelsGeo)
        {
            wheel.transform.localRotation *= Quaternion.Euler(new Vector3(spin, 0f, 0f));
        }

        //Rotate the controls to the wheels that need to turn
        foreach (GameObject rotWheel in carPart.rotatingWheelsControls)
        {
            float turn = PlayerMovement.h * wheelTurnSpeed;
            turn = Mathf.Clamp(turn, wheelTurnAngle * -1, wheelTurnAngle);

            rotWheel.transform.localRotation = Quaternion.Euler(new Vector3(0f, turn, 0f));
        }
    }
}


[System.Serializable]
public class CarComponents{

    //This class allows me to construct the components of the car to individualy control
    //them for procedural animation
    public GameObject[] chasis;
    public GameObject[] doors;
    public GameObject[] rotatingWheelsControls;
    public GameObject[] allWheelsGeo;
}