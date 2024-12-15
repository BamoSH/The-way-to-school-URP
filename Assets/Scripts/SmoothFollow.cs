using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target; 
    public float positionSmoothTime = 0.3f; 
    public float rotationSmoothTime = 0.3f; 

    private Vector3 velocity; 

    void LateUpdate()
    {
        Quaternion targetRotation = transform.parent.rotation;
        if (target != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, positionSmoothTime);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothTime * Time.deltaTime);
        }
    }
}
