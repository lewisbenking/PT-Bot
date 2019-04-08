using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScreen : MonoBehaviour
{
    private GameObject pt;
    private static Animator animator;
    private JimBot jimBot;
    private AudioClip audioClip;
    private AudioSource audioSource;

    void Start()
    {
        pt = GameObject.Find("JimBot");
        audioSource = GameObject.Find("JimBot").GetComponentInChildren<AudioSource>();
        PlayAudio("StartingScreen");
    }
    
    public void PlayAudio(string fileName)
    {
        audioSource.Stop();
        audioClip = WavUtility.ToAudioClip($"{Application.dataPath}/Audio/{fileName}.wav");
        audioSource.spatialBlend = 0.0f;
        audioSource.PlayOneShot(audioClip);
        Debug.Log("Played Audio");
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
