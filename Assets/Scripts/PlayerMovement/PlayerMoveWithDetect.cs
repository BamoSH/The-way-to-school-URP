using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveWithDetect : MonoBehaviour
{
    public Animator animator;
    public float normalSpeed = 6f;
    public float currentSpeed;
    public float turnSpeed =30;

    public float acceleratePadSpeed = 14f;
    public float accelerationRate = 0.03f; // 每秒加速率
    public float decelerationRate = 1f;

    public bool isAccelerate = false;
    public float force = 10;
    
    public float maxSpeed = 10;
    public float acceleration = 0.1f;  // 施加的力的强度
    public float deceleration = 0.95f;

    public Transform groundCheck;
    public float rayLength = 0.5f;
    public float maxAngleOnSlope = 45f;
    public Vector3 hitNormal;

    private Vector3 moveAmount;
    private PhysicsCheck _physicsCheck;

    //  使用刚体进行移动
    private Rigidbody rb;
    //  获取移动输入
    private Vector2 movementInput;
    //  是否在移动
    private bool isRunning;
    //  移动方向
    private Vector3 movement;
    //  旋转
    private Quaternion targetRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = normalSpeed;
        _physicsCheck = GetComponent<PhysicsCheck>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    private void Update()
    {
        // Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        // Debug.Log(rb.velocity.magnitude);

    }

    void FixedUpdate()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movement.Set(movementInput.x, 0, movementInput.y);
        //  检查输入
        bool horizontalInput = !Mathf.Approximately(movementInput.x, 0);
        bool verticalInput = !Mathf.Approximately(movementInput.y, 0);
        
        isRunning = horizontalInput || verticalInput;

        if (Input.GetKey(KeyCode.W))
        {
        
            if (currentSpeed<=maxSpeed)
            {          
                currentSpeed += accelerationRate;
            }
            
            // if (isAccelerate && currentSpeed<=10)
            // {
            //     Debug.Log("111111111");
            //     isAccelerate = false;
            // }
            // else
            // {
            //     Debug.Log("2222222222222222222");
            // }
        } else
        {
            currentSpeed = normalSpeed;
        }
        
        if (isRunning)
        {
            animator.SetBool("Drive", true);
            movement = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * movement;
        }
        else
        {
            animator.SetBool("Drive", false);
        }
        //  Vector3.RotateTowards 将当前方向朝目标方向旋转
        Vector3 lookForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.fixedDeltaTime, 0);
        targetRotation = Quaternion.LookRotation(lookForward);
        if (OnSlope())
        {
            movement = Vector3.ProjectOnPlane(movement, hitNormal).normalized;
        }
        rb.MovePosition(rb.position + (movement * currentSpeed * Time.fixedDeltaTime));
        // rb.MovePosition(rb.position + moveAmount);
        rb.MoveRotation(targetRotation);

        // if (Input.GetKey(KeyCode.Space) && _physicsCheck.isGround)
        // {
        //     Debug.Log("Space is pressed!");
        //     rb.velocity = new Vector3(0,force,0);
        // }
    }

    public IEnumerator BoostSpeed(float tarSpeed, float accDuration, float DecDuration)
    {
        Debug.Log("BoostSpeed is running");

        float startTime = Time.time;
        while (Time.time < startTime + accDuration)
        {
            Debug.Log("Acceleration!");
            currentSpeed = Mathf.Lerp(currentSpeed, tarSpeed, (Time.time - startTime) / accDuration);
            yield return null;
        }
        startTime = Time.time;
        while (Time.time < startTime + DecDuration)
        {
            Debug.Log("Deceleration!");
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, (Time.time - startTime) / DecDuration);
            yield return null;
        }
    }

    private void AdjustSpeed()
    {
        
    }

    public void AcceleratorPadSpeed(bool accelerate)
    {
        if (accelerate)
        {
            currentSpeed = acceleratePadSpeed;
            isAccelerate = true;
        }
        else
        {
            // currentSpeed -= decelerationRate * Time.fixedDeltaTime;
            StartCoroutine(DecelerateSpeed());
        }
    }
    private IEnumerator DecelerateSpeed()
    {
        while (currentSpeed > maxSpeed)
        {
            currentSpeed -= decelerationRate * Time.fixedDeltaTime;
            // isAccelerate = false;
            yield return null;
        }
    }

    private bool OnSlope()
    {
        Ray ray = new Ray(groundCheck.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            hitNormal = hit.normal;
            // Debug.Log("ground!");
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            // Debug.Log(slopeAngle);
        }

        return true;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;  // 设置Gizmos颜色为红色
        // 绘制一个球体表示地面检测区域
        Gizmos.DrawWireSphere(groundCheck.position, rayLength);
    }
}
