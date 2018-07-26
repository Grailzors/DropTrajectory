using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPosition : MonoBehaviour {


    public Color colorCorrect = Color.cyan;
    public Color colorError = Color.red;
    public float radius = 1f;


    private void OnDrawGizmos()
    {
        Gizmos.color = colorCorrect;
        Gizmos.DrawSphere(transform.position, radius);
    }

}
