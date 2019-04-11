using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip audioClip;
    private GameObject mainPanel, panelChooseEquipment, panelChooseBodyArea, panelViewExerciseInfo, panelStartWorkout, panelSkipExercise, panelPauseWorkout, panelEndWorkout;

    void Start()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        mainPanel = GameObject.Find("MainPanel");
        panelChooseBodyArea = GameObject.Find("PanelChooseBodyArea"); PanelSetActive(panelChooseBodyArea, false);
        panelChooseEquipment = GameObject.Find("PanelChooseEquipment"); PanelSetActive(panelChooseEquipment, false);
        panelEndWorkout = GameObject.Find("PanelEndWorkout"); PanelSetActive(panelEndWorkout, false);
        panelPauseWorkout = GameObject.Find("PanelPauseWorkout"); PanelSetActive(panelPauseWorkout, false);
        panelSkipExercise = GameObject.Find("PanelSkipExercise"); PanelSetActive(panelSkipExercise, false);
        panelStartWorkout = GameObject.Find("PanelStartWorkout"); PanelSetActive(panelStartWorkout, false);
        panelViewExerciseInfo = GameObject.Find("PanelViewExerciseInfo"); PanelSetActive(panelViewExerciseInfo, false);
    }

    public void ShowHelp(GameObject newActivePanel)
    {
        PanelSetActive(mainPanel, false);
        PanelSetActive(newActivePanel, true);
    }

    public void PanelSetActive(GameObject panel, bool isActive)
    {
        if (panel != null) { panel.SetActive(isActive); }
    }

    public void PlayAudio(string fileName)
    {
        audioSource.Stop();
        audioClip = WavUtility.ToAudioClip($"{Application.dataPath}/Audio/{fileName}.wav");
        audioSource.spatialBlend = 0.0f;
        audioSource.PlayOneShot(audioClip);
    }

    public void LoadScene(int sceneNumber)  {  SceneManager.LoadScene(sceneNumber); }
}