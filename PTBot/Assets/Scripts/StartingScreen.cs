using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScreen: MonoBehaviour
{
    private AudioClip audioClip;
    private AudioSource audioSource;
    public bool audioPlayed;

    void Start()
    {
        audioSource = GameObject.Find("JimBot").GetComponentInChildren<AudioSource>();
        PlayAudio();
    }
    
    public void PlayAudio()
    {
        audioPlayed = false;
        audioSource.Stop();
        audioClip = WavUtility.ToAudioClip($"{Application.dataPath}/Audio/StartingScreen.wav");
        audioSource.spatialBlend = 0.0f;
        audioSource.PlayOneShot(audioClip);
        audioPlayed = true;
    }

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void ExitApp()
    {
        Debug.Log("Exit Pressed");
        Application.Quit();
    }
}
