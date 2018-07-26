using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPosition : MonoBehaviour {

    public GameObject ancorPoint;

    public float distanceMax = 0f;
    public float distanceMin = 0f;

    public Color colorCorrect = Color.cyan;
    public Color colorError = Color.red;
    public float radius = 1f;

    private bool isFar = false;

    private void OnDrawGizmos()
    {
        GetDistance();

        if (isFar == false)
        {
            Gizmos.color = colorCorrect;
        }
        else
        {
            Gizmos.color = colorError;
        }

        Gizmos.DrawSphere(transform.position, radius);
    }


    void GetDistance()
    {
        float d = Vector3.Distance(ancorPoint.transform.position, transform.position);

        if (d < distanceMin || d > distanceMax)
        {
            isFar = true;
        }
        else
        {
            isFar = false;
        }
    }   
}
