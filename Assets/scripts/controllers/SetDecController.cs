using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDecController : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Platform" || other.tag == "PickUp" || other.tag == "Bank" || other.tag == "StartLine")
        {
            //print("SetDec Inside Object Deleting");
            DestroyObject(gameObject);
            //print("Done");
        }
    }
}
