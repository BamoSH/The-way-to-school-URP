using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SkyLevelMovement : MonoBehaviour
{
    [Header("Audio")] 
    public AudioClip jumpClip;
    public AudioClip popDown2;
    public AudioClip popUp1;
    public AudioClip cat;
    public AudioSource audioSource;
    [Header("Movement")]
    public ParagliderMove paragliderMove;
    public float moveSpeedInV = 15f;
    public float moveSpeedInH = 10f;
    public float jumpForce = 10f;

    public float collideWallForce;
    public float collideCatForce;

    [Header("Death")]
    public Animator deathAnimator;
    public Canvas deathCanvas;
    private bool jump = true;
    private float horizontalInput;
    private Vector3 reboundDir;
    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * (moveSpeedInV * Time.deltaTime);
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontalInput) > 0)
        {
            Vector3 force = transform.right * horizontalInput * moveSpeedInH;
            _rb.AddForce(force, ForceMode.Force);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jump)
        {
            audioSource.PlayOneShot(jumpClip);
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jump = false;
            StartCoroutine(ResetJump());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            moveSpeedInH = 0;
        }
        if (other.CompareTag("Failure"))
        {
            print("Failure");
            StartCoroutine(StartTransitionDelay());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Acceleration"))
        {
            print("Acc!");
            moveSpeedInH = 20;
            paragliderMove.speed = 20;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Acceleration"))
        {
            print("12");
            moveSpeedInH = 12;
            paragliderMove.speed = 12;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            audioSource.PlayOneShot(jumpClip);
            print("Collide Wall");
            GetCollideDirection(other);
            _rb.AddForce(reboundDir * collideWallForce, ForceMode.Impulse);
        }

        if (other.gameObject.CompareTag("Cat"))
        {
            audioSource.PlayOneShot(cat);
            GetCollideDirection(other);
            _rb.AddForce(reboundDir * collideCatForce, ForceMode.Impulse);
        }
    }

    private void GetCollideDirection(Collision other)
    {
        Vector3 colliderPos = transform.position;
        Vector3 collideredObjPos = other.transform.position;
        Vector3 directionFromCollider = (colliderPos - collideredObjPos).normalized;
        reboundDir = (directionFromCollider + new Vector3(0, 1, 0)).normalized;
    }

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(1);
        jump = true;
    }
    
    IEnumerator StartTransitionDelay()
    {
        if (MainBGM.instance != null)
        {
            MainBGM.instance.SwitchAudioWithFade();
        }
        deathCanvas.sortingOrder = 2;
        audioSource.PlayOneShot(popDown2);
        deathAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        StartCoroutine(EndTransitionDelay());

    }

    IEnumerator EndTransitionDelay()
    {
        audioSource.PlayOneShot(popUp1);
        deathAnimator.SetTrigger("End");
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        deathCanvas.sortingOrder = -1;
    }
}

