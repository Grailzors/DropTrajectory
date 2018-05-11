using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlaneController : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = GameObject.Find("Respawn").transform.position;    
        //DestroyObject(other);
    }

}
