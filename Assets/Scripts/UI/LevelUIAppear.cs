using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelUIAppear : MonoBehaviour
{
    public GameObject levelBoard;
    public Camera mainCamera;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUIRotation();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DisplayBoard();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseBoard();
        }
    }

    public void DisplayBoard()
    {
        levelBoard.transform.DOScale(1,0.5f).SetEase(Ease.OutBounce);
    }

    public void CloseBoard()
    {
        levelBoard.transform.DOScale(0,0.5f).SetEase(Ease.InBounce);
    }
    // Start is called before the first frame update

    private void UpdateUIRotation()
    {
        levelBoard.transform.rotation = mainCamera.transform.rotation;
    }
}
