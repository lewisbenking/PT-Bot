using CrazyMinnow.SALSA.Fuse;
using CrazyMinnow.SALSA;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Dialogflow.v2beta1;
using JsonData;
using JsonDataAudioInput;
using System.Collections;
using System.IO;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class DialogflowAPIScript : MonoBehaviour
{
    private AudioClip audioClip;
    private AudioSource audioSource;
    private string url;
    private Salsa3D salsa3D;
    private string AccessToken { get; set; }
    private UnityEngine.UI.Text chatbotResponse;
    public InputField inputField;
    private GameObject pt;
    private Animator animator;

    public void SendSpeechToChatbot(string inputAudio)
    {
        pt = GameObject.Find("PT");
        animator = pt.GetComponent<Animator>();
        animator.SetBool("waitingForChatbotResponse", true);
        url = "https://dialogflow.googleapis.com/v2beta1/projects/pt-bot-d56dd/agent/sessions/34563:detectIntent";
        Debug.Log(JsonUtility.ToJson(CreateRequestBodyInputAudio(inputAudio), true));
        AccessToken = GetAccessToken();
        Debug.Log("Got Access Token");
        Debug.Log(AccessToken);
        chatbotResponse = GameObject.Find("TextChatbotResponse").GetComponent<UnityEngine.UI.Text>();
        chatbotResponse.text = "I'm thinking of a response";
        JsonDataAudioInput.RequestBody requestBody = CreateRequestBodyInputAudio(inputAudio);
        Debug.Log("Got Request Body");
        StartCoroutine(PostRequestAudio(requestBody));
    }

    public void SendTextToChatbot()
    {
        pt = GameObject.Find("PT");
        animator = pt.GetComponent<Animator>();
        animator.SetBool("waitingForChatbotResponse", true);
        url = "https://dialogflow.googleapis.com/v2beta1/projects/pt-bot-d56dd/agent/sessions/34563:detectIntent";
        AccessToken = GetAccessToken();
        Debug.Log("Got Access Token");
        chatbotResponse = GameObject.Find("TextChatbotResponse").GetComponent<UnityEngine.UI.Text>();
        chatbotResponse.text = "I'm thinking of a response";
        JsonData.RequestBody requestBody = CreateRequestBodyInputText();
        Debug.Log("Got Request Body");
        Debug.Log(requestBody.ToString());
        StartCoroutine(PostRequestText(requestBody));
    }

    private string GetAccessToken()
    {
        //https://stackoverflow.com/questions/52607901/authenticating-request-with-google-apis-dialogflow-v2
        var credentials = GoogleCredential.FromJson("{  'type': 'service_account',  'project_id': 'pt-bot-d56dd',  'private_key_id': 'be933ba53ff9d381ad3afc49ce7501ac6e7b026e',  'private_key': '-----BEGIN PRIVATE KEY-----MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDSPZ/YWS94JMbGqLXvG7VgfQ//NWG7gzUpo2lRUwiDQ07XPXTszog4zyTDY72x6la9mlaBaK/fC/Yv5C/8tqIOJ+SO1nYpxEGnR1YXEhk5y7+K97V+FHxJmQuYWSrIpU9pV0w2GJz8h5uuMsAnwDMMmCboGSS1ncz2xgZcr4GPXG9z4TWZZTq51PemQNSYv6L5D1dPPG6kr4tV8ZWvd8Ko7UbJkqc000vi3pWlghjDNnxKp6u+5kCFteHcs/uu/ElJTHsMkWinF3CarR5hkeirWddVJMRJFgQAE6ujV4loqXupIPGb2D9og5kBOJb7Rk+YIU6RSFYn7p3S5+7V1WdZAgMBAAECggEADNioHjug22n/3V6ssz4RsKIjqpfz71W+l1s9UbNNp1ujAyLltJyQFUyO9gNvsWHcx/wYwhKIAIyGD/oU9o+gSlYksJepI7cyvcptl75K3U22WAL3y4rr50FbRIVaSGVVe13SsdGCMioFGLlQJX2ogOIBKphytkg8oG2MMPimZYCHee3d7AvtfI133rsS3wMeNKkvxgJI7FVraWqTnlZwvzxj5YGEUzHgBGUU7aDrUBcYj+7jCzO/9vMS4zXitARgNGTn8l9/FONhw8viqw1GCLNEsUGSZBN/Y8sdqvghu7HygMVULWIhrQsRTVJ9OjDHeq0q38qLejfd5F3sWbDtgQKBgQD8C9GEeXDvFMnc/9Xnceb918UEV8Q0LtiKCW64a6UI5pfOCgBWeVvLtwZ9kaKC9otLi/Plo3X/KgicYBg4aDJjr/NIsHGXg2z1pmru/5BH7gexaUNkWRMXGQMIsnCveyPPGS5pq7m+nV8FouoqwEsZuRpFngdwdM4wmC04W+nRuQKBgQDVievWdt/Fv6Pt2xDovDcFWx7C5GpUDlGB7sLrA9POlqFYiQOz3wetSYhKiRA0ybvdqyS8evXShqqAQB1ySS5QT0fZtKsWUR8C6vKIcExKV6fqHXi/Pq6bP3H4LwVM0VF+/+Vt13yyhLXk4jw7aOnHeGGuhs4AV0WgMnZkShWSoQKBgB/nvxXt6YXaM9Nt7z3lBUCM17u9AHE6nN6cYw+lULbXuc+zJGfN5Pjcqk2q6c96NhfSF4WyM3WhdIWXBHnfdsF3vGwvKbHsSRavgknOwAza7M5gbM9/FxONbvzi2bDc/aNxpJZrzo96jFTCUrImtVsEO3ckkfyCTLeKC+9eczLBAoGBAKLKdHqZQVsWEDkCqs9ivWdd4gOd8tmF2Ol/RiW4Uz7JYtOGEMaNnuKijj6UY0B7EreZA3aVHtaSR2Vie5Bm7eHXruTvcQagbU3iI2eUhPSgAqjeMvFJLf+4zH/yCM5ZPRHer9+fSbcmqSyGtHhuMNsakQ1mQ6HK5o+MKOmn+O5BAoGBANEGT/5lPsJtpqkf6pCihSyTlRcLIv4/Pe6KI3FMW8mPkBHtQ84RaBfw7CwtS/pnNfgKkq6BImvcrXwAUXJ+1YK1PFlQlWc70cFZiE+Kj1EFhXC//yeC2yuXTSx79hXeHmU8Ch7VrYX7JtGAKRnBrZtbQp4JPBzgWlrYE+PAqLcy-----END PRIVATE KEY-----',  'client_email': 'dialogflow-ejoxan@pt-bot-d56dd.iam.gserviceaccount.com',  'client_id': '106195813506666296547',  'auth_uri': 'https://accounts.google.com/o/oauth2/auth',  'token_uri': 'https://oauth2.googleapis.com/token',  'auth_provider_x509_cert_url': 'https://www.googleapis.com/oauth2/v1/certs',  'client_x509_cert_url': 'https://www.googleapis.com/robot/v1/metadata/x509/dialogflow-ejoxan%40pt-bot-d56dd.iam.gserviceaccount.com'}");
        var scopedCredentials = credentials.CreateScoped(DialogflowService.Scope.CloudPlatform);
        var oAuth2Token = scopedCredentials.UnderlyingCredential.GetAccessTokenForRequestAsync().Result;
        return oAuth2Token;
    }

    private JsonData.RequestBody CreateRequestBodyInputText()
    {
        JsonData.RequestBody requestBody = new JsonData.RequestBody
        {
            queryInput = new JsonData.QueryInput
            {
                text = new JsonData.TextInput()
            }
        };
        requestBody.queryInput.text.text = inputField.text.ToString();
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
            animator.SetBool("waitingForChatbotResponse", false);
            // Or retrieve results as binary data
            byte[] resultbyte = postRequest.downloadHandler.data;
            string result = System.Text.Encoding.UTF8.GetString(resultbyte);
            JsonData.ResponseBody content = (JsonData.ResponseBody)JsonUtility.FromJson<JsonData.ResponseBody>(result);
            Debug.Log(content.queryResult.fulfillmentText);
            chatbotResponse.text = content.queryResult.fulfillmentText;
            chatbotResponse.text = content.queryResult.fulfillmentMessages[0].text.text[0].ToString();
            GetAndPlayAudio(content.outputAudio);
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
            animator.SetBool("waitingForChatbotResponse", false);
            Debug.Log(content.queryResult.fulfillmentText);
            chatbotResponse.text = content.queryResult.fulfillmentText;
            chatbotResponse.text = content.queryResult.fulfillmentMessages[0].text.text[0].ToString();
            GetAndPlayAudio(content.outputAudio);
        }
    }

    private void GetAndPlayAudio(string outputAudio)
    {
        File.WriteAllBytes($"{Application.dataPath}/chatbotResponse.wav", Convert.FromBase64String(outputAudio));
        audioClip = WavUtility.ToAudioClip($"{Application.dataPath}/chatbotResponse.wav");
        //File.WriteAllBytes(string.Format("{0}/{1}", Application.dataPath, "chatbotResponse.wav"), Convert.FromBase64String(outputAudio));
        //audioClip = WavUtility.ToAudioClip(string.Format("{0}/{1}", Application.dataPath, "chatbotResponse.wav"));
        audioSource = GameObject.Find("PT").GetComponentInChildren<AudioSource>();
        audioSource.spatialBlend = 0.0f;
        audioSource.PlayOneShot(audioClip);
        Debug.Log("Played Audio");
        return;
    }
}

/*
  "fulfillmentText": "Sorry, what was that?",
    "fulfillmentMessages": [
      {
        "text": {
          "text": [
            "One more time?"
          ]
        }
      }
    ],
 */
