using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScreen: MonoBehaviour
{
    private AudioClip audioClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GameObject.Find("JimBot").GetComponentInChildren<AudioSource>();
        PlayAudio();
    }
    
    // Plays audio clip
    public void PlayAudio()
    {
        audioSource.Stop();
        audioClip = WavUtility.ToAudioClip($"{Application.dataPath}/Audio/StartingScreen.wav");
        audioSource.spatialBlend = 0.0f;
        audioSource.PlayOneShot(audioClip);
    }

    // Loads a scene
    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    // Closes the application
    public void ExitApp()
    {
        Debug.Log("Exit Pressed");
        Application.Quit();
    }
}