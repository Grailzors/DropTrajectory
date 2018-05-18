using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControllerOLD : MonoBehaviour {

    public int platformNum = 12;
 
    private GameObject startLine;
    private GameObject finishLine;
    private GameObject platformPrefab;
    private GameObject finishPlatform;
    private int platformX;
    private int platformY;
    private int platformZ;
    

    private void Awake()
    {
        startLine = GameObject.FindWithTag("StartPosition");
        finishLine = GameObject.FindWithTag(("FinishLine"));

        //Change this to be created from the resources folder
        platformPrefab = GameObject.Find("Platform1");
        finishPlatform = GameObject.Find("FinishPlatform");
        platformZ = 430;
        platformY = -80;
    }

    // Use this for initialization
    void Start ()
    {
        LevelSpawn();
	}
	
    void LevelSpawn()
    {

        GameObject platformGroup = new GameObject("PlatformGroup");

        for (int i = 0; i < platformNum + 1; i++)
        {

            platformX = Random.Range(-90, 90);

            Vector3 platformPos = new Vector3(platformX, platformY, platformZ);

            var newPlatform = Instantiate(platformPrefab, new Vector3(platformX, platformY, platformZ), Quaternion.identity);

            newPlatform.transform.parent = platformGroup.transform;

            platformZ += platformZ;
            platformY += platformY;

            print(platformY);
            print(platformY);

        }

        var finishPlat = Instantiate(Instantiate(finishPlatform, new Vector3(platformX, platformY, platformZ), Quaternion.identity));

        finishPlat.transform.parent = platformGroup.transform;

        KillPlane(platformX, platformY, platformZ);

    }

    void KillPlane(float x, float y, float z)
    {
        GameObject killPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
        killPlane.name = "KillPlane";
        killPlane.AddComponent<KillPlaneController>();
        killPlane.GetComponent<MeshCollider>().convex = enabled;
        killPlane.GetComponent<MeshCollider>().isTrigger = enabled;
        killPlane.GetComponent<MeshRenderer>().enabled = false;

        killPlane.transform.position = new Vector3(0f, y * 3, 0f);
        killPlane.transform.rotation = Quaternion.Euler(90, 0, 0);
        killPlane.transform.localScale = new Vector3(x * 100, z * 100, 0f);

    }


    public Vector3 StartLinePos()
    {
        return startLine.transform.position;
    }

    public Quaternion StartLineRot()
    {
        return startLine.transform.rotation;
    }
    
    public Vector3 FinishLinePos()
    {
        return finishLine.transform.position;
    }

    public Quaternion FinishLineRot()
    {
        return finishLine.transform.rotation;
    }
  
    /*
    public Dictionary<string, object> StartLine()
    {
        Dictionary<string, object> startLineDict = new Dictionary<string, object>();

        startLineDict.Add("StartPos", startLine.transform.position);
        startLineDict.Add("StartRot", startLine.transform.rotation);

        return startLineDict;
    }


    public Dictionary<string, object> FinishLine()
    {
        Dictionary<string, object> finishLineDict = new Dictionary<string, object>();

        finishLineDict.Add("FinishPos", finishLine.transform.position);
        finishLineDict.Add("FinishRot", finishLine.transform.rotation);

        return finishLineDict;
    }
    */


}
