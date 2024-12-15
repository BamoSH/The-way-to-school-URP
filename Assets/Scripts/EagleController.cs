using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EagleController : MonoBehaviour
{
    public enum EagleState
    {
        Following,
        Eating,
        Flying,       
        Destroying    
    }
    
    
    // todo: Follow State, Fly State
    public EagleState currentState = EagleState.Following;
    public float flightSpeed = 15f;
    public float flightDirectionSpeed = 20f; 
    public float followDistance = 1.5f;
    public GameObject player;
    public float smoothTime = 0.3f; 

    public float flyDistance = 30;
    public Animator animator;
    
    private bool isFollowing;
    private bool isFlying;
    private Vector3 velocity = Vector3.zero; 

    private PlayerMoveCityLevel playerMoveCityLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMoveCityLevel = player.GetComponent<PlayerMoveCityLevel>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EagleState.Following:
                FollowingState();
                break;
            case EagleState.Flying:
                FlyingState();
                break;
            case EagleState.Destroying:
                DestroyingState();
                break;
        }
        // Vector3 direction = (transform.position - player.transform.position).normalized;
        // Vector3 targetPosition = player.transform.position + direction * followDistance;
        // transform.position = Vector3.SmoothDamp(transform.position,targetPosition,ref velocity, smoothTime);
    }
    
    private void Fly()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 

        Vector3 flyDirection = new Vector3(horizontal, vertical, 1).normalized;
        transform.Translate(flyDirection * flightSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.transform.position) > flyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void FollowingState()
    {
        Vector3 vector = (player.transform.position - transform.position);
        Vector3 direction = vector.normalized;
        float distance = vector.magnitude;
        Vector3 targetPosition = player.transform.position;
        if (distance <= followDistance)
        {
            StartCoroutine(EatingState());
        }
        else
        {
            transform.position += direction * (flightSpeed * Time.deltaTime);
        }
    }

    IEnumerator EatingState()
    {
        // todo: animator
        yield return new WaitForSeconds(1f);
        playerMoveCityLevel.DisableControl();
        player.SetActive(false);
        currentState = EagleState.Flying;
        player.transform.SetParent(transform);
        player.transform.localPosition = new Vector3(0, 0, followDistance); 
    }

    private void FlyingState()
    {
        transform.position += transform.forward * (flightSpeed * Time.deltaTime);
        float horizontal = Input.GetAxis("Horizontal"); 
        Vector3 moveDirection = transform.right * (horizontal * flightDirectionSpeed * Time.deltaTime);
        transform.position += moveDirection;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReleasePlayer();
        }
    }

    private void DestroyingState()
    {
        transform.position += transform.forward * ((flightSpeed + 10) * Time.deltaTime);
        if (Vector3.Distance(transform.position, player.transform.position) > flyDistance)
        {
            Destroy(gameObject);
        }
    }
    
    private void ReleasePlayer()
    {
        player.transform.SetParent(null);
        player.gameObject.SetActive(true); 
        playerMoveCityLevel.EnableControl(); 
        player.transform.position = transform.position; 

        currentState = EagleState.Destroying;
    }
}
