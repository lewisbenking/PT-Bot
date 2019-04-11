using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]

public class MicInput : MonoBehaviour
{
    private bool micConnected = false;
    private int minFreq, maxFreq;
    private AudioSource goAudioSource;
    public AudioClip recordedClip;
    private float[] samples;
    private DialogflowAPIScript dialogFlowAPIScript;
    private Button buttonTalkToBot;
    private InputField inputField;

    void Start()
    {
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        if (Microphone.devices.Length <= 0)
        {
            Debug.LogWarning("Microphone not connected!");
        }
        else
        {
            micConnected = true;
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
            if (minFreq == 0 && maxFreq == 0) { maxFreq = 44100; }
            goAudioSource = this.GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && (String.IsNullOrWhiteSpace(inputField.text)))  { RecordAudio(); }
    }

    public void RecordAudio()
    {
        buttonTalkToBot = GameObject.Find("ButtonSendVoiceToChatbot").GetComponent<Button>();

        if (micConnected)
        {
            if (!Microphone.IsRecording(null))
            {
                buttonTalkToBot.GetComponentInChildren<TextMeshProUGUI>().text = "Stop Recording";
                goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq);
                recordedClip = goAudioSource.clip;
                samples = new float[goAudioSource.clip.samples];
            }
            else
            {
                buttonTalkToBot.GetComponentInChildren<TextMeshProUGUI>().text = "Talk to JimBot";
                Microphone.End(null); //Stop the audio recording  
                Convert.ToByte(goAudioSource.clip);
                byte[] byteArray = WavUtility.FromAudioClip(goAudioSource.clip);
                string base64string = Convert.ToBase64String(byteArray);
                Debug.Log(base64string);
                dialogFlowAPIScript = GameObject.Find("DialogFlowAPIObject").GetComponentInChildren<DialogflowAPIScript>();
                dialogFlowAPIScript.SendSpeechToChatbot(base64string);
                Debug.Log(recordedClip.length);
            }
        }
        else
        {
            buttonTalkToBot.GetComponentInChildren<TextMeshProUGUI>().text = "Can't use microphone";
        }
    }
}