using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPosition : MonoBehaviour {

    public Color color = Color.red;
    public float radius = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
