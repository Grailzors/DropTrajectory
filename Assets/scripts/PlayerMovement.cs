using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed = 150f;
    public float fallingRotSpeed = -18f;
    public float angleMult = 60f;
    public float donutSpinSpeed = 0f;
    public float donutReturnSpeed = 0f;
    public float speedUp = 2.5f;
    public float wheelRot = 100f;

    private GameObject player;
    private Transform donutPiv;
    private Slider donutSlider;
    private GameObject[] backWheels;
    private GameObject[] frontWheels;
    private float h;
    private float fallingRot;
    private float buttonCounter;
    private float angle;
    private bool isIgnition;
    private bool isFalling;
    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        donutPiv = player.transform.GetChild(4);
        donutSlider = GameObject.Find("DonutSlider").GetComponent<Slider>();
        backWheels = GameObject.FindGameObjectsWithTag("BackWheels");
        frontWheels = GameObject.FindGameObjectsWithTag("FrontWheels");
    }


    private void Start()
    {
        buttonCounter = 0f;
        isIgnition = false;
        angle = 0f;
        fallingRot = 0f;
        isFalling = false;
    }


    private void Update()
    {
        h = Input.GetAxis("Horizontal");

        Ignition();
        WheelsControl();
        

        //Debug.Log(isFalling);
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FinishLine")
        {
            print("GAMEOVER!!!!");
        }
        else if (other.gameObject.tag == "Platform")
        {
            isFalling = false;
            print("Entered!");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            isFalling = true;

            //print("Exit!");
        }
    }
    

    private void PlayerFalling()
    {
        if (isFalling == true)
        {
            fallingRot += (Time.deltaTime * -1) * fallingRotSpeed;
            fallingRot = Mathf.Clamp(fallingRot, 0, 70);
            //Debug.Log(fallingRot);
        }
        else
        {
            fallingRot = (0f * Time.deltaTime) * 100f;
        }
    }


    private bool Ignition()
    {
        //float angle;
        Vector3 vec;

        //Increase speed of the car as it rotates towards 360 degrees
        if (Input.GetKey(KeyCode.A))
        {
            buttonCounter += speedUp * (Time.deltaTime);
        }
        else
        {
            buttonCounter = 0f;
        }

        //using the horizontal input to drive the y rotation
        if (angle < 359)
        {
            //using the horizontal input to drive the y rotation
            donutPiv.rotation = donutPiv.rotation * Quaternion.Euler(0f, h * donutSpinSpeed, 0f);
            donutPiv.rotation.ToAngleAxis(out angle, out vec);

            //grabbing the angle and vector from the rotation and clamping it to make it rotate only in one direction
            //and to stop it going further then 359 degrees
            angle = Mathf.Clamp(angle - (donutReturnSpeed * Time.deltaTime) + buttonCounter, 0f, 359f);
            vec.y = Mathf.Clamp(vec.y, -1, 0);

            //setting the clamps rotation angle and vector
            donutPiv.rotation = Quaternion.AngleAxis(angle, new Vector3(0f, vec.y, 0f));

        }

        //Fill the UI donut up (need to replace with donut shaped UI)
        FillDonut(angle);
        
        //When angle gets close to 360 set isIgnition to true and move player forward  
        if (angle >= 359f)
        {
            angle = 360f;
         
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isIgnition = true;
                Debug.Log("GOOOOOOOOOOOOOOOO!!!!!");
            }
            
        }
        return isIgnition;
    }


    private void PlayerMove()
    {
        PlayerFalling();

        if (isIgnition == true)
        {
            //start moving player perminatly forward
            player.GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed;
            transform.rotation = Quaternion.Euler(fallingRot, Mathf.Clamp(h * angleMult, -45, 45), 0f);
        }
    }


    void WheelsControl()
    {
        foreach (GameObject wheel in frontWheels)
        {
            //Debug.Log(wheel);
            wheel.transform.rotation = Quaternion.Euler(fallingRot, Mathf.Clamp(h * angleMult, -45, 45), 0f);
            if (isIgnition == true)
            {
                wheel.transform.rotation = wheel.transform.rotation * Quaternion.Euler((Time.deltaTime * wheelRot), 0f, 0f);
            }
        }

        foreach (GameObject wheel in backWheels)
        {
            wheel.transform.rotation = wheel.transform.rotation * Quaternion.Euler((Time.deltaTime * wheelRot), 0f, 0f);
        }
    }


    private void FillDonut(float angle)
    {
        donutSlider.value = angle;
    }

    

}
