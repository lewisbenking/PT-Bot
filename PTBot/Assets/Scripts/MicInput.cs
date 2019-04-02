using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]

public class MicInput : MonoBehaviour
{
    //A boolean that flags whether there's a connected microphone  
    private bool micConnected = false;
    //The maximum and minimum available recording frequencies  
    private int minFreq;
    private int maxFreq;
    //A handle to the attached AudioSource  
    private AudioSource goAudioSource;
    //Public variable for saving recorded sound clip
    public AudioClip recordedClip;
    private float[] samples;
    private DialogflowAPIScript dialogFlowAPIScript;
    private Button buttonTalkToBot;
    private InputField inputField;

    //Use this for initialization  
    void Start()
    {
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        //Check if there is at least one microphone connected  
        if (Microphone.devices.Length <= 0)
        {
            Debug.LogWarning("Microphone not connected!");
        }
        else
        {
            micConnected = true;
            //Get the default microphone recording capabilities  
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
            if (minFreq == 0 && maxFreq == 0)
            {
                //...meaning 44100 Hz can be used as the recording sampling rate  
                maxFreq = 44100;
            }
            //Get the attached AudioSource component  
            goAudioSource = this.GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && (String.IsNullOrWhiteSpace(inputField.text)))
        {
            Debug.Log("T key pressed");
            RecordAudio();
        }
        //ToDo
        // for holding the T key down
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
            Debug.Log("Microphone not connected");
        }
    }
}