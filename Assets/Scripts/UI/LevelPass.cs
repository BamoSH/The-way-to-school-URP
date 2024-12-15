using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPass : MonoBehaviour
{
    public GameObject level;
    public AudioClip successClip;
    public AudioSource audioSource;

    private UIManager _uiManager;
    private PlayerMoveCityLevel _playerMoveCityLevel;
    private PlayerChangeRail _playerChangeRail;
    // Start is called before the first frame update
    void Start()
    {
        level.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        if (level != null && level.transform.parent != null)
        {
            _uiManager = level.GetComponentInParent<UIManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")||other.CompareTag("Train"))
        {
            _playerMoveCityLevel = other.GetComponent<PlayerMoveCityLevel>();
            _playerChangeRail = other.GetComponent<PlayerChangeRail>();
            if (_playerChangeRail != null)
            {
                _playerChangeRail.trainSoundSource.Stop();
            }
            if (_playerMoveCityLevel != null)
            {
                _playerMoveCityLevel.walkSpeed = 0;
                _playerMoveCityLevel.moveSpeed = 0;
            }
            StartCoroutine(StartTransition());
        }
    }

    IEnumerator StartTransition()
    {
        if (MainBGM.instance != null)
        {
            MainBGM.instance.SwitchAudioWithFade();
        }
        _uiManager.PlayStartTransition();
        yield return new WaitForSeconds(1f);
        level.SetActive(true);
        audioSource.PlayOneShot(successClip);
    }
}
