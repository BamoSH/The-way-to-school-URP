using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class OldManSpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform endPoint;
    public float spawnInterval = 5f;
    private int spawnCount = 3; 
    public float speed = 2f;
    public float rotationY = 60;
    public float rotationX = -90;
    public GameObject oldManPrefab;
    private List<GameObject> activeOldMen = new List<GameObject>();
    
    private void Start()
    {
        StartCoroutine(SpawnOldMen());
    }

    private void Update()
    {
        for (int i = activeOldMen.Count - 1; i >= 0; i--)
        {
            GameObject oldMan = activeOldMen[i];
            if (oldMan != null)
            {
                MoveOldMan(oldMan);
            }
        }
    }

    private IEnumerator SpawnOldMen()
    {
        while (true)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 offset = new Vector3(i * 1.5f, 0, i*1.5f); 
                Vector3 spawnPosition = spawnPoint.position + offset;
                GameObject oldMan = OldManPool.Instance.GetOldMan(); 
                oldMan.transform.position = spawnPosition;
                activeOldMen.Add(oldMan); 
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void MoveOldMan(GameObject oldMan)
    {
        Vector3 initialRotation = new Vector3(rotationX, rotationY, 0);
        Vector3 direction = (endPoint.position - oldMan.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        oldMan.transform.rotation = Quaternion.Euler(initialRotation);
        // oldMan.transform.rotation = Quaternion.Slerp(oldMan.transform.rotation, targetRotation, speed * Time.deltaTime);
        
        oldMan.transform.position += direction * (speed * Time.deltaTime);
        if (Vector3.Distance(oldMan.transform.position, endPoint.position) < 5f)
        {
            activeOldMen.Remove(oldMan);
            OldManPool.Instance.ReturnOldMan(oldMan);
        }
    }
}
