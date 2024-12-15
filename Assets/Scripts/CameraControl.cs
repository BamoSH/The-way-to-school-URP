using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject cameraLook;
    public float rotateSpeed = 80;
    
    public int yMinLimit = 3;
    public int yMaxLimit = 80;
    public float lerp = 5;
    
    private float distance;
    private Quaternion rotation;
    private Vector3 resultPos;
    private Vector2 mousePos;
    private bool isClickRotate;
    
    private float xAngle;
    private float yAngle;
     
    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(cameraLook.transform.position, transform.position);
        rotation = transform.rotation;
        xAngle = Vector3.Angle(Vector3.right,transform.right);
        yAngle = Vector3.Angle(Vector3.up,transform.up);
     }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle<-360)
            angle += 360;
        if (angle>360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isClickRotate = Input.GetMouseButton(0);
        isClickRotate = true;
        mousePos.x = Input.GetAxis("Mouse X");
        mousePos.y = -Input.GetAxis("Mouse Y");

        Cursor.lockState = CursorLockMode.Locked;
        
        if (isClickRotate)
        {
            xAngle += mousePos.x * rotateSpeed * Time.fixedDeltaTime;
            yAngle += mousePos.y * rotateSpeed * Time.fixedDeltaTime;
            yAngle = ClampAngle(yAngle,yMinLimit,yMaxLimit);
            rotation = Quaternion.Euler(yAngle,xAngle,0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.fixedDeltaTime * 5);
        }
        resultPos = cameraLook.transform.position - (rotation * Vector3.forward * distance);
        transform.position = Vector3.Lerp(transform.position, resultPos, Time.fixedDeltaTime * lerp);
    }
}
