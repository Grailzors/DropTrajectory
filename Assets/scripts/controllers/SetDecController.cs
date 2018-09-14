using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDecController : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Platform" || other.tag == "PickUp" || other.tag == "Bank" || other.tag == "StartLine")
        {
            //print("SetDec Inside Object Deleting");
            Object.Destroy(gameObject);
            //print("Done");
        }
    }
}
