using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public int platformNum = 5;

    private GameObject[] platformPrefabs;
    private Vector3 startJoint;
    //private Vector3 finishJoint;

    private Vector3 lastJoint;
    private int lastPlatformType;
    private int indexNum;
    private int hillCounter;



    private void Awake()
    {
        platformPrefabs = GameObject.FindGameObjectsWithTag("Platform");
        startJoint = new Vector3();

        //startJoint = GameObject.FindGameObjectWithTag("StartPosition").transform.parent.gameObject.transform.GetChild(1).position;
        //finishJoint = GameObject.FindGameObjectWithTag("FinishLine").transform.parent.gameObject.transform.GetChild(1).position;
    }


    private void Start()
    {
       GenerateLevel();
    }


    void GenerateLevel()
    {
        
        
        //creating the container for the platforms generated 
        GameObject platformsContainer = new GameObject("PlatformContainer");
        platformsContainer.transform.position = new Vector3(0f, 0f, 0f);

        //Setting the starting track
        GameObject startPlatform = Instantiate(GameObject.FindGameObjectWithTag("StartLine"), startJoint, Quaternion.identity);
        startPlatform.transform.parent = platformsContainer.transform;

        //stores the position of the back joint from the last platform
        lastJoint = startPlatform.transform.GetChild(0).position;
        lastPlatformType = startPlatform.layer;

        for (int num = 0; num < platformNum + 1; num++)
        {
            //NEED TO MAKE THIS WORK WITH THE INTERSECTION PLATFORMS

            if (lastPlatformType == 8 || lastPlatformType == 10) //Checking if lastPlatformType was the start or stright
            {
                indexNum = 1; //Choose a hill platform
                hillCounter += 1;
            }
            else if (hillCounter == 3)
            {
                indexNum = 0; //Choose randomly between hill or straight
                hillCounter = 0;
            }
            else if (lastPlatformType == 11) //Checking if lastPlatformType was a hill
            {
                indexNum = Random.Range(0,2); //Choose randomly between hill or straight

                if (indexNum == 1)
                {
                    hillCounter += 1;
                }
            }

            Debug.Log(hillCounter);
            //Debug.Log(indexNum);

            //Create an new platform randomly from the platformPrefabs array
            GameObject platform = platformPrefabs[indexNum];
            GameObject platformClone = Instantiate(platform, lastJoint, Quaternion.identity);
            platformClone.transform.parent = platformsContainer.transform;

            lastJoint = platformClone.transform.GetChild(0).position;
            lastPlatformType = platformClone.layer;
        }

        GameObject finishPlatform = Instantiate(GameObject.FindGameObjectWithTag("Finish"), lastJoint, Quaternion.identity);
        finishPlatform.transform.parent = platformsContainer.transform;

    }
}
