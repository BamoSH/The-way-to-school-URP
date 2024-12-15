using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EagleTriggerArea : MonoBehaviour
{
    public GameObject eaglePrefab; 
    public Transform eagleSpawnPoint;
    private bool isAreaTriggered = false;
    private GameObject eagle;
    
    private PlayerMoveCityLevel playerMoveCityLevel;
    private EagleController eagleController;

    void Start()
    {
        playerMoveCityLevel = FindObjectOfType<PlayerMoveCityLevel>();
    }

    void Update()
    {
        // 这里可以加入更多逻辑，例如在某些情况下检测角色是否飞到目标区域后
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAreaTriggered && other.CompareTag("Player")) // 当角色触发某个区域
        {
            isAreaTriggered = true;

            eagle = Instantiate(eaglePrefab, eagleSpawnPoint.position, Quaternion.identity);
            eagleController = eagle.GetComponent<EagleController>();
            eagleController.player = other.GameObject();
            
            // eagleController.StartFlying();

        }
    }
}
