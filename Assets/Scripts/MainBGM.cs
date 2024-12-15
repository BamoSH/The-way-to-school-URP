using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainBGM : MonoBehaviour
{
    public AudioClip musicClip1;
    public AudioClip musicClip2;
    public AudioClip musicClip3;
    public static MainBGM instance;
    private AudioSource bgmSource;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        bgmSource = GetComponent<AudioSource>();
        if (bgmSource != null)
        {
            bgmSource.loop = true; 
            bgmSource.Play();  
            DontDestroyOnLoad(bgmSource.gameObject);  
        }
    }

    public void PlayMusic1()
    {
        bgmSource.volume = 0.3f;
        bgmSource.clip = musicClip1;
        bgmSource.Play();
    }

    public void PlayMusic2()
    {        
        bgmSource.volume = 0.3f;
        bgmSource.clip = musicClip2;
        bgmSource.Play();
    }

    public void PlayMusic3()
    {
        bgmSource.volume = 0.3f;
        bgmSource.clip = musicClip3;
        bgmSource.Play();
    }

    public void Pause()
    {
        bgmSource.Pause();
    }
    
    public void SwitchAudioWithFade()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float fadeTime = 1f;  

        float startVolume = bgmSource.volume;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }
        bgmSource.volume = 0;
        bgmSource.Stop();
    }
}
