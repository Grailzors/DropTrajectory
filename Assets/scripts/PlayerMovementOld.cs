using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOld : MonoBehaviour {

    public float finishDelay = 5f;
    public float moveSpeed = 80f;
    public float acceleration = 5f;
    public float rotationSpeed = 3f;
    public float latSpeed = 2f;
    public float angleMult = 5f;
    public float gravityMult = 1.1f;
    [HideInInspector]
    public bool burnOut;

    private GameObject player;
    private Vector3 startLinePos;
    private Quaternion startLineRot;
    private Vector3 finishLinePos;
    private bool isFalling;
    private bool isFinished;
    private float fallingRot;
    private float h;
    private float ignitionCounter;
     


    private void Awake()
    {
        GameObject playerManager = GameObject.FindGameObjectWithTag("Player");

        player = GameObject.FindWithTag("Player");

        isFalling = playerManager.GetComponent<PlayerManager>().isFalling;
        isFinished = playerManager.GetComponent<PlayerManager>().isFinished;
        fallingRot = 0f;
        burnOut = false;

    }


    private void Start()
    {
        GameObject levelController = GameObject.FindGameObjectWithTag("LevelController");
        
        startLinePos = levelController.GetComponent<LevelController>().StartLinePos();
        startLineRot = levelController.GetComponent<LevelController>().StartLineRot();
        finishLinePos = levelController.GetComponent<LevelController>().FinishLinePos();

        player.transform.position = startLinePos;
        player.transform.rotation = startLineRot;
    }


    private void Update()
    {
        h = Input.GetAxis("Horizontal");

        PlayerFalling();
        Move();

    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FinishLine")
        {
            print("GAMEOVER!!!!");

            GameOver();
        }
        else if (other.gameObject.tag == "Platform")
        {
            isFalling = false;

            print("Entered!");
            print(transform.position - startLinePos);
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            isFalling = true;

            print("Exit!");
            print(transform.position - startLinePos);
        }
    }


    void PlayerFalling()
    {
        if (isFalling == true)
        {
            fallingRot += (Time.deltaTime * -1) * rotationSpeed;
            fallingRot = Mathf.Clamp(fallingRot, 0, 70);
            //Debug.Log(fallingRot);
        }
        else
        {
            fallingRot = (0f * Time.deltaTime) * 100f;
        }

    }


    bool Ignition()
    {
        bool ignition;

        if (ignitionCounter > 10f)
        {
            ignition = true;
            burnOut = false;
        }
        else 
        {
            if (Input.GetButton("Jump") == true)
            {
                ignitionCounter += (1.4f * Time.deltaTime) * acceleration;
                burnOut = true;
            }
            else
            {
                ignitionCounter = Mathf.Clamp(ignitionCounter - (10f * Time.deltaTime), 0f, 10f);
                burnOut = false;
            }
            ignition = false;
        }
        return ignition;
    }


    void Move()
    {
        /*
        if (isFinished == true)
        {
            moveSpeed = 0f;
        }
        */

        //Ignition to start
        if (Ignition() == true)
        {
            player.GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed;
            transform.rotation = Quaternion.Euler(fallingRot, Mathf.Clamp(h * angleMult, -45, 45), 0f);
        }
    }


    //Take this out and put else where i
    void GameOver()
    {
        isFinished = true;

        //Off for testing
        //player.transform.position = finishPos;

        player.transform.position = startLinePos;

        Debug.Log(finishLinePos);
    }
}
