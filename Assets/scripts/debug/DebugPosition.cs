using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPosition : MonoBehaviour {


    public Color colorCorrect = Color.cyan;
    public Color colorError = Color.red;
    public float radius = 1f;

    private GameObject tSphere;
    private Vector3 test;

    private float time;

    private void OnDrawGizmos()
    {
        Gizmos.color = colorCorrect;
        Gizmos.DrawSphere(test, radius);
    }
    

}
