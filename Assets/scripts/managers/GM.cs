using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

    public int playerLives = 5;
    public int pickupCounter = 0;
    public bool isFinished;

    private void Start()
    {
        //Create the player Controller and Main Camera in the correct location on Start
       
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = GameObject.FindGameObjectWithTag("StartPosition").transform.position;
        
    }



    /*
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
            print(transform.position - startLine.transform.position);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            isFalling = true;

            print("Exit!");
            print(transform.position - startLine.transform.position);
        }
    }
    */

    //Take this out and put else where i



}
