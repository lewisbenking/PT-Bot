using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideoScript : MonoBehaviour
{
    private AudioSource audioSource;
    public RawImage image;
    private readonly VideoSource videoSource;
    private VideoPlayer videoPlayer;
    private string url;
    private static Animator animator;
    private GameObject pt;
    private Text chatbotResponse;

    public void PlayVideo(string url)
    {
        Application.runInBackground = true;
        this.url = url;
        chatbotResponse = GameObject.Find("TextChatbotResponse").GetComponent<Text>();
        chatbotResponse.text = "";
        pt = GameObject.Find("PT");
        animator = pt.GetComponent<Animator>();
        animator.SetBool("IsVideoPreparing", false);
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
            Debug.Log("Preparing Video");
            yield return null;
        }

        Debug.Log("Done Preparing Video");        
        animator.SetBool("IsVideoPreparing", true);
        
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
        animator.SetBool("IsVideoPreparing", false);
        image.texture = null;
    }
}