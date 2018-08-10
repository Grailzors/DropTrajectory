using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public float radius = 6.60f;
    public float power = 1200f;
    public float upModifier = 200f;
    public Vector3 posOffset = new Vector3(0f,-2.14f,-2.97f);
    public float destroyObjectTime = 3f;

    private Vector3 explodePoint;

    private void Start()
    {
        explodePoint = gameObject.GetComponent<BoxCollider>().transform.position + gameObject.GetComponent<BoxCollider>().center + posOffset;
    }

    private void OnDrawGizmos()
    {
        explodePoint = gameObject.GetComponent<BoxCollider>().transform.position + gameObject.GetComponent<BoxCollider>().center + posOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(explodePoint, radius);    
    }

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
                //print(child);

                child.GetComponent<Rigidbody>().AddExplosionForce(power, explodePoint, radius);
            }
        }
    }


    IEnumerator DestroyObjectTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(destroyObjectTime);

            DestroyObject(gameObject);
            print("Destroyed: " + gameObject);
        }
    }

}
