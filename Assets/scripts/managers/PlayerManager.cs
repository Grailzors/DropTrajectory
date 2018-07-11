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

    public static float screenFadeValue;
    public static float resetCounter;
    public static float fallTimer;

    private Rigidbody playerRB;    

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        resetCounter = 0f;
    }

    private void LateUpdate()
    {
        ResetPlayerPosition();

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

}
