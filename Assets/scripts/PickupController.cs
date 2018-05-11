using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {

    public float speed = 2f;
    public float duration = 5f;
    public float pulseSpeed = 10f;

    private Light halo;
    private Color color0;
    private Color color1;

    private void Start()
    {
        halo = GameObject.Find("HaloLight").GetComponent<Light>();
    }

    private void Update()
    {
        float y = (1 * Time.deltaTime) * speed;

        transform.rotation = transform.rotation * Quaternion.Euler(0f, y, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
            
    }
}
