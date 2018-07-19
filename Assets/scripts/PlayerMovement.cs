using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float test = 0f;

    [Header("Control Variables")]
    public float moveSpeed = 5f;
    public float horizontalSpeed = 5f;

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

    void PlayerMove()
    {
        float pAngle;
        Vector3 pAxis;

        playerRB.velocity = transform.forward * moveSpeed;

        transform.rotation.ToAngleAxis(out pAngle, out pAxis);
        
        print(playerRB.velocity.sqrMagnitude);

        playerRB.AddForce(new Vector3(h * 5000, 0f, 0f));
        

    }

    /*
    //Keep this control method works well atm and can be modified for better feel
    void PlayerMove()
    {
        Transform playerTran = gameObject.transform;
        float z = moveSpeed * Time.deltaTime;
        float falling = (fallStrength * Time.deltaTime) * -1;

        playerTran.position += new Vector3((h * horizontalSpeed), falling, z);
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



