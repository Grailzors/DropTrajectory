using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float donutSpeed = 0f;

    private GameObject player;
    private Transform donutPiv;
    private GameObject[] backWheels;
    private GameObject[] frontWheels;
    private float h;
    private bool isIgnition;
   

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        donutPiv = player.transform.GetChild(4);
        backWheels = GameObject.FindGameObjectsWithTag("BackWheels");
        frontWheels = GameObject.FindGameObjectsWithTag("FrontWheels");
        
        isIgnition = false;
    }


    private void Start()
    {
        
            
    }


    private void Update()
    {
        h = Input.GetAxis("Horizontal");

        //Figure out a way to get the rotation to increase based on 
        //the angle of the car along the donut route       
        //donutSpeed = donutSpeed + Time.deltaTime;

        Ignition();

    }


    private void Ignition()
    {
        print(donutPiv.rotation * Quaternion.Euler(0f, h, 0f));

        donutPiv.rotation = donutPiv.rotation * Quaternion.Euler(0f,h * donutSpeed,0f);
    }



}
