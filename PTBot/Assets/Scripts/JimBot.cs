﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class JimBot : MonoBehaviour
{
    private Animator animator;
    private GameObject jimBot, areasToTrainPanel, exercisesPanel, individualExercisePanel, muscleDiagramPanel, scrollArea, workoutEquipmentPanel, startWorkoutPanel, workoutEndPanel, exercise1, exercise2, exercise3;
    private ArrayList exerciseNames;
    private AudioClip audioClip;
    private AudioSource audioSource;
    private ExerciseDetails exerciseDetails;
    private Image muscleDiagram;
    private RawImage tvScreen;
    private readonly VideoSource videoSource;
    private string url, diagramToShow, individualExerciseURL, dialogFlowResponse;
    private string AccessToken { get; set; }
    private string[] chatbotResponseSplit;
    private TextMeshProUGUI chatbotResponse, individualExerciseName, currentExercise, individualExerciseDescription, timerLabel;
    private Toggle barbellToggle, cableMachineToggle, dumbbellToggle;
    private VideoPlayer videoPlayer;
    private float timerTime;
    private int exerciseNameArrayIndex;
    private bool isWorkoutPaused, hasWorkoutBeenPaused;

    private void Awake()
    {
        scrollArea = GameObject.Find("ScrollArea");
        exercisesPanel = GameObject.Find("ExercisesPanel");
        individualExercisePanel = GameObject.Find("IndividualExercisePanel");
        workoutEquipmentPanel = GameObject.Find("WorkoutEquipmentPanel");
        muscleDiagramPanel = GameObject.Find("MuscleDiagramPanel");
        areasToTrainPanel = GameObject.Find("AreasToTrainPanel");
        startWorkoutPanel = GameObject.Find("StartWorkoutPanel");
        workoutEndPanel = GameObject.Find("WorkoutEndPanel");

        jimBot = GameObject.Find("JimBot");
        tvScreen = GameObject.Find("TV Screen").GetComponent<RawImage>();
        audioSource = GameObject.Find("JimBot").GetComponentInChildren<AudioSource>();
        timerLabel = GameObject.Find("WorkoutTimer").GetComponent<TextMeshProUGUI>();
        currentExercise = GameObject.Find("CurrentExercise").GetComponentInChildren<TextMeshProUGUI>();
        timerTime = 60f;
        exerciseNameArrayIndex = 0;
    }

    private void Start()
    {
        exerciseDetails = new ExerciseDetails();
        animator = jimBot.GetComponent<Animator>();
    }

    private IEnumerator TimerCountdown()
    {
        while (timerTime > 0f && !isWorkoutPaused)
        {
            if (timerTime == 30f)
            {
                animator.SetTrigger("WellDone");
                PlayAudio("HalfwayTime");
            }

            if (exerciseNameArrayIndex <= (exerciseNames.Count - 1))
            {
                currentExercise.text = exerciseNames[exerciseNameArrayIndex].ToString();
                timerLabel.text = $"Time Left:  {Mathf.Floor(timerTime / 60).ToString("00")}:{(timerTime % 60).ToString("00")}";
            }
            else
            {
                EndWorkout();
            }
            yield return new WaitForSeconds(1);
            timerTime--;
            Debug.Log(timerTime);
        }
        Debug.Log("timerTime is 0");
        Debug.Log($"ArrayIndex is {exerciseNameArrayIndex}. ArrayList Count is {exerciseNames.Count - 1}");
        NextExercise();
        yield return null;
    }

    public void NextExercise()
    {
        timerTime = 60f;
        exerciseNameArrayIndex++;
        if (exerciseNameArrayIndex > (exerciseNames.Count - 1))
        {
            StopCoroutine("TimerCountdown");
            EndWorkout();
        }
        else
        {
            if (exerciseNameArrayIndex == (exerciseNames.Count - 1))
            {
                PlayAudio("TheLastExercise");
            }
            else
            {
                PlayAudio("NextExercise");
            }
            StopCoroutine("TimerCountdown");
            StartCoroutine("TimerCountdown");
        }
    }

    public void StartWorkoutTimer()
    {
        isWorkoutPaused = false;
        if (hasWorkoutBeenPaused)
        {
            PlayAudio("ResumeWorkout");
        }
        else
        {
            PlayAudio("StartWorkout");
        }
        StartCoroutine("TimerCountdown");
    }

    public void GoBackToExerciseList()
    {
        StopCoroutine("TimerCountdown");
        isWorkoutPaused = true;
        hasWorkoutBeenPaused = true;
        PanelSetActive(exercisesPanel, true);
        PanelSetActive(startWorkoutPanel, false);
    }

    public void EndWorkout()
    {
        StopAllCoroutines();
        PanelSetActive(workoutEndPanel, true);
        PanelSetActive(startWorkoutPanel, false);
        animator.SetTrigger("WellDone");
        PlayAudio("CompletedWorkout");
    }

    public void LoadScene(int sceneNumber) { SceneManager.LoadScene(sceneNumber); }

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
        Debug.Log("Played Audio");
    }

    public string GetEquipmentText()
    {
        barbellToggle = GameObject.Find("ToggleBarbell").GetComponent<Toggle>();
        cableMachineToggle = GameObject.Find("ToggleCableMachine").GetComponent<Toggle>();
        dumbbellToggle = GameObject.Find("ToggleDumbbells").GetComponent<Toggle>();
        if (barbellToggle.isOn)
        {
            if (cableMachineToggle.isOn && dumbbellToggle.isOn) { return "All 3"; }
            else if (cableMachineToggle.isOn && !dumbbellToggle.isOn) { return "Barbell and Cable Machine"; }
            else if (!cableMachineToggle.isOn && dumbbellToggle.isOn) { return "Barbell and Dumbbells"; }
            else { return "Barbell Only"; }
        }
        else
        {
            if (cableMachineToggle.isOn && dumbbellToggle.isOn) { return "Cable Machine and Dumbbells"; }
            else if (cableMachineToggle.isOn && !dumbbellToggle.isOn) { return "Cable Machine Only"; }
            else if (!cableMachineToggle.isOn && dumbbellToggle.isOn) { return "Dumbbells Only"; }
            else { return "None selected"; }
        }
    }

    public void GetExerciseDetails(string chatbotResponse)
    {
        exerciseNames = new ArrayList();
        PanelSetActive(scrollArea, false);
        PanelSetActive(exercisesPanel, true);
        chatbotResponseSplit = chatbotResponse.Split('-');
        int index; string word;
        foreach (string iteration in chatbotResponseSplit)
        {
            word = iteration;
            word = word.Replace("workout for you!\n\n", ""); word = word.Replace("- ", ""); word = word.Replace(".\n", ""); word = word.Replace("\nYou can see more about each exercise by clicking the buttons\nWhen you're ready to work out, please select \"Start Workout\".", "");
            index = exerciseDetails.GetArrayIndex(word);
            Debug.Log(word);
            if (index != -1) { exerciseNames.Add(word); }
        }
        // Set individual exercise panel to active
        exercise1 = GameObject.Find("ButtonExercise1");
        exercise2 = GameObject.Find("ButtonExercise2");
        exercise3 = GameObject.Find("ButtonExercise3");
        exercise1.SetActive(false); exercise2.SetActive(false); exercise3.SetActive(false);

        // Assign the exercise names to buttons
        if (exerciseNames.Count >= 1)
        {
            exercise1.SetActive(true);
            exercise1.GetComponentInChildren<TextMeshProUGUI>().text = exerciseNames[0].ToString();
            if (exerciseNames.Count >= 2)
            {
                exercise2.SetActive(true);
                exercise2.GetComponentInChildren<TextMeshProUGUI>().text = exerciseNames[1].ToString();
                if (exerciseNames.Count >= 3)
                {
                    exercise3.SetActive(true);
                    exercise3.GetComponentInChildren<TextMeshProUGUI>().text = exerciseNames[2].ToString();
                }
            }
        }
    }

    public void ShowIndividualExercise(GameObject buttonChosen)
    {
        PanelSetActive(individualExercisePanel, true);
        individualExerciseName = GameObject.Find("IndividualExerciseName").GetComponentInChildren<TextMeshProUGUI>();
        individualExerciseName.text = buttonChosen.GetComponentInChildren<TextMeshProUGUI>().text;
        PanelSetActive(scrollArea, false);
        PanelSetActive(exercisesPanel, false);
        individualExerciseDescription = GameObject.Find("IndividualExerciseDescription").GetComponent<TextMeshProUGUI>();
        int index = exerciseDetails.GetArrayIndex(individualExerciseName.text);
        if (index != -1)
        {
            individualExerciseURL = exerciseDetails.GetArrayValue(index, 1);
            individualExerciseDescription.text = exerciseDetails.GetArrayValue(index, 2);
        }
    }

    public void ShowMuscleDiagram(string diagramToShow)
    {
        PanelSetActive(muscleDiagramPanel, true);
        PanelSetActive(individualExercisePanel, false);
        muscleDiagram = GameObject.Find("MuscleDiagram").GetComponent<Image>();
        muscleDiagram.GetComponent<Image>().sprite = Resources.Load<Sprite>(diagramToShow);
    }

    public void PlayExerciseVideo()
    {
        PanelSetActive(individualExercisePanel, false);
        Application.runInBackground = true;
        StartCoroutine(PlayTheExerciseVideo(individualExerciseURL));
    }

    IEnumerator PlayTheExerciseVideo(string url)
    {
        //Add VideoPlayer/AudioSource to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        //Pass URL to the source
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.Prepare();
        //Wait until video is prepared
        while (!videoPlayer.isPrepared) { yield return null; }
        animator.SetTrigger("TurnToTV");
        //Assign the Texture from Video to RawImage to be displayed
        tvScreen.texture = videoPlayer.texture;
        //Play Video & Audio
        videoPlayer.Play();
        audioSource.Play();

        while (videoPlayer.isPlaying) { yield return null; }
        animator.SetTrigger("TurnToCamera");
        tvScreen.texture = null;
        PanelSetActive(individualExercisePanel, true);
        audioSource = GameObject.Find("JimBot").GetComponentInChildren<AudioSource>();
    }
}