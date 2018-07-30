﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [Header("Player Components")]
    public GameObject rayPoint;

    [Header("Player Variables")]
    public int playerLives;
    public float fallTimeOut = 0f;
    public float rayLength = 500f;
    public float rayTargetSize = 5f;

    [Header("Player Particle Systems")]
    public ParticleSystem[] fallParticles;
    public ParticleSystem[] tireSmoke;

    public static GameObject player;
    public static float screenFadeValue;
    public static float resetCounter;
    public static float fallTimer;

    private Rigidbody playerRB;
    private GameObject rayTarget;


    private void Start()
    {
        player = gameObject;
        playerRB = GetComponent<Rigidbody>();
        resetCounter = 0f;

        MoveToStartPos();
    }

    private void Update()
    {
        //FallTarget();
        ResetPlayerPosition();
    }

    private void LateUpdate()
    {
        //ResetPlayerPosition();
        PlayFallingParticles();

        //Maybe find a better way for this to be done? coroutine?
        PlayTireSmoke();

        fallTimer = fallTimeOut;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PickUp")
        {
            GM.score += 1;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        ResetSpawnPos(col);
    }

    void ResetSpawnPos(Collision col)
    {
        GameObject colObject = col.gameObject;

        if (colObject.tag == "Platform")
        {
            //Check to see if a repawn point exists and delete it if it does
            if (GameObject.FindGameObjectWithTag("Respawn") != null)
            {
                //print(GameObject.FindGameObjectWithTag("Respawn"));

                DestroyObject(GameObject.FindGameObjectWithTag("Respawn"));
            }

            //Create respawn object 
            GameObject respawnPoint = new GameObject
            {
                name = "RespawnPoint",
                tag = "Respawn"
            };

            //Get the reset position on the platform
            Vector3 r = colObject.transform.Find("RespawnPoint").transform.position;
            print("Found Respawn Point");

            respawnPoint.transform.position = r;
        }
    }

    void MoveToStartPos()
    {
        transform.position = GameObject.FindGameObjectWithTag("StartPosition").transform.position;
    }


    void ResetPlayerPosition()
    {
        //Using this method as I think i will be destroying/instanciting the spawn point
        GameObject resetPoint = GameObject.FindGameObjectWithTag("Respawn");

        //Reset Player position to a respawn marker
        if (resetCounter > fallTimeOut)
        {
            print("Respawn");
            transform.position = resetPoint.transform.position;
            PlayerMovement.isIgnition = false;
            PlayerMovement.isFalling = false;
            playerRB.velocity = new Vector3();
            playerLives -= 1;
        }

        //Set and reset counter depending if player is falling
        if (PlayerMovement.isFalling != true)
        {
            resetCounter = 0f;
        }
        else
        {
            resetCounter += Time.deltaTime;
        }
    }
    
    void PlayFallingParticles()
    {
        foreach (ParticleSystem particles in fallParticles)
        {
            if (PlayerMovement.isFalling == false)
            {
                particles.Stop();
            }
            else
            {
                particles.Play();
            }
        }
    }

    //Move from Start to lateupdate when the burnout stuff is sorted out
    void PlayTireSmoke()
    {
        foreach (ParticleSystem smoke in tireSmoke)
        {
            if (PlayerMovement.isIgnition == true)
            {
                smoke.Stop();
            }
            else if (PlayerMovement.isIgnition == false)
            {
                smoke.Play();
            }

            /*
             * Currently off because it will be tied in with a burnout before the car goes 
            if (PlayerMovement.isFalling == false)
            {
                smoke.Stop();
            }
            else
            {
                smoke.Play();
            }
            */
        }
    }

    /*
    void FallTarget()
    {
        if (PlayerMovement.isFalling == true)
        {
            RaycastHit hit;

            if (rayTarget == null)
            {
                rayTarget = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                rayTarget.GetComponent<SphereCollider>().enabled = false;
                rayTarget.transform.localScale = new Vector3(rayTargetSize, rayTargetSize, rayTargetSize);
                rayTarget.GetComponent<Renderer>().material.color = Color.red;
            }

            //Cast ray to see where the car will fall
            if (Physics.Raycast(rayPoint.transform.position, rayPoint.transform.forward, out hit, rayLength))
            {
                //draw the sphere on the objects it collides with or draw sphere at the end of the ray
                Debug.DrawRay(rayPoint.transform.position, rayPoint.transform.forward * hit.distance, Color.green);

                //Move rayTarget
                rayTarget.transform.position = hit.point;
            }
            else
            {
                Debug.DrawRay(rayPoint.transform.position, rayPoint.transform.forward * rayLength, Color.white);

                rayTarget.transform.position = rayPoint.transform.position + rayPoint.transform.forward * rayLength;
            } 
        }
        else
        {
            DestroyObject(rayTarget);
        }
    }
    */
}
