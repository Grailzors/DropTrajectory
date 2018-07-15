using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    [Header("Platform Control")]
    public GameObject[] platformPrefabs;
    public GameObject startPlatform;
    public int initialPlatformNum = 5;
    public float spawnPlatformTimer = 10;


    [Header("Platform Axis Controls")]
    public float platformX = 0f;
    public float platformY = 0f;
    public float platformZ = 0f;

    private GameObject killPlane;
    private GameObject platformsContainer;
    private float y = 0f;
    private float x = 0f;
    private float z = 0f;

    private void Start()
    {
        InitialGenerateLevel();
        StartCoroutine(GeneratePlatforms());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) == true)
        {
            MakeKillPlane();
        }
    }

    /*
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
    */

    void InitialGenerateLevel()
    {
        //creating the container for the platforms generated  
        platformsContainer = new GameObject("PlatformContainer");
        platformsContainer.transform.position = new Vector3(0f, 0f, 0f);

        //Setting the starting track 
        GameObject initialPlatform = Instantiate(startPlatform, new Vector3(), Quaternion.identity);
        initialPlatform.transform.parent = platformsContainer.transform;

        if (initialPlatform != null)
        {
            for (int i = 0; i < initialPlatformNum; i++)
            {
                SpawnAxisUpdate();
                InstancePlatform();
            }
        }

        print("Finished Initial Generation");
    }
    
    //Generating a new platform ever 'x' seconds after initial level set up
    IEnumerator GeneratePlatforms()
    {
        print("Procedural Generation Started");

        while (GM.gameOver == false)
        {
            yield return new WaitForSeconds(Mathf.Clamp(spawnPlatformTimer - PlayerMovement.fallingCounter, 0.1f, spawnPlatformTimer));

            SpawnAxisUpdate();
            InstancePlatform();
            print("New Platform");
        }

        print("GameOver level generation stopped");
    }

    void SpawnAxisUpdate()
    {
        x = Random.Range(platformX * -1, platformX);
        y += Random.Range(platformY - (platformY / 2), platformY);
        z += Random.Range(platformZ - (platformZ / 2), platformZ);
    }
    
    void InstancePlatform()
    {
        GameObject platform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], new Vector3(x, y, z), Quaternion.identity);
        platform.transform.parent = platformsContainer.transform;
    }

    void MakeKillPlane()
    {
        killPlane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        killPlane.name = "KillPlane";
        killPlane.AddComponent<KillPlaneController>();
        killPlane.GetComponent<MeshRenderer>().enabled = false;
        killPlane.GetComponent<Collider>().isTrigger = true;
        killPlane.transform.position = new Vector3(0f, 1000f, 0f);
        killPlane.transform.localScale = new Vector3(4000f, 1f, 4000f);

        print("Made Kill Plane");
    }
}
