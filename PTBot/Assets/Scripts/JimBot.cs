﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class JimBot : MonoBehaviour
{
    private Animator animator;
    private ArrayList exerciseNames;
    private AudioClip audioClip;
    private AudioSource audioSource;
    private bool isWorkoutStopped, hasWorkoutBeenPaused;
    private ExerciseDetails exerciseDetails;
    private float timerTime;
    private GameObject jimBot, areasToTrainPanel, exercisesPanel, individualExercisePanel, muscleDiagramPanel, scrollArea, workoutEquipmentPanel, startWorkoutPanel, workoutEndPanel, exercise1, exercise2, exercise3, HUD;
    private Image muscleDiagram;
    private int exerciseNameArrayIndex;
    private RawImage tvScreen;
    private readonly VideoSource videoSource;
    private string individualExerciseURL;
    private TextMeshProUGUI individualExerciseName, currentExercise, individualExerciseDescription, timerLabel;
    private Toggle barbellToggle, cableMachineToggle, dumbbellToggle;
    private VideoPlayer videoPlayer;

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
        HUD = GameObject.Find("HUD");
        jimBot = GameObject.Find("JimBot");
        tvScreen = GameObject.Find("TV Screen").GetComponent<RawImage>();
        audioSource = GameObject.Find("JimBot").GetComponentInChildren<AudioSource>();
        timerLabel = GameObject.Find("WorkoutTimer").GetComponent<TextMeshProUGUI>();
        currentExercise = GameObject.Find("CurrentExercise").GetComponentInChildren<TextMeshProUGUI>();
        timerTime = 600f;
        exerciseNameArrayIndex = 0;
    }

    private void Start()
    {
        exerciseDetails = new ExerciseDetails();
        animator = jimBot.GetComponent<Animator>();
    }

    // Counts down from 10 minutes, updates timer label every second, and acts at 5 minutes and 0 minutes accordingly
    // For pausing the timer, general help was provided from https://forum.unity.com/threads/how-to-pause-a-coroutine.161479/
    private IEnumerator TimerCountdown()
    {
        while (timerTime > 0f && !isWorkoutStopped)
        {
            if (timerTime == 300f)
            {
                animator.SetTrigger("WellDone");
                PlayAudio("HalfwayTime");
            }

            if (exerciseNameArrayIndex <= (exerciseNames.Count - 1))
            {
                currentExercise.text = exerciseNames[exerciseNameArrayIndex].ToString();
                timerLabel.text = $"Time Remaining:  {Mathf.Floor(timerTime / 60).ToString("00")}:{(timerTime % 60).ToString("00")}";
            }
            else
            {
                EndWorkout();
            }
            yield return new WaitForSeconds(1);
            timerTime--;
        }
        NextExercise();
        yield return null;
    }

    // Reset timer, starts the next exercise
    public void NextExercise()
    {
        timerTime = 600f;
        exerciseNameArrayIndex++;
        if (exerciseNameArrayIndex > (exerciseNames.Count - 1))
        {
            StopCoroutine("TimerCountdown");
            EndWorkout();
        }
        else
        {
            if (exerciseNameArrayIndex == (exerciseNames.Count - 1)) PlayAudio("TheLastExercise");
            else PlayAudio("NextExercise");
            StopCoroutine("TimerCountdown");
            StartCoroutine("TimerCountdown");
        }
    }

    // Starts the workout timer
    public void StartWorkoutTimer()
    {
        isWorkoutStopped = false;
        if (hasWorkoutBeenPaused)
        {
            PlayAudio("ResumeWorkout");
            animator.SetTrigger("WellDone");
        }
        else PlayAudio("StartWorkout");
        StartCoroutine("TimerCountdown");
    }

    // Returns the user to the exercise list screen
    public void GoBackToExerciseList()
    {
        StopCoroutine("TimerCountdown");
        isWorkoutStopped = true;
        hasWorkoutBeenPaused = true;
        PanelSetActive(exercisesPanel, true);
        PanelSetActive(startWorkoutPanel, false);
    }

    // Ends the workout and changes the active panel
    public void EndWorkout()
    {
        isWorkoutStopped = true;
        StopCoroutine("TimerCountdown");
        PanelSetActive(workoutEndPanel, true);
        PanelSetActive(startWorkoutPanel, false);
        animator.SetTrigger("WellDone");
        PlayAudio("CompletedWorkout");
    }

    // Loads a scene
    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    // Sets the panel active/inactive
    public void PanelSetActive(GameObject panel, bool isActive)
    {
        if (panel != null) panel.SetActive(isActive);
    }

    // Plays the audio clip
    public void PlayAudio(string fileName)
    {
        audioSource.Stop();
        audioClip = WavUtility.ToAudioClip($"{Application.dataPath}/Audio/{fileName}.wav");
        audioSource.spatialBlend = 0.0f;
        audioSource.PlayOneShot(audioClip);
    }

    // Returns the selected equipment from the user
    public string GetEquipmentText()
    {
        barbellToggle = GameObject.Find("ToggleBarbell").GetComponent<Toggle>();
        cableMachineToggle = GameObject.Find("ToggleCableMachine").GetComponent<Toggle>();
        dumbbellToggle = GameObject.Find("ToggleDumbbells").GetComponent<Toggle>();
        if (barbellToggle.isOn)
        {
            if (cableMachineToggle.isOn && dumbbellToggle.isOn) return "All 3";
            else if (cableMachineToggle.isOn && !dumbbellToggle.isOn) return "Barbell and Cable Machine";
            else if (!cableMachineToggle.isOn && dumbbellToggle.isOn) return "Barbell and Dumbbells";
            else return "Barbell Only";
        }
        else
        {
            if (cableMachineToggle.isOn && dumbbellToggle.isOn) return "Cable Machine and Dumbbells";
            else if (cableMachineToggle.isOn && !dumbbellToggle.isOn) return "Cable Machine Only";
            else if (!cableMachineToggle.isOn && dumbbellToggle.isOn) return "Dumbbells Only";
            else return "None selected";
        }
    }

    // Setup for the exercises list panel, with up to 3 buttons being shown if 3 exercises are in the chatbot response
    // General help for accessing inactive gameobject was given from https://answers.unity.com/questions/890636/find-an-inactive-game-object.html
    public void GetExerciseDetails(string chatbotResponse)
    {
        exerciseNames = new ArrayList();
        PanelSetActive(scrollArea, false);
        PanelSetActive(exercisesPanel, true);
        string[] chatbotResponseSplit = chatbotResponse.Split('-');
        int index; string word;
        foreach (string iteration in chatbotResponseSplit)
        {
            word = iteration;
            word = word.Replace("workout for you!\n\n", ""); word = word.Replace("- ", ""); word = word.Replace(".\n", ""); word = word.Replace("\nYou can see more about each exercise by selecting them from the list\nWhen you're ready to work out, please select \"Start Workout\".", "");
            index = exerciseDetails.GetArrayIndex(word);
            if (index != -1) exerciseNames.Add(word);
        }

        exercise1 = exercisesPanel.transform.Find("ButtonExercise1").gameObject;
        exercise2 = exercisesPanel.transform.Find("ButtonExercise2").gameObject;
        exercise3 = exercisesPanel.transform.Find("ButtonExercise3").gameObject;
        exercise1.SetActive(false);
        exercise2.SetActive(false);
        exercise3.SetActive(false);

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

    // Shows the individual exercise details in another panel
    public void ShowIndividualExercise(GameObject buttonChosen)
    {
        PanelSetActive(scrollArea, false);
        PanelSetActive(exercisesPanel, false);
        PanelSetActive(individualExercisePanel, true);
        individualExerciseName = GameObject.Find("IndividualExerciseName").GetComponentInChildren<TextMeshProUGUI>();
        individualExerciseName.text = buttonChosen.GetComponentInChildren<TextMeshProUGUI>().text;
        individualExerciseDescription = GameObject.Find("IndividualExerciseDescription").GetComponent<TextMeshProUGUI>();
        int index = exerciseDetails.GetArrayIndex(individualExerciseName.text);
        if (index != -1)
        {
            individualExerciseURL = exerciseDetails.GetArrayValue(index, 1);
            individualExerciseDescription.text = exerciseDetails.GetArrayValue(index, 2);
        }
    }

    // Shows the muscle diagram panel
    public void ShowMuscleDiagram(string diagramToShow)
    {
        PanelSetActive(muscleDiagramPanel, true);
        PanelSetActive(individualExercisePanel, false);
        muscleDiagram = GameObject.Find("MuscleDiagram").GetComponent<Image>();
        muscleDiagram.GetComponent<Image>().sprite = Resources.Load<Sprite>(diagramToShow);
    }

    // Plays the exercise video
    public void PlayExerciseVideo()
    {
        PanelSetActive(individualExercisePanel, false);
        Application.runInBackground = true;
        StartCoroutine(PlayTheExerciseVideo(individualExerciseURL));
    }

    IEnumerator PlayTheExerciseVideo(string url)
    {
        PanelSetActive(HUD, false);
        audioSource.Stop();
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared) yield return null;
        animator.SetTrigger("TurnToTV");
        tvScreen.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
        while (videoPlayer.isPlaying) yield return null;
        animator.SetTrigger("TurnToCamera");
        tvScreen.texture = null;
        PanelSetActive(individualExercisePanel, true);
        audioSource = GameObject.Find("JimBot").GetComponentInChildren<AudioSource>();
        PanelSetActive(HUD, true);
    }

    // Plays the relevant help audio depending on the active panel
    public void HelpMe()
    {
        if (areasToTrainPanel.activeInHierarchy) PlayAudio("Help_AreasToTrainPanel");
        else if (exercisesPanel.activeInHierarchy) PlayAudio("Help_ExercisesPanel");
        else if (individualExercisePanel.activeInHierarchy) PlayAudio("Help_IndividualExercisePanel");
        else if (muscleDiagramPanel.activeInHierarchy) PlayAudio("Help_IndividualExercisePanel");
        else if (startWorkoutPanel.activeInHierarchy) PlayAudio("Help_StartWorkoutPanel");
        else if (workoutEndPanel.activeInHierarchy) PlayAudio("Help_WorkoutEndPanel");
        else if (workoutEquipmentPanel.activeInHierarchy) PlayAudio("Help_WorkoutEquipmentPanel");
    }
}