using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    //PLAYER MANAGER NEEDS TO HANDLE PLAYER SPAWNING, RESPAWN & VICTORY/GAMEOVER

    //TestLine


    public float playerHealth = 100f;
    [HideInInspector]
    public bool isFalling = false;
    [HideInInspector]
    public bool isFinished = false;

    private Vector3 startLine;
    private GameObject fallingParticleObject;
    private ParticleSystem fallingParticles;
    private GameObject[] tireBurnParticles;
    

    private void Awake()
    {
        fallingParticles = GameObject.FindGameObjectWithTag("FallingParticles").GetComponent<ParticleSystem>();
        tireBurnParticles = GameObject.FindGameObjectsWithTag("BurnOutParticles"); 
    }


    private void Start()
    {
        fallingParticles.Stop();
        BurnOut(tireBurnParticles, false);

        //startLine = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().StartLinePos();      
    }


    private void LateUpdate()
    {
        //Cycle through an array of particles and get burnout bool from playmovement script
        BurnOut(tireBurnParticles, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementOld>().burnOut);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            fallingParticles.Stop();
            Respawn();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {   
            fallingParticles.Play();
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        WallCollision(collision);
    }


    void WallCollision(Collision collision)
    {
        Debug.Log(playerHealth);

        if (collision.gameObject.tag == "DamageWall")
        {
            playerHealth -= 0.8f;
        }
    }


    void Respawn()
    {
        if(GameObject.Find("Respawn") != null)
        {
            Destroy(GameObject.Find("Respawn"));
        }

        GameObject respawnPos = new GameObject("Respawn");

        respawnPos.transform.position = transform.position;
        respawnPos.transform.rotation = Quaternion.identity;

    }


    void IsFalling(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            isFalling = false;

            print("Entered!");
            print(transform.position - startLine);
        }
    }


    void BurnOut(GameObject[] particleObj, bool burnOut)
    {
        if (burnOut == false)
        {
            for (int i = 0; i < particleObj.Length; i++)
            {
                particleObj[i].GetComponent<ParticleSystem>().Stop();
            }
        }
        else
        {
            for (int i = 0; i < particleObj.Length; i++)
            {
                particleObj[i].GetComponent<ParticleSystem>().Play();
            }
        }
    }

    /*
    void GameOver()
    {
        isFinished = true;

        //Off for testing
        //player.transform.position = finishPos;

        player.transform.position = startLine.transform.position;

        Debug.Log(finishPos);
    }
    */

}
