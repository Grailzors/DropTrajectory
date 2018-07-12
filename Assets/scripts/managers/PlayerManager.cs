using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    //PLAYER MANAGER NEEDS TO HANDLE PLAYER RESPAWN
    [Header("Player Components")]
    public GameObject player;

    [Header("Player Variables")]
    public int playerLives;
    public float fallTimeOut = 0f;

    [Header("Player Particle Systems")]
    public ParticleSystem[] fallParticles;
    public ParticleSystem[] tireSmoke;

    public static float screenFadeValue;
    public static float resetCounter;
    public static float fallTimer;

    private Rigidbody playerRB;    

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        resetCounter = 0f;

        PlayTireSmoke();
    }

    private void LateUpdate()
    {
        ResetPlayerPosition();
        PlayFallingParticles();

        fallTimer = fallTimeOut;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PickUp")
        {
            GM.score += 1;
        }
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
            smoke.Stop();
            
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
}
