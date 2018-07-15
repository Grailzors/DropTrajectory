using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public float destroyObjectTime = 3f;

    private void OnTriggerEnter(Collider other)
    {
        //Go through and tell all child objects of the pick up to ignore the plow collider
        if (other.gameObject.tag == "Player")
        {
            print("Hit Player");

            StartCoroutine(DestroyObjectTimer());

            GameObject player = other.gameObject;
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                print(child);

                if (other.gameObject.name != "Player")
                {
                    Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), child.GetComponent<Collider>());
                }
                    
            }
        }
    }


    IEnumerator DestroyObjectTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(destroyObjectTime);

            DestroyObject(gameObject);
        }
    }

}
