using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class ParagliderMove : MonoBehaviour
{
    public float speed = 12;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            speed = 0;
        }
    }
}
