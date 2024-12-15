using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorPad : MonoBehaviour
{
    public float accelerationTime = 3f;
    public float accSpeed = 16f;
    [Header("Not Used")]
    public float boostedSpeed = 50f;
    public float decelerationTime = 5f;
    public bool accelerate = false;

    private PlayerMoveWithDetect _playerMoveWithDetect;

    private PlayerMoveCityLevel _playerMoveCityLevel;
    // public bool decelerate = false;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Accelerate pad is triggered!");
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Collider is Player!");
            PlayerMoveWithDetectEnter(other);
            _playerMoveCityLevel = other.GetComponent<PlayerMoveCityLevel>();
            if (_playerMoveCityLevel != null)
            {
                _playerMoveCityLevel.audioSource.PlayOneShot(_playerMoveCityLevel.accClip);
                _playerMoveCityLevel.speedPadDuration = accelerationTime;
                _playerMoveCityLevel.accPadSpeed = accSpeed;
                _playerMoveCityLevel.isSpeedPad = true;
                _playerMoveCityLevel.speedPadTimer = _playerMoveCityLevel.speedPadDuration;
                _playerMoveCityLevel.state = PlayerMoveCityLevel.MovementState.speedPad;
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Collider is Player exit!");
            PlayerMoveWithDetectExit();
            if (_playerMoveCityLevel != null)
            {
                _playerMoveCityLevel.isSpeedPad = false;
            }
        }
    }
    
    private void PlayerMoveWithDetectEnter(Collider other)
    {
        _playerMoveWithDetect = other.GetComponent<PlayerMoveWithDetect>();
        if (_playerMoveWithDetect != null)
        {
            Debug.Log("PlayerMoveV object running!");
            // StartCoroutine(playerMove.BoostSpeed(boostedSpeed, accelerationTime, decelerationTime));
            _playerMoveWithDetect.AcceleratorPadSpeed(accelerate = true);
        }
    }
    private void PlayerMoveWithDetectExit()
    {
        if (_playerMoveWithDetect!=null)
        {
            Debug.Log("Player is exit accelerator pad!");
            _playerMoveWithDetect.AcceleratorPadSpeed(accelerate = false);
        }
    }
}
