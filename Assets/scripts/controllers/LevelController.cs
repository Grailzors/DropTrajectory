using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    [Header("Platform Controls")]
    public GameObject startPlatform;
    public GameObject[] platformPrefabs;
    public GameObject[] bankPrefabs;
    public int initialPlatformNum = 5;
    public int platformLimit = 35;
    public float spawnPlatformTimer = 10;
    public int spawnBank = 0;

    [Header("Platform Axis Controls")]
    public float platformX = 0f;
    public float platformY = 0f;
    public float platformZ = 0f;

    [Header("PickUp Controls")]
    public GameObject[] pickUpPrefabs;
    [Range(0,100)]
    public int pickUpSpawnPercentage = 50;
    public int numOfConsecutivePickUps = 2;
    public float pickUpsPerPlatformMax = 5f;
    public float pickUpsPerPlatformMin = 2f;

    [Header("SetDec Controls")]
    public GameObject[] setDecPrefabs;
    public float setDecPerPlatformMax = 5f;
    public float setDecPerPlatformMin = 2f;
    public float setDecPositionMultiplier = 80f;
    public float setDecDistanceMax = 0f;
    public float setDecDistanceMin = 0f;

    private GameObject killPlane;
    private GameObject platformsContainer;
    private GameObject pickUpsContainer;
    private GameObject setDecContainer;
    private float y = 0f;
    private float x = 0f;
    private float z = 0f;
    private int numPickUps = 0;
    private int platformCount = 0;


    private void Awake()
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
        pickUpsContainer = new GameObject("PickUpsContainer");
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
                PopulateLevel();
            }
        }

        //print("Finished Initial Generation");
    }
    

    //Generating a new platform ever 'x' seconds after initial level set up
    IEnumerator GeneratePlatforms()
    {
        //print("Procedural Generation Started");        

        while (GM.gameOver == false)
        {
            yield return new WaitForSeconds(Mathf.Clamp(spawnPlatformTimer - PlayerMovement.fallingCounter, 0.1f, spawnPlatformTimer));

            print(platformsContainer.transform.childCount);

            if (platformsContainer.transform.childCount < platformLimit)
            {
                PopulateLevel();
            }
            
        }

        //print("GameOver level generation stopped");
    }


    void SpawnAxisUpdate()
    {
        //Update the X,Y,Z varaiables (asset placement in world)
        x = Random.Range(platformX * -1, platformX);
        y += Random.Range(platformY - (platformY / 2), platformY);
        z += Random.Range(platformZ - (platformZ / 2), platformZ);
    }
    

    //Populate the Level using this chunk of code
    void PopulateLevel()
    {
        //Update the X,Y,Z position for the next plaform or pickUp
        SpawnAxisUpdate();

        //Choose what type of prefab to instanciate between platform or pickUp
        int randNum = Random.Range(0, 100);

        //Here i am forcing a platform to be made if the numPickUps matches my 
        //numOfConsecutivePickUps threshold
        if (randNum > pickUpSpawnPercentage || numPickUps >= numOfConsecutivePickUps)
        {
            numPickUps = 0;
            InstancePlatform();
        }
        else if (randNum <= pickUpSpawnPercentage)
        {
            numPickUps += 1;
            InstancePickUp();
        }

        InstanceSetDec();
    }


    void InstancePlatform()
    {
        
        if (platformCount < spawnBank)
        {
            //Create Platforms through the level and calls function to place setdec around it
            GameObject platform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], new Vector3(x, y, z), Quaternion.identity);
            platform.transform.parent = platformsContainer.transform;

            platformCount += 1;

            //print("New Platform");
        }
        else
        {
            //Create a new bank for every 'x' amount of platforms 
            GameObject bank = Instantiate(bankPrefabs[Random.Range(0, bankPrefabs.Length)], new Vector3(x, y, z), Quaternion.identity);
            bank.transform.parent = platformsContainer.transform;

            platformCount = 0;

            //print("New Bank");
        }
    }


    void InstancePickUp()
    {
        //Create pickUps around the platforms based on a random range between a max/min value
        for (int i = 0; i < Random.Range(pickUpsPerPlatformMin, pickUpsPerPlatformMax); i++)
        {
            //Create PickUps through the level
            GameObject pickUp = Instantiate(pickUpPrefabs[Random.Range(0, pickUpPrefabs.Length)], new Vector3(x, y, z), Quaternion.identity);
            pickUp.transform.parent = pickUpsContainer.transform;

            //print("New PickUp");
        }
    }
    

    void InstanceSetDec()
    {
        //Create a bunch of setdec items around the platforms based on a random range between a max/min value
        for (int i = 0; i < Random.Range(setDecPerPlatformMin, setDecPerPlatformMax + 1); i++)
        {
            float setDecX = Random.Range((x + setDecPositionMultiplier) / 2, x + setDecPositionMultiplier);
            float setDecY = Random.Range((y + setDecPositionMultiplier) / 2, y + setDecPositionMultiplier);
            float setDecZ = Random.Range((z + setDecPositionMultiplier) / 2, z + setDecPositionMultiplier);

            float d = Vector3.Distance(new Vector3(x, y, z), new Vector3(setDecX, setDecY, setDecZ));

            if (d < setDecDistanceMin || d > setDecDistanceMax)
            {
                //Create PickUps through the level
                GameObject setDec = Instantiate(setDecPrefabs[Random.Range(0, setDecPrefabs.Length)], new Vector3(setDecX, setDecY, setDecZ), Quaternion.identity);
                setDec.transform.parent = setDecContainer.transform;

                //print("New setDec");
            }
            else
            {
                //print("No setDec");
            }
        }
    }


    void MakeKillPlane()
    {
        //Create then modify the killPlane gameObject
        killPlane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        killPlane.name = "KillPlane";
        killPlane.layer = 12;
        killPlane.AddComponent<KillPlaneController>();
        killPlane.GetComponent<MeshRenderer>().enabled = false;
        killPlane.GetComponent<Collider>().isTrigger = true;

        killPlane.transform.position = new Vector3(0f, 3000f, 0f);
        killPlane.transform.localScale = new Vector3(999999f, 100f, 999999f);
        
        //print("Made Kill Plane");
    }
}