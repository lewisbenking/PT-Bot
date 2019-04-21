using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip audioClip;
    private GameObject mainPanel, panelWorkoutEquipment, panelAreasToTrain, panelExercises, panelIndividualExercise, panelStartWorkout, panelSkipExercise, panelPauseWorkout, panelEndWorkout, panelHUD;

    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        mainPanel = GameObject.Find("MainPanel");
        panelAreasToTrain = GameObject.Find("Help_AreasToTrainPanel"); PanelSetActive(panelAreasToTrain, false);
        panelWorkoutEquipment = GameObject.Find("Help_WorkoutEquipmentPanel"); PanelSetActive(panelWorkoutEquipment, false);
        panelExercises = GameObject.Find("Help_ExercisesPanel"); PanelSetActive(panelExercises, false);
        panelEndWorkout = GameObject.Find("Help_WorkoutEndPanel"); PanelSetActive(panelEndWorkout, false);
        panelPauseWorkout = GameObject.Find("Help_PauseWorkoutPanel"); PanelSetActive(panelPauseWorkout, false);
        panelSkipExercise = GameObject.Find("Help_SkipExercisePanel"); PanelSetActive(panelSkipExercise, false);
        panelStartWorkout = GameObject.Find("Help_StartWorkoutPanel"); PanelSetActive(panelStartWorkout, false);
        panelIndividualExercise = GameObject.Find("Help_IndividualExercisePanel"); PanelSetActive(panelIndividualExercise, false);
        panelHUD = GameObject.Find("Help_HUD"); PanelSetActive(panelHUD, false);
    }

    public void ShowHelp(GameObject newActivePanel)
    {
        PanelSetActive(mainPanel, false);
        PanelSetActive(newActivePanel, true);
        PlayAudio(newActivePanel.name);
    }

    public void PanelSetActive(GameObject panel, bool isActive)
    {
        if (panel != null) panel.SetActive(isActive);
    }

    public void PlayAudio(string fileName)
    {
        audioSource.Stop();
        audioClip = WavUtility.ToAudioClip($"{Application.dataPath}/Audio/{fileName}.wav");
        audioSource.spatialBlend = 0.0f;
        audioSource.PlayOneShot(audioClip);
    }

    public void LoadScene(int sceneNumber) { SceneManager.LoadScene(sceneNumber); }
}