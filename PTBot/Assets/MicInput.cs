using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

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
    //Use this for initialization  
    void Start()
    {
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

    public void Yeet()
    {
        buttonTalkToBot = GameObject.Find("ButtonSendVoiceToChatbot").GetComponent<Button>();

        if (micConnected)
        {
            if (!Microphone.IsRecording(null))
            {
                buttonTalkToBot.GetComponentInChildren<Text>().text = "Stop Recording";
                goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq);
                recordedClip = goAudioSource.clip;
                samples = new float[goAudioSource.clip.samples];
            }
            else
            {
                buttonTalkToBot.GetComponentInChildren<Text>().text = "Talk to PT-Bot";
                Microphone.End(null); //Stop the audio recording  
                //goAudioSource.Play(); //Playback the recorded audio
                Convert.ToByte(goAudioSource.clip);
                byte[] boi = WavUtility.FromAudioClip(goAudioSource.clip);
                string ree = Convert.ToBase64String(boi);
                Debug.Log(ree);
                dialogFlowAPIScript = GameObject.Find("GameObject2").GetComponentInChildren<DialogflowAPIScript>();
                dialogFlowAPIScript.SendSpeechToChatbot(ree);
                Debug.Log(recordedClip.length);
            }
        }
        else
        {
            buttonTalkToBot.GetComponentInChildren<Text>().text = "Can't use microphone";
            Debug.Log("Microphone not connected");
        }
    }
}