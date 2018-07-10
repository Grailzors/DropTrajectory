using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    [Header("Platform Control")]
    public GameObject[] platformPrefabs;
    public GameObject startPlatform;
    public int platformNum = 5;

    [Header("Platform Axis Controls")]
    public float platformX = 0f;
    public float platformY = 0f;
    public float platformZ = 0f;


    private void Start()
    {
       GenerateLevel();
    }

    //need to make this procedural

    void GenerateLevel()
    {
        float y = 0f;
        float x = 0f;
        float z = 0f;

        //creating the container for the platforms generated  
        GameObject platformsContainer = new GameObject("PlatformContainer");
        platformsContainer.transform.position = new Vector3(0f, 0f, 0f);

        //Setting the starting track 
        GameObject initialPlatform = Instantiate(startPlatform, new Vector3(), Quaternion.identity);
        initialPlatform.transform.parent = platformsContainer.transform;

        if (initialPlatform != null)
        {
            for (int i = 0; i < platformNum; i++)
            {
                x = Random.Range(platformX * -1, platformX);
                y += platformY;
                z += platformZ;

                GameObject platform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], new Vector3(x, y, z), Quaternion.identity);
                platform.transform.parent = platformsContainer.transform;
            }
        }
        


    }

}
