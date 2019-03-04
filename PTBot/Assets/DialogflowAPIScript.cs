using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using JsonData;
using UnityEngine.UI;
using System.IO;
using Google.Apis.Dialogflow.v2beta1;
using Google.Apis.Auth.OAuth2;

//https://stackoverflow.com/questions/51272889/unable-to-send-post-request-to-dialogflow-404

public class DialogflowAPIScript : MonoBehaviour
{
    public InputField inputField;
    private string url = "https://dialogflow.googleapis.com/v2beta1/projects/pt-bot-d56dd/agent/sessions/34563:detectIntent";
    private string accessToken { get; set; }//= "ya29.c.El_CBitJqntwQ34NmI1sfOS8uYIVqxRsf51AZYKRUHRXZY9wq32YpvJOYTorrZU7_vc7ibFUe-V-jUz72G2XdvQ-U6sUpdzmFBnnOjpVyPTwSU6GTTlJTawJB0GH5NmOOQ";
    // ^ NEED TO FIGURE OUT HOW TO GET ACCESS TOKEN EVERY TIME AS IT EXPIRES EVERY HOUR!
    private UnityEngine.UI.Text chatbotResponse;

    public void Test()
    {
        accessToken = GetAccessToken();
        chatbotResponse = GameObject.Find("TextChatbotResponse").GetComponent<UnityEngine.UI.Text>();
        Debug.Log(inputField.text.ToString());
        StartCoroutine(PostRequest(inputField.text.ToString()));
    }

    private string GetAccessToken()
    {
        var credentials = GoogleCredential.FromFile(@"C:\Users\lbk95\Documents\_ASTON\GoogleServiceAccount\pt-bot-d56dd-be933ba53ff9.json");
        var scopedCredentials = credentials.CreateScoped(DialogflowService.Scope.CloudPlatform);
        var _oAuthToken = scopedCredentials.UnderlyingCredential.GetAccessTokenForRequestAsync().Result;
        Debug.Log(_oAuthToken);
        return _oAuthToken;
    }


    IEnumerator PostRequest(String inputMessage)
    {
        UnityWebRequest postRequest = new UnityWebRequest(url, "POST");
        RequestBody requestBody = new RequestBody
        {
            queryInput = new QueryInput
            {
                text = new TextInput()
            }
        };
        requestBody.queryInput.text.text = inputMessage;
        requestBody.queryInput.text.languageCode = "en";

        string jsonRequestBody = JsonUtility.ToJson(requestBody, true);
        Debug.Log(jsonRequestBody);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
        //Debug.Log(bodyRaw);
        postRequest.SetRequestHeader("Authorization", "Bearer " + accessToken);
        postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        postRequest.SetRequestHeader("Content-Type", "application/json");

        yield return postRequest.SendWebRequest();

        if (postRequest.isNetworkError || postRequest.isHttpError)
        {
            Debug.Log(postRequest.responseCode);
            Debug.Log(postRequest.error);
        }
        else
        {
            // Show results as text
            Debug.Log("Response: " + postRequest.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] resultbyte = postRequest.downloadHandler.data;
            string result = System.Text.Encoding.UTF8.GetString(resultbyte);
            ResponseBody content = (ResponseBody)JsonUtility.FromJson<ResponseBody>(result);
            Debug.Log(content.queryResult.fulfillmentText);
            Debug.Log(content.outputAudio);
            chatbotResponse.text = content.queryResult.fulfillmentText;
            File.WriteAllBytes(@"C:\test\test.wav", Convert.FromBase64String(content.outputAudio));
            PlayAudio(content.outputAudio);
        }
    }

    //https://stackoverflow.com/questions/35228767/noisy-audio-clip-after-decoding-from-base64

    private void PlayAudio(string base64)
    {
        byte[] bytes = Convert.FromBase64String(base64);
        float[] f = ConvertByteToFloat(bytes);

        //Normalize(f);
        /*
        foreach (float floaty in f)
        {
            if (floaty > 1.0f || floaty < 0.0f)
                Debug.Log(floaty);
        }*/

        AudioClip audioClip = AudioClip.Create("testSound", f.Length, 2, 44100, false);
        audioClip.SetData(f, 0);

        AudioSource audioSource = GameObject.FindObjectOfType<AudioSource>();
        audioSource.spatialBlend = 0.0f;
        audioSource.PlayOneShot(audioClip);
        return;
    }

    private float[] ConvertByteToFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 4];

        for (int i = 0; i < floatArr.Length; i++)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(array, i * 4, 4);

            floatArr[i] = BitConverter.ToSingle(array, i * 4) / 0x80000000;
        }

        return floatArr;
    }
}