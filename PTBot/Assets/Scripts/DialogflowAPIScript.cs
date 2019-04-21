using Google.Apis.Auth.OAuth2;
using Google.Apis.Dialogflow.v2beta1;
using System.Collections;
using System.IO;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DialogflowAPIScript : MonoBehaviour
{
    public InputField inputField;
    private Animator animator;
    private AudioSource audioSource;
    private ExerciseDetails exerciseDetails;
    private GameObject jimBotCharacter, areasToTrainPanel, exercisesPanel, individualExercisePanel, muscleDiagramPanel, scrollArea, workoutEquipmentPanel, startWorkoutPanel, workoutEndPanel, lastPanel;
    private readonly VideoSource videoSource;
    private string url, diagramToShow;
    private string AccessToken { get; set; }
    private TextMeshProUGUI chatbotResponse;
    private JimBot jimBot;

    private ResponseHandler responseHandler;

    private void Awake()
    {
        jimBot = GameObject.Find("JimBot").AddComponent<JimBot>();
        scrollArea = GameObject.Find("ScrollArea");
        exercisesPanel = GameObject.Find("ExercisesPanel");
        jimBot.PanelSetActive(exercisesPanel, false);
        individualExercisePanel = GameObject.Find("IndividualExercisePanel");
        jimBot.PanelSetActive(individualExercisePanel, false);
        workoutEquipmentPanel = GameObject.Find("WorkoutEquipmentPanel");
        muscleDiagramPanel = GameObject.Find("MuscleDiagramPanel");
        jimBot.PanelSetActive(muscleDiagramPanel, false);
        areasToTrainPanel = GameObject.Find("AreasToTrainPanel");
        jimBot.PanelSetActive(areasToTrainPanel, false);
        startWorkoutPanel = GameObject.Find("StartWorkoutPanel");
        jimBot.PanelSetActive(startWorkoutPanel, false);
        workoutEndPanel = GameObject.Find("WorkoutEndPanel");
        jimBot.PanelSetActive(workoutEndPanel, false);
        jimBotCharacter = GameObject.Find("JimBot");
        audioSource = GameObject.Find("JimBot").GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        exerciseDetails = new ExerciseDetails();
        responseHandler = new ResponseHandler();
        animator = jimBotCharacter.GetComponent<Animator>();
        url = "https://dialogflow.googleapis.com/v2beta1/projects/pt-bot-d56dd/agent/sessions/34563:detectIntent";
        AccessToken = GetAccessToken();
        chatbotResponse = GameObject.Find("TextChatbotResponse").GetComponent<TextMeshProUGUI>();
        jimBot.PlayAudio("IntroClip");
    }

    private string GetAccessToken()
    {
        //https://stackoverflow.com/questions/52607901/authenticating-request-with-google-apis-dialogflow-v2
        var credentials = GoogleCredential.FromJson("{  'type': 'service_account',  'project_id': 'pt-bot-d56dd',  'private_key_id': 'be933ba53ff9d381ad3afc49ce7501ac6e7b026e',  'private_key': '-----BEGIN PRIVATE KEY-----MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDSPZ/YWS94JMbGqLXvG7VgfQ//NWG7gzUpo2lRUwiDQ07XPXTszog4zyTDY72x6la9mlaBaK/fC/Yv5C/8tqIOJ+SO1nYpxEGnR1YXEhk5y7+K97V+FHxJmQuYWSrIpU9pV0w2GJz8h5uuMsAnwDMMmCboGSS1ncz2xgZcr4GPXG9z4TWZZTq51PemQNSYv6L5D1dPPG6kr4tV8ZWvd8Ko7UbJkqc000vi3pWlghjDNnxKp6u+5kCFteHcs/uu/ElJTHsMkWinF3CarR5hkeirWddVJMRJFgQAE6ujV4loqXupIPGb2D9og5kBOJb7Rk+YIU6RSFYn7p3S5+7V1WdZAgMBAAECggEADNioHjug22n/3V6ssz4RsKIjqpfz71W+l1s9UbNNp1ujAyLltJyQFUyO9gNvsWHcx/wYwhKIAIyGD/oU9o+gSlYksJepI7cyvcptl75K3U22WAL3y4rr50FbRIVaSGVVe13SsdGCMioFGLlQJX2ogOIBKphytkg8oG2MMPimZYCHee3d7AvtfI133rsS3wMeNKkvxgJI7FVraWqTnlZwvzxj5YGEUzHgBGUU7aDrUBcYj+7jCzO/9vMS4zXitARgNGTn8l9/FONhw8viqw1GCLNEsUGSZBN/Y8sdqvghu7HygMVULWIhrQsRTVJ9OjDHeq0q38qLejfd5F3sWbDtgQKBgQD8C9GEeXDvFMnc/9Xnceb918UEV8Q0LtiKCW64a6UI5pfOCgBWeVvLtwZ9kaKC9otLi/Plo3X/KgicYBg4aDJjr/NIsHGXg2z1pmru/5BH7gexaUNkWRMXGQMIsnCveyPPGS5pq7m+nV8FouoqwEsZuRpFngdwdM4wmC04W+nRuQKBgQDVievWdt/Fv6Pt2xDovDcFWx7C5GpUDlGB7sLrA9POlqFYiQOz3wetSYhKiRA0ybvdqyS8evXShqqAQB1ySS5QT0fZtKsWUR8C6vKIcExKV6fqHXi/Pq6bP3H4LwVM0VF+/+Vt13yyhLXk4jw7aOnHeGGuhs4AV0WgMnZkShWSoQKBgB/nvxXt6YXaM9Nt7z3lBUCM17u9AHE6nN6cYw+lULbXuc+zJGfN5Pjcqk2q6c96NhfSF4WyM3WhdIWXBHnfdsF3vGwvKbHsSRavgknOwAza7M5gbM9/FxONbvzi2bDc/aNxpJZrzo96jFTCUrImtVsEO3ckkfyCTLeKC+9eczLBAoGBAKLKdHqZQVsWEDkCqs9ivWdd4gOd8tmF2Ol/RiW4Uz7JYtOGEMaNnuKijj6UY0B7EreZA3aVHtaSR2Vie5Bm7eHXruTvcQagbU3iI2eUhPSgAqjeMvFJLf+4zH/yCM5ZPRHer9+fSbcmqSyGtHhuMNsakQ1mQ6HK5o+MKOmn+O5BAoGBANEGT/5lPsJtpqkf6pCihSyTlRcLIv4/Pe6KI3FMW8mPkBHtQ84RaBfw7CwtS/pnNfgKkq6BImvcrXwAUXJ+1YK1PFlQlWc70cFZiE+Kj1EFhXC//yeC2yuXTSx79hXeHmU8Ch7VrYX7JtGAKRnBrZtbQp4JPBzgWlrYE+PAqLcy-----END PRIVATE KEY-----',  'client_email': 'dialogflow-ejoxan@pt-bot-d56dd.iam.gserviceaccount.com',  'client_id': '106195813506666296547',  'auth_uri': 'https://accounts.google.com/o/oauth2/auth',  'token_uri': 'https://oauth2.googleapis.com/token',  'auth_provider_x509_cert_url': 'https://www.googleapis.com/oauth2/v1/certs',  'client_x509_cert_url': 'https://www.googleapis.com/robot/v1/metadata/x509/dialogflow-ejoxan%40pt-bot-d56dd.iam.gserviceaccount.com'}");
        var scopedCredentials = credentials.CreateScoped(DialogflowService.Scope.CloudPlatform);
        var oAuth2Token = scopedCredentials.UnderlyingCredential.GetAccessTokenForRequestAsync().Result;
        return oAuth2Token;
    }

    public void SendSpeechToChatbot(string inputAudio)
    {
        //jimBot.PanelSetActive(workoutEquipmentPanel, false);
        //jimBot.PanelSetActive(areasToTrainPanel, false);
        //jimBot.PanelSetActive(exercisesPanel, false);
        animator.SetTrigger("Thinking");
        chatbotResponse.text = "I'm thinking of a response, please wait...";
        JsonDataAudioInput.RequestBody requestBody = CreateRequestBodyInputAudio(inputAudio);
        StartCoroutine(PostRequestAudio(requestBody));
    }

    public void SendTextToChatbot(string inputText)
    {
        if (inputText == "Continue") inputText = jimBot.GetEquipmentText();
        //jimBot.PanelSetActive(workoutEquipmentPanel, false);
        //jimBot.PanelSetActive(exercisesPanel, false);
        chatbotResponse.text = "I'm thinking of a response, please wait...";
        animator.SetTrigger("Thinking");
        JsonData.RequestBody requestBody = CreateRequestBodyInputText(inputText);
        StartCoroutine(PostRequestText(requestBody));
    }

    public void SendTextToChatbot()
    {
        //jimBot.PanelSetActive(workoutEquipmentPanel, false);
        //jimBot.PanelSetActive(areasToTrainPanel, false);
        //jimBot.PanelSetActive(exercisesPanel, false);
        if (String.IsNullOrWhiteSpace(inputField.text))
        {
            jimBot.PlayAudio("DefaultErrorResponse");
        }
        else
        {
            chatbotResponse.text = "I'm thinking of a response, please wait...";
            animator.SetTrigger("Thinking");
            JsonData.RequestBody requestBody = CreateRequestBodyInputText(inputField.text.ToString());
            inputField.text = "";
            StartCoroutine(PostRequestText(requestBody));
        }
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
            File.WriteAllBytes($"{Application.dataPath}/Audio/ChatbotResponse.wav", Convert.FromBase64String(content.outputAudio));
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
                jimBot.PlayAudio("DefaultErrorResponse");
            }
            else
            {
                chatbotResponse.text = content.queryResult.fulfillmentMessages[0].text.text[0].ToString();
                File.WriteAllBytes($"{Application.dataPath}/Audio/chatbotResponse.wav", Convert.FromBase64String(content.outputAudio));
                ResponseHandler(chatbotResponse.text);
            }
        }
    }

    public void ShowIndividualExercise(GameObject buttonChosen) { jimBot.ShowIndividualExercise(buttonChosen); }

    public void ShowMuscleDiagram() { jimBot.ShowMuscleDiagram(diagramToShow); }

    public void PlayExerciseVideo()
    {
        Application.runInBackground = true;
        jimBot.PlayExerciseVideo();
    }

    public void StartWorkout() { jimBot.StartWorkoutTimer(); }

    public void GoBackToExerciseList() { jimBot.GoBackToExerciseList(); }

    public void NextExercise() { jimBot.NextExercise(); }

    public void LoadScene(int sceneNumber) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - sceneNumber); }

    public void PanelSetActive(GameObject panel, bool isActive)
    {
        if (panel != null) panel.SetActive(isActive);
    }

    private void ResponseHandler(string chatbotResponse)
    {
        responseHandler.HandleResponse(chatbotResponse);
        jimBot.PlayAudio(responseHandler.CheckAudioToPlay());
        if (responseHandler.CheckGoHome()) LoadScene(1);
        if (!string.IsNullOrWhiteSpace(responseHandler.CheckDiagramToShow())) diagramToShow = responseHandler.CheckDiagramToShow();
        
        if (responseHandler.PanelsToSetActive().Contains("AreasToTrainPanel"))
        {
            jimBot.PanelSetActive(areasToTrainPanel, true);
            jimBot.PanelSetActive(scrollArea, true);
            jimBot.PanelSetActive(startWorkoutPanel, false);
            jimBot.PanelSetActive(exercisesPanel, false);
            jimBot.PanelSetActive(individualExercisePanel, false);
            jimBot.PanelSetActive(muscleDiagramPanel, false);
            jimBot.PanelSetActive(workoutEquipmentPanel, false);
        }

        if (responseHandler.PanelsToSetActive().Contains("StartWorkoutPanel"))
        {
            jimBot.PanelSetActive(areasToTrainPanel, false);
            jimBot.PanelSetActive(scrollArea, false);
            jimBot.PanelSetActive(startWorkoutPanel, true);
            jimBot.PanelSetActive(exercisesPanel, false);
            jimBot.PanelSetActive(individualExercisePanel, false);
            jimBot.PanelSetActive(muscleDiagramPanel, false);
            jimBot.PanelSetActive(workoutEquipmentPanel, false);
        }

        if (responseHandler.PanelsToSetActive().Contains("ExercisesPanel"))
        {
            jimBot.PanelSetActive(areasToTrainPanel, false);
            jimBot.PanelSetActive(scrollArea, false);
            jimBot.PanelSetActive(startWorkoutPanel, false);
            jimBot.PanelSetActive(exercisesPanel, true);
            jimBot.PanelSetActive(individualExercisePanel, false);
            jimBot.PanelSetActive(muscleDiagramPanel, false);
            jimBot.PanelSetActive(workoutEquipmentPanel, false);
        }

        if (responseHandler.PanelsToSetActive().Contains("IndividualExercisePanel"))
        {
            jimBot.PanelSetActive(areasToTrainPanel, false);
            jimBot.PanelSetActive(scrollArea, false);
            jimBot.PanelSetActive(startWorkoutPanel, false);
            jimBot.PanelSetActive(exercisesPanel, false);
            jimBot.PanelSetActive(individualExercisePanel, true);
            jimBot.PanelSetActive(muscleDiagramPanel, false);
            jimBot.PanelSetActive(workoutEquipmentPanel, false);
        }

        if (responseHandler.PanelsToSetActive().Contains("MuscleDiagramPanel"))
        {
            jimBot.PanelSetActive(areasToTrainPanel, false);
            jimBot.PanelSetActive(scrollArea, false);
            jimBot.PanelSetActive(startWorkoutPanel, false);
            jimBot.PanelSetActive(exercisesPanel, false);
            jimBot.PanelSetActive(individualExercisePanel, false);
            jimBot.PanelSetActive(muscleDiagramPanel, true);
            jimBot.PanelSetActive(workoutEquipmentPanel, false);
        }

        /*
        jimBot.PanelSetActive(areasToTrainPanel, responseHandler.PanelsToSetActive().Contains("AreasToTrainPanel"));
        jimBot.PanelSetActive(scrollArea, responseHandler.PanelsToSetActive().Contains("scrollArea"));
        jimBot.PanelSetActive(startWorkoutPanel, responseHandler.PanelsToSetActive().Contains("StartWorkoutPanel"));
        jimBot.PanelSetActive(exercisesPanel, responseHandler.PanelsToSetActive().Contains("ExercisesPanel"));
        jimBot.PanelSetActive(individualExercisePanel, responseHandler.PanelsToSetActive().Contains("IndividualExercisePanel"));
        jimBot.PanelSetActive(muscleDiagramPanel, responseHandler.PanelsToSetActive().Contains("MuscleDiagramPanel"));
        */
        if (responseHandler.CheckGetExerciseDetails())
        {
            jimBot.GetExerciseDetails(chatbotResponse);
        }
        else if (responseHandler.CheckStartWorkout())
        {
            StartWorkout();
        }
    }
}