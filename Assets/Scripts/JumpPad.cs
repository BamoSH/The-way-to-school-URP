using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 20f;

    private bool isReady = true;
    
    private PlayerMoveCityLevel _playerMoveCityLevel;
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
        // Debug.Log("Bounce pad is triggered!");
        if (other.CompareTag("Player"))
        {
            _playerMoveCityLevel = other.GetComponent<PlayerMoveCityLevel>();
            PhysicsCheck physicsCheck = other.GetComponent<PhysicsCheck>();
            if (physicsCheck != null && other.attachedRigidbody != null && isReady && physicsCheck.isGround)
            {
                Debug.Log("Collider is Player!");
                other.attachedRigidbody.AddForce(0,jumpForce,0, ForceMode.Impulse);
                isReady = false;
            }
            else
            {
                var rigidBody = other.attachedRigidbody;
                if (_playerMoveCityLevel != null)
                {
                    _playerMoveCityLevel.audioSource.PlayOneShot(_playerMoveCityLevel.jumpClip);
                }
                other.attachedRigidbody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
                other.attachedRigidbody.AddForce(0,jumpForce,0, ForceMode.Impulse);
            }
        }
    }

    IEnumerator PrepareBouncePad()
    {
        yield return new WaitForSeconds(1);
        isReady = true;
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Bounce pad is ready again!");
        StartCoroutine(PrepareBouncePad());
    }
}
