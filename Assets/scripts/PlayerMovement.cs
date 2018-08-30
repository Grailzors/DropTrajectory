using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Control Variables")]
    public float moveSpeed = 5f;
    public float horizontalSpeed = 5f;
    public float horizontalSmoothStep = 0f;  

    [Header("Abilities Variables")]
    public float coolDownTime = 0f;
    public float airDashDuration = 0f;
    public float airDashMultiplier = 0f;
    public float airBreakDuration = 0f;
    public float airBreakAmount = 0f;
    public float groundSlamDuration = 0f;
    public float groundSlamAmount = 0f;

    public static float h;
    public static bool isFalling;
    public static float fallingCounter;
    public static bool isIgnition;
    public static int abilitySelect = 1;
    public static bool abilityEnabled;

    private Rigidbody playerRB;
    private float fallStrength;
    private float previousPos;
    private bool isAirDash;
    private bool isAirBreak;
    private bool isGroundSlam;
    //private bool abilityEnabled;
    private bool coolDown;


    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        isFalling = false;
        isIgnition = false;
        abilityEnabled = false;
        coolDown = true;
    }

    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        Ignition();
        DebugSwitch();

        if (isIgnition == true)
        {
            PlayerMove();
        }
    }

    private void FixedUpdate()
    {
        if (isIgnition == true)
        {
            PlayerFalling();
        }
    }

    void Ignition()
    {
        //This starts the movement of the car 
        if (Input.GetKeyDown(KeyCode.Space) && GM.gameOver == false)
        {
            isIgnition = true;
            //Debug.Log("ignited!");
        }
    }

    //Keep this control method works well atm and can be modified for better feel
    void PlayerMove()
    {
        Transform playerTran = gameObject.transform;
        float z = moveSpeed * Time.deltaTime;
        float falling = (fallStrength * Time.deltaTime) * -1;

        //Trying some easing on the horizontal movement
        //playerTran.position += new Vector3((((h*h*h) + (h * horizontalSmoothStep)) * horizontalSpeed) * Time.deltaTime, falling, z);

        //Old method more direct
        playerTran.position += new Vector3((h * horizontalSpeed) * Time.deltaTime, falling, z);

        //Do player ability
        if (Input.GetKeyDown(KeyCode.Q) == true && isFalling == true && abilityEnabled == false)
        {
            abilityEnabled = true;

            StartCoroutine(AbilityCooldDown());
            PlayerAbiltiy();
        }


    }
    
    void PlayerAbiltiy()
    {
        switch(abilitySelect)
        {
            case 3:
                StartCoroutine(GroundSlam());
                print("GroundSlam");
                break;
            case 2:
                StartCoroutine(AirBreak());
                print("AirBreak");
                break;
            case 1:
                StartCoroutine(AirDash());
                print("AirDash");
                break;
            default:
                StartCoroutine(AirDash());
                print("AirDash");
                break;
        }
    }

    //Remove before release
    void DebugSwitch()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            abilitySelect = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            abilitySelect = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            abilitySelect = 3;
        }
    }

    /*
     * I like the feel of this one but need to play around with the prefab
     * settings to get the car reacting right to the movement
     * 
    void PlayerMove()
    {
        //playerRB.velocity = transform.forward * moveSpeed;
        float z = moveSpeed * Time.deltaTime;
        float falling = (fallStrength * Time.deltaTime) * -1;
        transform.position += new Vector3(0f, falling, z);

        playerRB.AddForce(new Vector3(h * test, 0f, 0f));

    }
    */

    void PlayerFalling()
    {
        //Get the y position of the player and normalize it to see if it is falling 
        //on a negative scale when moving on the y axis

        float currentPos = transform.position.y;

        if (currentPos < previousPos)
        {
            isFalling = true;
            fallingCounter += Time.deltaTime;
            //print("isFalling");
        }
        else
        {
            isFalling = false;
            //print("notFalling");
            fallingCounter = 0;
        }

        //reset the previous position with new previouse position
        previousPos = currentPos;
    }

    IEnumerator AbilityCooldDown()
    {
        while (abilityEnabled == true)
        {
            //Added another yield in here that also gives the 
            yield return new WaitForSeconds(coolDownTime);

            abilityEnabled = false;
        }
    }

    IEnumerator AirDash()
    {
        isAirDash = true;
        //print("Dashing");

        float origSpeed = moveSpeed;

        moveSpeed = moveSpeed * airDashMultiplier;

        while (isAirDash == true)
        {
            yield return new WaitForSeconds(airDashDuration);

            moveSpeed = origSpeed;

            isAirDash = false;
            //print("Stopped Dashing");
        }
    }

    IEnumerator AirBreak()
    {
        float origSpeed = moveSpeed;        
        //print("Dashing");

        if (isAirBreak == false)
        {
            moveSpeed = moveSpeed / airBreakAmount;
            isAirBreak = true;
        }

        while (isAirBreak == true)
        {
            yield return new WaitForSeconds(airBreakDuration);

            moveSpeed = origSpeed;

            isAirBreak = false;
            //print("Stopped Dashing");
        }
    }

    IEnumerator GroundSlam()
    {
        isGroundSlam = true;
        //print("Dashing");

        //float origFall = fallStrength;

        //fallStrength = groundSlamAmount;

        //float y = playerRB.velocity.y;

        playerRB.AddForce(0f, groundSlamAmount * -1, 0f);

        while (isGroundSlam == true)
        {
            yield return new WaitForSeconds(groundSlamDuration);


            //playerRB.AddForce(0f, y, 0f);

            
            isGroundSlam = false;
            //print("Stopped Dashing");
        }
    }

}



