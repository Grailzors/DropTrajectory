using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlaneController : MonoBehaviour {

    private Vector3 offSet;

    private void Start()
    {
        offSet = transform.position - PlayerManager.player.transform.position;
    }

    //need to make this procedural
    private void LateUpdate()
    {
        KillPlaneMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        //Destroy gameObject top node so hole asset is destroyed
        DestroyObject(other.gameObject.transform.parent.gameObject);
    }

    //Move the killPlane in relation to the player
    void KillPlaneMove()
    {
        transform.position = PlayerManager.player.transform.position + offSet;
    }

}
