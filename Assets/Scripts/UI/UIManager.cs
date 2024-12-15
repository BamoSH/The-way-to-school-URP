using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public int mainMenuScene = 0;
    public Animator animator;
    public Canvas canvas;

    [Header("Audio")] 
    public AudioClip buttonClick;
    public AudioClip popUp1;
    public AudioClip popUp2;
    public AudioSource audioSource;
    
    private void Awake()
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(EndTransitionDelay());
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (MainBGM.instance != null)
            {
                print("music1");
                MainBGM.instance.PlayMusic1();
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (MainBGM.instance != null)
            {
                print("music1");
                MainBGM.instance.PlayMusic1();
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (MainBGM.instance != null)
            {
                print("music2");

                MainBGM.instance.PlayMusic2();
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            if (MainBGM.instance != null)
            {
                MainBGM.instance.PlayMusic3();
            }
        }

    }

    public void JpToMainMenu()
    {
        // SceneManager.LoadScene(0);  // jump to main menu
        audioSource.PlayOneShot(buttonClick);
        StartCoroutine(StartTransitionDelay(mainMenuScene));
        print("Go to Main Menu");
    }
    
    public void JpToLevelChoose()
    {
        audioSource.PlayOneShot(buttonClick);
        // SceneManager.LoadScene(1);  // jump to level choose
        StartCoroutine(StartTransitionDelay(1));
    }
    
    public void JpToCity()
    {
        audioSource.PlayOneShot(buttonClick);
        // SceneManager.LoadScene(3);  // jump to railway level
        StartCoroutine(StartTransitionDelay(2));
    }
    
    public void JpToRailway()
    {
        audioSource.PlayOneShot(buttonClick);
        // SceneManager.LoadScene(3);  // jump to railway level
        StartCoroutine(StartTransitionDelay(3));
        print("Go to Railway Level");
    }

    public void JpToSkyLevel()
    {
        audioSource.PlayOneShot(buttonClick);
        // SceneManager.LoadScene(2);  // jump to sky level
        StartCoroutine(StartTransitionDelay(4));
    }

    public void JpToLevelPass()
    {
        audioSource.PlayOneShot(buttonClick);
        StartCoroutine(StartTransitionDelay(5));
    }
    
    public void OnNextLevelButtonClicked()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex; 
        int nextLevelIndex = currentLevelIndex + 1;
        
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(StartTransitionDelay(nextLevelIndex));
        }
        else
        {
            Debug.Log("已经是最后一关了，回到主界面！");
            StartCoroutine(StartTransitionDelay(mainMenuScene));
        }
    }

    public void PlayStartTransition()
    {
        audioSource.PlayOneShot(popUp1);
        animator.SetTrigger("Start");
    }

    public void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    IEnumerator StartTransitionDelay(int sceneIndex)
    {
        canvas.sortingOrder = 1;
        animator.SetTrigger("Start");
        audioSource.PlayOneShot(popUp1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator EndTransitionDelay()
    {
        animator.SetTrigger("End");
        audioSource.PlayOneShot(popUp2);
        yield return new WaitForSeconds(1);
        canvas.sortingOrder = -1;
    }
    
}
