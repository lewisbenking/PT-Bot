using Google.Apis.Auth.OAuth2;
using Google.Apis.Dialogflow.v2beta1; //For chatbot connectivity
using System.Collections;
using System.IO;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using TMPro; //For textmesh
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DialogflowAPIScript : MonoBehaviour
{
    public InputField inputField;
    public RawImage image;
    private Animator animator;
    private ArrayList exerciseNames;
    private AudioClip audioClip;
    private AudioSource audioSource;
    private ExerciseDetails exerciseDetails;
    private GameObject pt, areasToTrainPanel, exercisesPanel, individualExercisePanel, muscleDiagramPanel, scrollArea, workoutEquipmentPanel, exercise1, exercise2, exercise3, exercise4;
    private Image muscleDiagram;
    private PlayVideoScript playVideoScript;
    private readonly VideoSource videoSource;
    private string url, diagramToShow, individualExerciseURL;
    private string AccessToken { get; set; }
    private string[] chatbotResponseSplit;
    private TextMeshProUGUI chatbotResponse, individualExerciseName, individualExerciseDescription;
    private Toggle barbellToggle, cableMachineToggle, dumbbellToggle;
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        scrollArea = GameObject.Find("ScrollArea");
        exercisesPanel = GameObject.Find("ExercisesPanel");
        PanelSetActive(exercisesPanel, false);
        individualExercisePanel = GameObject.Find("IndividualExercisePanel");
        PanelSetActive(individualExercisePanel, false);
        workoutEquipmentPanel = GameObject.Find("WorkoutEquipmentPanel");
        muscleDiagramPanel = GameObject.Find("MuscleDiagramPanel");
        PanelSetActive(muscleDiagramPanel, false);
        pt = GameObject.Find("PT");
        areasToTrainPanel = GameObject.Find("AreasToTrainPanel");
        PanelSetActive(areasToTrainPanel, false);
        audioSource = GameObject.Find("PT").GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        playVideoScript = new PlayVideoScript();
        exerciseDetails = new ExerciseDetails();
        animator = pt.GetComponent<Animator>();
        url = "https://dialogflow.googleapis.com/v2beta1/projects/pt-bot-d56dd/agent/sessions/34563:detectIntent";
        AccessToken = GetAccessToken();
        chatbotResponse = GameObject.Find("TextChatbotResponse").GetComponent<TextMeshProUGUI>();
        PlayAudio("IntroClip");
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void PanelSetActive(GameObject panel, bool isActive)
    {
        if (panel != null)
        {
            panel.SetActive(isActive);
        }
    }

    private void GetExerciseDetails(string chatbotResponse)
    {
        exerciseNames = new ArrayList();
        PanelSetActive(scrollArea, false);
        chatbotResponseSplit = chatbotResponse.Split('-');
        int index; string word;
        foreach (string iteration in chatbotResponseSplit)
        {
            // See if it matches an exercise
            word = iteration;
            word = word.Replace("workout for you!\n\n", ""); word = word.Replace("- ", ""); word = word.Replace(".\n", ""); word = word.Replace("\nYou can see more about each exercise by clicking the buttons.", "");
            index = exerciseDetails.GetArrayIndex(word);
            Debug.Log(word);
            if (index != -1)
            {
                exerciseNames.Add(word);
            }
        }
        // Set individual exercise panel to active
        PanelSetActive(exercisesPanel, true);
        exercise1 = GameObject.Find("ButtonExercise1");
        exercise2 = GameObject.Find("ButtonExercise2");
        exercise3 = GameObject.Find("ButtonExercise3");
        exercise4 = GameObject.Find("ButtonExercise4");
        exercise1.SetActive(false); exercise2.SetActive(false); exercise3.SetActive(false); exercise4.SetActive(false);

        // Assign the exercise names to buttons
        if (exerciseNames.Count > 1)
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
                    if (exerciseNames.Count == 4)
                    {
                        exercise4.SetActive(true);
                        exercise4.GetComponentInChildren<TextMeshProUGUI>().text = exerciseNames[3].ToString();
                    }
                }
            }
        }
    }

    private void ResponseHandler(string chatbotResponse)
    {
        if ((chatbotResponse.Contains("Bye")) || (chatbotResponse.Contains("Thanks for your time")) || (chatbotResponse.Contains("No worries, take care")) || (chatbotResponse.Contains("Thanks for using JimBot"))) { GoHome(); }
        PanelSetActive(areasToTrainPanel, (chatbotResponse.ToLower().Contains("which area would you like to train today")));
        if (chatbotResponse.Contains("Arms")) { diagramToShow = "ArmsDiagram"; }
        else if (chatbotResponse.Contains("Back")) { diagramToShow = "BackDiagram"; }
        else if (chatbotResponse.Contains("Chest")) { diagramToShow = "ChestDiagram"; }
        else if (chatbotResponse.Contains("Core")) { diagramToShow = "CoreDiagram"; }
        else if (chatbotResponse.Contains("Legs")) { diagramToShow = "LegsDiagram"; }

        // Get the exercise details
        if (chatbotResponse.ToLower().Contains("- ")) { GetExerciseDetails(chatbotResponse); }
    }

    private void PlayAudio(string fileName)
    {
        audioClip = WavUtility.ToAudioClip($"{Application.dataPath}/Audio/{fileName}.wav");
        audioSource.spatialBlend = 0.0f;
        audioSource.PlayOneShot(audioClip);
        Debug.Log("Played Audio");
    }

    private string GetEquipmentText()
    {
        barbellToggle = GameObject.Find("ToggleBarbell").GetComponent<Toggle>();
        cableMachineToggle = GameObject.Find("ToggleCableMachine").GetComponent<Toggle>();
        dumbbellToggle = GameObject.Find("ToggleDumbbells").GetComponent<Toggle>();
        string response = "";
        if (barbellToggle.isOn)
        {
            if (cableMachineToggle.isOn && dumbbellToggle.isOn)
            {
                response = "All 3";
            }
            else if (cableMachineToggle.isOn && !dumbbellToggle.isOn)
            {
                response = "Barbell and Cable Machine";
            }
            else if (!cableMachineToggle.isOn && dumbbellToggle.isOn)
            {
                response = "Barbell and Dumbbells";
            }
            else
            {
                response = "Barbell Only";
            }
        }
        else
        {
            if (cableMachineToggle.isOn && dumbbellToggle.isOn)
            {
                response = "Cable Machine and Dumbbells";
            }
            else if (cableMachineToggle.isOn && !dumbbellToggle.isOn)
            {
                response = "Cable Machine Only";
            }
            else if (!cableMachineToggle.isOn && dumbbellToggle.isOn)
            {
                response = "Dumbbells Only";
            }
            else
            {
                response = "None selected";
            }
        }
        return response;
    }

    public void SendSpeechToChatbot(string inputAudio)
    {
        PanelSetActive(areasToTrainPanel, false);
        PanelSetActive(exercisesPanel, false);
        animator.SetTrigger("Thinking");
        chatbotResponse.text = "I'm thinking of a response, please wait...";
        JsonDataAudioInput.RequestBody requestBody = CreateRequestBodyInputAudio(inputAudio);
        Debug.Log("Got Request Body");
        StartCoroutine(PostRequestAudio(requestBody));
    }

    public void SendTextToChatbot(string inputText)
    {
        if (inputText == "Continue") { inputText = GetEquipmentText(); }
        PanelSetActive(workoutEquipmentPanel, false);
        PanelSetActive(exercisesPanel, false);
        chatbotResponse.text = "I'm thinking of a response, please wait...";
        animator.SetTrigger("Thinking");
        JsonData.RequestBody requestBody = CreateRequestBodyInputText(inputText);
        Debug.Log(requestBody.ToString());
        StartCoroutine(PostRequestText(requestBody));
    }

    public void SendTextToChatbot()
    {
        PanelSetActive(areasToTrainPanel, false);
        PanelSetActive(exercisesPanel, false);
        if (String.IsNullOrWhiteSpace(inputField.text))
        {
            PlayAudio("DefaultErrorResponse");
        }
        else
        {
            chatbotResponse.text = "I'm thinking of a response, please wait...";
            animator.SetTrigger("Thinking");
            JsonData.RequestBody requestBody = CreateRequestBodyInputText(inputField.text.ToString());
            Debug.Log(requestBody.ToString());
            StartCoroutine(PostRequestText(requestBody));
        }
    }

    private string GetAccessToken()
    {
        //https://stackoverflow.com/questions/52607901/authenticating-request-with-google-apis-dialogflow-v2
        var credentials = GoogleCredential.FromJson("{  'type': 'service_account',  'project_id': 'pt-bot-d56dd',  'private_key_id': 'be933ba53ff9d381ad3afc49ce7501ac6e7b026e',  'private_key': '-----BEGIN PRIVATE KEY-----MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDSPZ/YWS94JMbGqLXvG7VgfQ//NWG7gzUpo2lRUwiDQ07XPXTszog4zyTDY72x6la9mlaBaK/fC/Yv5C/8tqIOJ+SO1nYpxEGnR1YXEhk5y7+K97V+FHxJmQuYWSrIpU9pV0w2GJz8h5uuMsAnwDMMmCboGSS1ncz2xgZcr4GPXG9z4TWZZTq51PemQNSYv6L5D1dPPG6kr4tV8ZWvd8Ko7UbJkqc000vi3pWlghjDNnxKp6u+5kCFteHcs/uu/ElJTHsMkWinF3CarR5hkeirWddVJMRJFgQAE6ujV4loqXupIPGb2D9og5kBOJb7Rk+YIU6RSFYn7p3S5+7V1WdZAgMBAAECggEADNioHjug22n/3V6ssz4RsKIjqpfz71W+l1s9UbNNp1ujAyLltJyQFUyO9gNvsWHcx/wYwhKIAIyGD/oU9o+gSlYksJepI7cyvcptl75K3U22WAL3y4rr50FbRIVaSGVVe13SsdGCMioFGLlQJX2ogOIBKphytkg8oG2MMPimZYCHee3d7AvtfI133rsS3wMeNKkvxgJI7FVraWqTnlZwvzxj5YGEUzHgBGUU7aDrUBcYj+7jCzO/9vMS4zXitARgNGTn8l9/FONhw8viqw1GCLNEsUGSZBN/Y8sdqvghu7HygMVULWIhrQsRTVJ9OjDHeq0q38qLejfd5F3sWbDtgQKBgQD8C9GEeXDvFMnc/9Xnceb918UEV8Q0LtiKCW64a6UI5pfOCgBWeVvLtwZ9kaKC9otLi/Plo3X/KgicYBg4aDJjr/NIsHGXg2z1pmru/5BH7gexaUNkWRMXGQMIsnCveyPPGS5pq7m+nV8FouoqwEsZuRpFngdwdM4wmC04W+nRuQKBgQDVievWdt/Fv6Pt2xDovDcFWx7C5GpUDlGB7sLrA9POlqFYiQOz3wetSYhKiRA0ybvdqyS8evXShqqAQB1ySS5QT0fZtKsWUR8C6vKIcExKV6fqHXi/Pq6bP3H4LwVM0VF+/+Vt13yyhLXk4jw7aOnHeGGuhs4AV0WgMnZkShWSoQKBgB/nvxXt6YXaM9Nt7z3lBUCM17u9AHE6nN6cYw+lULbXuc+zJGfN5Pjcqk2q6c96NhfSF4WyM3WhdIWXBHnfdsF3vGwvKbHsSRavgknOwAza7M5gbM9/FxONbvzi2bDc/aNxpJZrzo96jFTCUrImtVsEO3ckkfyCTLeKC+9eczLBAoGBAKLKdHqZQVsWEDkCqs9ivWdd4gOd8tmF2Ol/RiW4Uz7JYtOGEMaNnuKijj6UY0B7EreZA3aVHtaSR2Vie5Bm7eHXruTvcQagbU3iI2eUhPSgAqjeMvFJLf+4zH/yCM5ZPRHer9+fSbcmqSyGtHhuMNsakQ1mQ6HK5o+MKOmn+O5BAoGBANEGT/5lPsJtpqkf6pCihSyTlRcLIv4/Pe6KI3FMW8mPkBHtQ84RaBfw7CwtS/pnNfgKkq6BImvcrXwAUXJ+1YK1PFlQlWc70cFZiE+Kj1EFhXC//yeC2yuXTSx79hXeHmU8Ch7VrYX7JtGAKRnBrZtbQp4JPBzgWlrYE+PAqLcy-----END PRIVATE KEY-----',  'client_email': 'dialogflow-ejoxan@pt-bot-d56dd.iam.gserviceaccount.com',  'client_id': '106195813506666296547',  'auth_uri': 'https://accounts.google.com/o/oauth2/auth',  'token_uri': 'https://oauth2.googleapis.com/token',  'auth_provider_x509_cert_url': 'https://www.googleapis.com/oauth2/v1/certs',  'client_x509_cert_url': 'https://www.googleapis.com/robot/v1/metadata/x509/dialogflow-ejoxan%40pt-bot-d56dd.iam.gserviceaccount.com'}");
        var scopedCredentials = credentials.CreateScoped(DialogflowService.Scope.CloudPlatform);
        var oAuth2Token = scopedCredentials.UnderlyingCredential.GetAccessTokenForRequestAsync().Result;
        return oAuth2Token;
    }

    private JsonData.RequestBody CreateRequestBodyInputText(string inputText)
    {
        JsonData.RequestBody requestBody = new JsonData.RequestBody
        {
            queryInput = new JsonData.QueryInput
            {
                text = new JsonData.TextInput()
            }
        };
        requestBody.queryInput.text.text = inputText;
        requestBody.queryInput.text.languageCode = "en";
        return requestBody;
    }

    IEnumerator PostRequestText(JsonData.RequestBody requestBody)
    {
        UnityWebRequest postRequest = new UnityWebRequest(url, "POST");
        string jsonRequestBody = JsonUtility.ToJson(requestBody, true);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
        postRequest.SetRequestHeader("Authorization", $"Bearer {AccessToken}");
        postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        postRequest.SetRequestHeader("Content-Type", "application/json");
        yield return postRequest.SendWebRequest();
        Debug.Log("Got Post Request Response");

        if (postRequest.isNetworkError || postRequest.isHttpError)
        {
            Debug.Log(postRequest.responseCode);
            Debug.Log(postRequest.error);
        }
        else
        {
            // Or retrieve results as binary data
            byte[] resultbyte = postRequest.downloadHandler.data;
            string result = System.Text.Encoding.UTF8.GetString(resultbyte);
            animator.SetTrigger("ReverseThinking");
            Debug.Log(result);
            JsonData.ResponseBody content = (JsonData.ResponseBody)JsonUtility.FromJson<JsonData.ResponseBody>(result);
            Debug.Log(content.queryResult.fulfillmentText);
            chatbotResponse.text = content.queryResult.fulfillmentMessages[0].text.text[0].ToString();
            File.WriteAllBytes($"{Application.dataPath}/Audio/chatbotResponse.wav", Convert.FromBase64String(content.outputAudio));
            PlayAudio("chatbotResponse");
            ResponseHandler(chatbotResponse.text);
        }
    }

    private JsonDataAudioInput.RequestBody CreateRequestBodyInputAudio(string inputAudio)
    {
        JsonDataAudioInput.RequestBody requestBody = new JsonDataAudioInput.RequestBody
        {
            queryInput = new JsonDataAudioInput.QueryInput
            {
                audioConfig = new JsonDataAudioInput.InputAudioConfig()
            },
            inputAudio = inputAudio
        };
        requestBody.queryInput.audioConfig.audioEncoding = JsonDataAudioInput.AudioEncoding.AUDIO_ENCODING_LINEAR_16;
        requestBody.queryInput.audioConfig.sampleRateHertz = 48000;
        requestBody.queryInput.audioConfig.languageCode = "en-US";
        return requestBody;
    }

    IEnumerator PostRequestAudio(JsonDataAudioInput.RequestBody requestBody)
    {
        UnityWebRequest postRequest = new UnityWebRequest(url, "POST");
        string jsonRequestBody = JsonUtility.ToJson(requestBody, true);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
        postRequest.SetRequestHeader("Authorization", $"Bearer {AccessToken}");
        postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        postRequest.SetRequestHeader("Content-Type", "application/json");
        yield return postRequest.SendWebRequest();
        Debug.Log("Got Post Request Response");

        if (postRequest.isNetworkError || postRequest.isHttpError)
        {
            Debug.Log(postRequest.responseCode);
            Debug.Log(postRequest.error);
        }
        else
        {
            // Or retrieve results as binary data
            byte[] resultbyte = postRequest.downloadHandler.data;
            string result = System.Text.Encoding.UTF8.GetString(resultbyte);
            Debug.Log(result);
            JsonDataAudioInput.ResponseBody content = (JsonDataAudioInput.ResponseBody)JsonUtility.FromJson<JsonDataAudioInput.ResponseBody>(result);
            animator.SetTrigger("ReverseThinking");
            chatbotResponse.text = content.queryResult.fulfillmentText;
            if (content.queryResult.fulfillmentText == null)
            {
                chatbotResponse.text = "I didn't get that. Can you repeat?";
                PlayAudio("DefaultErrorResponse");
            }
            else
            {
                chatbotResponse.text = content.queryResult.fulfillmentMessages[0].text.text[0].ToString();
                File.WriteAllBytes($"{Application.dataPath}/Audio/chatbotResponse.wav", Convert.FromBase64String(content.outputAudio));
                PlayAudio("chatbotResponse");
                ResponseHandler(chatbotResponse.text);
            }
        }
    }

    public void PlayVideo(string url)
    {
        areasToTrainPanel = GameObject.Find("AreasToTrainPanel");
        if (areasToTrainPanel != null) { areasToTrainPanel.SetActive(false); }
        scrollArea = GameObject.Find("ScrollArea");
        if (scrollArea != null) { scrollArea.SetActive(false); }
        muscleDiagramPanel.SetActive(false);
        Application.runInBackground = true;
        this.url = url;
        pt = GameObject.Find("PT");
        animator = pt.GetComponent<Animator>();
        StartCoroutine(PlayTheVideo(url));
    }

    IEnumerator PlayTheVideo(string url)
    {
        //Add VideoPlayer/AudioSource to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;

        //Pass URL to the source
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.Prepare();

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        Debug.Log("Done Preparing Video");
        animator.SetTrigger("TurnToTV");
        //Assign the Texture from Video to RawImage to be displayed
        image.texture = videoPlayer.texture;

        //Play Video & Audio
        videoPlayer.Play();
        audioSource.Play();
        Debug.Log("Playing Video");

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }
        Debug.Log("Done Playing Video");
        animator.SetTrigger("TurnToUser");
        image.texture = null;
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

    public void ShowMuscleDiagram()
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

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;

        //Pass URL to the source
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.Prepare();

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        Debug.Log("Done Preparing Video");
        animator.SetTrigger("TurnToTV");
        //Assign the Texture from Video to RawImage to be displayed
        image.texture = videoPlayer.texture;

        //Play Video & Audio
        videoPlayer.Play();
        audioSource.Play();
        Debug.Log("Playing Video");

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }
        Debug.Log("Done Playing Video");
        animator.SetTrigger("TurnToUser");
        image.texture = null;
        PanelSetActive(individualExercisePanel, true);
    }
}