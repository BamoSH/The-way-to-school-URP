using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public Image image;
    private Animator animator;

    private void Awake()
    {
        animator = image.GetComponent<Animator>();
    }

    void Start()
    {
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            print("Any key pressed!");
            StartCoroutine(ChangeSceneDelay(0));
        }
    }

    IEnumerator ChangeSceneDelay(int sceneIndex)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
    }
}
