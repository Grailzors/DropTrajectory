using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    [Header("Platform Controls")]
    public GameObject startPlatform;
    public GameObject[] platformPrefabs;
    public int initialPlatformNum = 5;
    public float spawnPlatformTimer = 10;

    /*
    [Header("PickUp Controls")]
    public GameObject[] pickUpPrefabs;
    */

    [Header("SetDec Controls")]
    public GameObject[] setDecPrefabs;
    public float amountPerPlatformMax = 5f;
    public float amountPerPlatformMin = 2f;
    public float positionMultiplier = 80f;

    [Header("Platform Axis Controls")]
    public float platformX = 0f;
    public float platformY = 0f;
    public float platformZ = 0f;

    private GameObject killPlane;
    private GameObject platformsContainer;
    private GameObject pickUpsContainer;
    private GameObject setDecContainer;
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
        if(Input.GetKeyDown(KeyCode.Space) == true && killPlane == null)
        {
            MakeKillPlane();
        }
    }

    void InitialGenerateLevel()
    {
        //creating the container for the platforms generated  
        platformsContainer = new GameObject("PlatformContainer");
        platformsContainer.transform.position = new Vector3(0f, 0f, 0f);

        //creating the container for the pickups generated  
        pickUpsContainer = new GameObject("SetDecContainer");
        pickUpsContainer.transform.position = new Vector3(0f, 0f, 0f);

        //creating the container for the platforms generated  
        setDecContainer = new GameObject("SetDecContainer");
        setDecContainer.transform.position = new Vector3(0f, 0f, 0f);

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
        //Create Platforms through the level and calls function to place setdec around it
        GameObject platform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], new Vector3(x, y, z), Quaternion.identity);
        platform.transform.parent = platformsContainer.transform;

        InstanceSetDec();
    }

    /*
    void InstancePickUp()
    {
        //Create PickUps through the level
        GameObject pickUp = Instantiate(pickUpPrefabs[Random.Range(0, pickUpPrefabs.Length)], new Vector3(x, y, z), Quaternion.identity);
        pickUp.transform.parent = pickUpsContainer.transform;
    }
    */

    void InstanceSetDec()
    {
        //Create a bunch of setdec items around the platforms based on a random range between a max/min value
        for (int i = 0; i < Random.Range(amountPerPlatformMin, amountPerPlatformMax + 1); i++)
        {
            float setDecX = Random.Range((x * positionMultiplier) / 2, x * positionMultiplier);
            float setDecY = Random.Range((y * positionMultiplier) / 2, y * positionMultiplier);
            float setDecZ = Random.Range((z * positionMultiplier) / 2, z * positionMultiplier);

            GameObject setDec = Instantiate(setDecPrefabs[Random.Range(0, setDecPrefabs.Length)], new Vector3(setDecX , setDecY, setDecZ), Quaternion.identity);
            setDec.transform.parent = setDecContainer.transform;
        }
        print("New SetDec");
    }

    void MakeKillPlane()
    {
        killPlane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        killPlane.name = "KillPlane";
        killPlane.layer = 12;
        killPlane.AddComponent<KillPlaneController>();
        killPlane.GetComponent<MeshRenderer>().enabled = false;
        killPlane.GetComponent<Collider>().isTrigger = true;

        killPlane.transform.position = new Vector3(0f, 1000f, 0f);
        killPlane.transform.localScale = new Vector3(9999f, 10f, 9999f);
        
        print("Made Kill Plane");
    }
}

/*
[System.Serializable]
public class RandomPlacement
{


}
*/
