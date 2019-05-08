/*
MIT License

Copyright (c) 2018 Ilana Pecis Bonder, Alice Sun, Lin Zhang

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

^ Some components from their class have been used and modified to suit this project.
*/

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]

public class MicInput : MonoBehaviour
{
    private bool micConnected = false;
    private int minFreq, maxFreq;
    private AudioSource audioSource;
    private float[] samples;
    private DialogflowAPIScript dialogFlowAPIScript;
    private Button buttonTalkToBot;
    private InputField inputField;

    // Validates microphone is connected and its recording capabilities
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
            if (minFreq == 0 && maxFreq == 0) maxFreq = 44100;
            audioSource = this.GetComponent<AudioSource>();
        }
    }

    // If T is pressed and the input field is empty, then record audio
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && (String.IsNullOrWhiteSpace(inputField.text))) RecordAudio();
    }

    // Starts/stops recording then sends to DialogFlow API
    public void RecordAudio()
    {
        buttonTalkToBot = GameObject.Find("ButtonSendVoiceToChatbot").GetComponent<Button>();

        if (micConnected)
        {
            if (!Microphone.IsRecording(null))
            {
                buttonTalkToBot.GetComponentInChildren<TextMeshProUGUI>().text = "Stop Recording";
                audioSource.clip = Microphone.Start(null, true, 20, maxFreq);
                samples = new float[audioSource.clip.samples];
            }
            else
            {
                buttonTalkToBot.GetComponentInChildren<TextMeshProUGUI>().text = "Talk to JimBot";
                Microphone.End(null); //Stop the audio recording  
                Convert.ToByte(audioSource.clip);
                byte[] byteArray = WavUtility.FromAudioClip(audioSource.clip);
                string base64string = Convert.ToBase64String(byteArray);
                dialogFlowAPIScript = GameObject.Find("DialogFlowAPIObject").GetComponentInChildren<DialogflowAPIScript>();
                audioSource.clip = null;
                dialogFlowAPIScript.SendSpeechToChatbot(base64string);
            }
        }
        else
        {
            buttonTalkToBot.GetComponentInChildren<TextMeshProUGUI>().text = "Can't use microphone";
        }
    }
}