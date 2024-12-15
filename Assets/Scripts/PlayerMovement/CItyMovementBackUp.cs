using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CItyMovementBackUp : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Animator")] public Animator animator;
    [Header("Movement")]
    public float moveSpeed = 7f;

    public float maxSpeed = 20f;
    public float walkSpeed = 8f;
    public float slopeSpeed = 8f;
    public float sprintSpeed = 12f;
    public float rotationSpeed = 175f;
    public float groundDrag = 5f;
    public float jumpForce = 12f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    public float gravityMultiplier = 10f;
    public bool readyToJump = true;
    
    [Header("Keybindings")]
    public KeyCode jumpKey = KeyCode.LeftShift;
    public KeyCode sprintKey = KeyCode.Space;
    [Header("Ground Check")] 
    public float playerHeight = 2f;

    public float detectionRadius = 0.5f;
    public LayerMask groundLayer;
    public bool isGround;

    [Header("Slope Check")] 
    public float maxSlopeAngle = 60f;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    
    // public Transform orientation;

    [Header("Sprint Check")] 
    public float airSprintForce = 25f;
    public float sprintAngle = 30f;
    public float sprintCooldown = 1f;
    [SerializeField]private bool readyToSprint = true;
    
    
    [Header("Speed Pad")]
    public float accPadSpeed = 16f;
    public float speedPadDuration = 2f;
    public float speedPadTimer = 0f;
    public bool isSpeedPad;
    
    private float horizontalInput;
    private float verticalInput;
    
    private Vector3 moveDirection;
    [SerializeField]private Vector3 reboundDir;
    [SerializeField]private bool isFaint = false;
    [SerializeField]private float faintTimer = 3f;
    public float faintForce = 10f;

    private bool isControlEnabled = true;
    private Rigidbody rb;
    public MovementState state;
    public enum MovementState
    {
        walking,
        faint,
        sprintingOnGround,
        sprintingOnAir,
        speedPad,
        air
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        state = MovementState.walking;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isControlEnabled)
        {
            GroundCheck();
            PlayerInput();
            SpeedControl();
            // StateHandler();
            ExecuteState();
        }
    }

    private void FixedUpdate()
    {
        if (isControlEnabled)
        {
            MovePlayer();
        }
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        // verticalInput = Input.GetAxis("Vertical");
        // when to jump
        
        if (Input.GetKey(jumpKey) && readyToJump && isGround)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // if (Input.GetKey(jumpKey))
        // {
        //     rb.AddForce(new Vector3(0,1,1) * faintForce,ForceMode.Force);
        // }
        if (Input.GetKey(sprintKey))
        {
            if (state == MovementState.walking && readyToSprint)
            {
                StateSwitch(MovementState.sprintingOnGround,0);
            }
            else if (state == MovementState.air)
            {
                StateSwitch(MovementState.sprintingOnAir,0);
            }
        }
    }

    private void StateHandler()
    {
        if (isGround)
        {
            if (isSpeedPad && speedPadTimer > 0)
            {
                speedPadTimer -= Time.deltaTime;
                moveSpeed = accPadSpeed;
            }
            else if (Input.GetKeyDown(sprintKey) && readyToSprint)
            {
                state = MovementState.sprintingOnGround;
                // moveSpeed = sprintSpeed;
                // rb.AddForce(transform.forward * sprintSpeed, ForceMode.Impulse);
                readyToSprint = false;
                // Invoke(nameof(ResetSprint), sprintCooldown);
            }
            else if (!readyToSprint && state == MovementState.sprintingOnGround)
            {
                state = MovementState.sprintingOnGround;
                moveSpeed = sprintSpeed;
            }
            else
            {
                state = MovementState.walking;
                moveSpeed = walkSpeed;
            }
        }
        else
        {
            if (Input.GetKeyDown(sprintKey) && readyToSprint)
            { 
                state = MovementState.sprintingOnAir;
                readyToSprint = false;
                AirSprint();
                // Invoke(nameof(ResetSprint), sprintCooldown);
            }
            else if (!readyToSprint)
            {
                state = MovementState.sprintingOnAir;
            }
            else
            {
                state = MovementState.air;
            }
        }
        if (speedPadTimer <= 0 && state == MovementState.speedPad)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
    }

    private void StateSwitch(MovementState newState, float delay)
    {
        if (delay > 0f)
        {
            StartCoroutine(DelayedStateSwitch(newState, delay));
        }
        else                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
        {
            state = newState;
        }
    }
    
    IEnumerator DelayedStateSwitch(MovementState newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        state = newState;
    }

    private void ExecuteState()
    {
        switch (state)
        {
            case MovementState.walking:
                if (isGround)
                {
                    moveSpeed = walkSpeed;
                    animator.SetTrigger("Walk");
                    if (readyToSprint == false)
                    {
                        readyToSprint = true;
                    }
                }
                else
                    StateSwitch(MovementState.air, 0);
                break;
            case MovementState.faint:
                moveSpeed = 0;
                animator.SetTrigger("Faint");
                if (!isFaint)
                {
                    print("FAINT");
                    // rb.velocity = new Vector3(0, 0, 0);
                    rb.AddForce(reboundDir * faintForce, ForceMode.Impulse);
                    Debug.Log("Rebound Direction: " + reboundDir);
                    Debug.Log("Force Applied: " + (reboundDir * faintForce).ToString());
                    isFaint = true;
                    StartCoroutine(ResetSprint(faintTimer));
                    if (isGround)
                    {
                        StateSwitch(MovementState.walking, faintTimer);
                    }
                    else
                    {
                        StateSwitch(MovementState.air, faintTimer);
                    }
                }
                break;
            case MovementState.sprintingOnGround:
                animator.SetTrigger("SprintOnGround");
                if (readyToSprint)
                {
                    moveSpeed = sprintSpeed;
                    readyToSprint = false;
                    StartCoroutine(ResetSprint(sprintCooldown));
                }
                StateSwitch(MovementState.walking, 1);
                break;
            case MovementState.sprintingOnAir:
                if (readyToSprint)
                {
                    readyToSprint = false;
                    animator.SetTrigger("SprintOnAir");
                    AirSprint();
                    StartCoroutine(ResetSprint(sprintCooldown));
                }
                StateSwitch(MovementState.air, 0);
                break;
            case MovementState.speedPad:
                if (speedPadTimer > 0)
                {
                    animator.SetTrigger("SprintOnGround");
                    speedPadTimer -= Time.deltaTime;
                    moveSpeed = accPadSpeed;
                }
                else
                {
                    StateSwitch(MovementState.walking, 0);
                }
                break;
            case MovementState.air:
                if (isGround)
                {
                    StateSwitch(MovementState.walking, 0);
                }
                else
                {
                    animator.SetTrigger("Air");
                }
                break;
        }
    }
    private void MovePlayer()
    {
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0);
        
        moveDirection = transform.forward;
        transform.rotation = Quaternion.LookRotation(moveDirection);
        // moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * (moveSpeed * 20f), ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                
            }
        }
        else if (isGround && state != MovementState.faint)
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        }
        else if (!isGround && state != MovementState.faint)
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Force);
        }
        rb.useGravity = !OnSlope();
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private void AirSprint()
    {
        // Vector3 sprintDirection = Quaternion.Euler(sprintAngle, 0, 0) * transform.forward;
        Vector3 upwardComponent = transform.up * Mathf.Sin(sprintAngle * Mathf.Deg2Rad); // Y轴分量
        Vector3 forwardComponent = transform.forward * Mathf.Cos(sprintAngle * Mathf.Deg2Rad); // Z轴分量

        // 合成最终的冲刺方向
        Vector3 sprintDirection = (forwardComponent + upwardComponent).normalized;
        rb.AddForce(sprintDirection * (airSprintForce), ForceMode.Impulse);
    }
    
    IEnumerator ResetSprint(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isGround)
        {
            readyToSprint = true;
            isFaint = false;
        }
    }
    
    private void SpeedControl()
    {
        // limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else if (state != MovementState.faint)
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
            // limit velocity if needed
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    private void GroundCheck()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.down;

        float rayLength = playerHeight * 0.5f + 1f;

        isGround = Physics.CheckSphere(rayOrigin, detectionRadius, groundLayer);

        if (isGround)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        Debug.DrawLine(rayOrigin, rayOrigin + Vector3.down * (detectionRadius), isGround ? Color.green : Color.red);
        Debug.DrawRay(rayOrigin, Vector3.down, Color.yellow); // 辅助调试的射线
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle <= maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection,slopeHit.normal).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            GetFaintDirection(other);
        }
    }

    private void GetFaintDirection(Collision other)
    {
        Vector3 colliderPos = transform.position;
        Vector3 collideredObjPos = other.transform.position;
        Vector3 directionFromCollider = (colliderPos - collideredObjPos).normalized;
        reboundDir = (directionFromCollider + new Vector3(0, 1, 0)).normalized;
        StateSwitch(MovementState.faint,0);
    }

    public void DisableControl()
    {
        isControlEnabled = false;
    }

    public void EnableControl()
    {
        isControlEnabled = true;
    }
}
