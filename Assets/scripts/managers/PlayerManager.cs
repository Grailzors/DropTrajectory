using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [Header("Player Components")]

    [Header("Player Variables")]
    public int playerLivesLimit;
    public float fallTimeOut = 0f;
    public float rayLength = 500f;
    public float rayTargetSize = 5f;

    [Header("Player Particle Systems")]
    public ParticleSystem[] fallParticles;
    public ParticleSystem[] tireSmoke;

    [Header("Score Multiplier")]
    public float bounceAmount = 1f;
    public float coolDownTimer = 2f;
    public float coolDownSpeed = 0.5f;

    public static GameObject player;
    public static float playerScore;
    public static float multiplierScore;
    public static float multiplierTimer;
    public static float screenFadeValue;
    public static float resetCounter;
    public static float fallTimer;

    private Rigidbody playerRB;
    private int multiplierCollisions;

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
        MultiplyUpdate();

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
        MultiplierIncrease(col);
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

    //Controlls passing the collect points of the character to the GM script
    void BankScore(Collider other)
    {
        if (other.tag == "Bank")
        {
            GM.bankScore += playerScore * multiplierScore;
            playerScore = 0;
            GM.isBanked = true;
            multiplierTimer = 0f;
            multiplierScore = 0f;
        }
    }

    //Controls the amount of points the character has collected so far before banking
    void PlayerScore(Collider other)
    {
        if (other.tag == "PickUp")
        {
            multiplierTimer = coolDownTimer;
            multiplierCollisions += 1;
            playerScore += 1;
            GM.isBanked = true;
        }
    }

    void MultiplierIncrease(Collision col)
    {
        if (col.gameObject.tag == "Platform" || col.gameObject.tag == "PickUp")
        {
            if (multiplierTimer > 0)
            {
                //increase multiplier timer per bounce/hit of a platform
                multiplierTimer += bounceAmount;
                multiplierTimer = Mathf.Clamp(multiplierTimer, 0, coolDownTimer);
            }
        }
    }

    void MultiplyUpdate()
    {
        //Limit the amount of collisions detected so that the multiplier doesn't go above 
        //a value of 30
        multiplierCollisions = Mathf.Clamp(multiplierCollisions, 0, 5);

        if (multiplierTimer > 0f)
        {
            //cooldown for the multiplier effect
            multiplierTimer -= coolDownSpeed * Time.deltaTime;

            //Set multiplier score
            switch (multiplierCollisions)
            {
                case 5:
                    multiplierScore = 30f;
                    break;
                case 4:
                    multiplierScore = 15f;
                    break;
                case 3:
                    multiplierScore = 5f;
                    break;
                case 2:
                    multiplierScore = 2f;
                    break;
                case 1:
                    multiplierScore = 1.5f;
                    break;
                default:
                    multiplierScore = 1f;
                    break;
            }
        }
        else if (multiplierTimer <= 0f)
        {
            multiplierTimer = 0f;
            multiplierScore = 0f;
        }

        print(multiplierScore);

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
            multiplierTimer = 0f;
            multiplierScore = 0f;
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
