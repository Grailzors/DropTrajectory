using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Control Variables")]
    public float moveSpeed = 5f;
    public float horizontalSpeed = 5f;
    public float horizontalSmoothStep = 0f;

    private float fallStrength;
    private Rigidbody playerRB;
    private float previousPos;

    public static float h;
    public static bool isFalling;
    public static float fallingCounter;
    public static bool isIgnition;


    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        isFalling = false;
        isIgnition = false;
    }

    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        Ignition();

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isIgnition = true;
            Debug.Log("ignited!");
        }
    }

    
    //Keep this control method works well atm and can be modified for better feel
    void PlayerMove()
    {
        Transform playerTran = gameObject.transform;
        float z = moveSpeed * Time.deltaTime;
        float falling = (fallStrength * Time.deltaTime) * -1;

        //Trying some easing on the horizontal movement
        playerTran.position += new Vector3((((h*h*h) + (h * horizontalSmoothStep)) * horizontalSpeed ), falling, z);
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
}



