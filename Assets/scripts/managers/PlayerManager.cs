using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [Header("Player Components")]
    public GameObject rayPoint;

    [Header("Player Variables")]
    public int playerLivesLimit;
    public float fallTimeOut = 0f;
    public float rayLength = 500f;
    public float rayTargetSize = 5f;

    [Header("Player Particle Systems")]
    public ParticleSystem[] fallParticles;
    public ParticleSystem[] tireSmoke;

    public static GameObject player;
    public static int playerScore;
    public static float screenFadeValue;
    public static float resetCounter;
    public static float fallTimer;

    private Rigidbody playerRB;
    //private GameObject rayTarget;

    private void Start()
    {
        player = gameObject;
        playerRB = GetComponent<Rigidbody>();
        GM.playerLives = playerLivesLimit;
        resetCounter = 0f;

        MoveToStartPos();
    }

    private void Update()
    {
        //FallTarget();
        RespawnPlayer();
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
        PlayerScore(other);
        BankScore(other);
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
            //Check to see if a respawn point exists and delete it if it does
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

    void BankScore(Collider other)
    {
        if (other.tag == "Bank")
        {
            GM.bankScore += playerScore;
            playerScore = 0;
            GM.isBanked = true;
        }
    }

    void PlayerScore(Collider other)
    {
        if (other.tag == "PickUp")
        {
            playerScore += 1;
            GM.isBanked = true;
        }
    }

    void MoveToStartPos()
    {
        transform.position = GameObject.FindGameObjectWithTag("StartPosition").transform.position;
    }

    void RespawnPlayer()
    {
        //Using this method as I think i will be destroying/instanciting the spawn point
        GameObject resetPoint = GameObject.FindGameObjectWithTag("Respawn");

        //Reset Player position to a respawn marker
        if (resetCounter > fallTimeOut)
        {
            //print("Respawn");
            transform.position = resetPoint.transform.position;
            PlayerMovement.isIgnition = false;
            PlayerMovement.isFalling = false;
            playerRB.velocity = new Vector3();
            GM.playerLives -= 1;
            playerScore = 0;
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
        }
    }
}
