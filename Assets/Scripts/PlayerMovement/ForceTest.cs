using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class ForceTest : MonoBehaviour
{
    public float force;
    public Vector3 direction;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = new Vector3(-0.30f, 0.21f, -0.93f);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
