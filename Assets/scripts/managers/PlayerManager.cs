using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    //PLAYER MANAGER NEEDS TO HANDLE PLAYER SPAWNING, RESPAWN & VICTORY/GAMEOVER

    public int playerLives = 5;
    public int playerHealth = 100;
    [HideInInspector]
    public bool isFalling = false;
    [HideInInspector]
    public bool isFinished = false;
    [HideInInspector]
    public bool gameOver = false;

    private Vector3 startLine;
    private GameObject fallingParticleObject;
    private ParticleSystem fallingParticles;
    private GameObject[] tireBurnParticles;
    

    private void Awake()
    {
        fallingParticles = GameObject.FindGameObjectWithTag("FallingParticles").GetComponent<ParticleSystem>();
        tireBurnParticles = GameObject.FindGameObjectsWithTag("BurnOutParticles");
        startLine = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelControllerOLD>().StartLinePos();
    }


    private void Start()
    {
        fallingParticles.Stop();
        transform.position = startLine; 
    }


    private void LateUpdate()
    {
        BurnOut(tireBurnParticles, gameObject.GetComponent<PlayerMovement>().isIgnition);
        GameOver();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FinishLine")
        {
            isFinished = true;
        }
        else if (other.gameObject.tag == "Platform")
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
        if (collision.gameObject.tag == "DamageWall")
        {
            playerHealth -= 1;
            Debug.Log(playerHealth);
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


    private void BurnOut(GameObject[] particles, bool ignition)
    {
        foreach (GameObject particle in particles)
        {
            if (ignition)
            {
                particle.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    
    void GameOver()
    {
        if (isFinished == true)
        {
            //have this trigger the end of the level and load up the next one
            //or maybe cutscene type thing?? 
            transform.position = startLine;
            Debug.Log("Winner");
        }
        else if (playerLives == 0)
        {
            //Have this go to a player death page to restart level
            transform.position = startLine;
            Debug.Log("GAMEOVER!!");
        }
    }
    

}
