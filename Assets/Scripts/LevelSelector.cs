using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public Canvas canvas;
    public Animator animator;
    public int level;
    private UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = canvas.GetComponent<UIManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Select", true);
            // ResetAllTrigger();
            // animator.SetTrigger("Select");
            if (Input.GetKeyDown(KeyCode.F))
            {
                switch (level)
                {
                    case 2:
                        uiManager.JpToCity();
                        break;
                    case 3:
                        uiManager.JpToRailway();
                        break;
                    case 4:
                        uiManager.JpToSkyLevel();
                        break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Select", false);

            // ResetAllTrigger();
            // animator.SetTrigger("Exit");
        }
    }

    private void ResetAllTrigger()
    {
        animator.ResetTrigger("Select");
        animator.ResetTrigger("Exit");
    }
}
