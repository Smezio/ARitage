using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PortaOro : DefaultTrackableEventHandler
{
    [SerializeField] private GameObject videoPlayer;
    [SerializeField] private GameObject LoadingScreen;

    private bool started;

    /*private bool canWork = false;
    private bool canChange = false;
    private int phase = 0;*/

    private void Awake()
    {
        videoPlayer.GetComponent<VideoPlayer>().prepareCompleted += PlayVideoPlayer;
        started = false;
    }

    private void PlayVideoPlayer(VideoPlayer animPlayer)
    {
        started = true;
        LoadingScreen.SetActive(false);
        videoPlayer.GetComponent<VideoPlayer>().Play();
        Debug.Log("Play");
        /*Debug.Log("CARICO");
        animPlayer.Play();
        canChange = true;*/
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*if (canWork)
        {
            if (animPlayer.clip == null)
            {
                Debug.Log("PRIMO INGRESSO");
                animPlayer.clip = anims[0];
                animPlayer.Prepare();
            }
            else if (animPlayer.clip == anims[phase] && canChange && !animPlayer.isPlaying)
            {
                Debug.Log("SECONDO INGRESSO");
                animPlayer.clip = anims[++phase];
                animPlayer.isLooping = true;
                animPlayer.Prepare();

                canChange = false;
            }
            
            if (canPlay)
            {
                Debug.Log("PLAY");
                animPlayer.Play();
                canPlay = false;
            }
        }*/
        
    }

    override
    protected void OnTrackingFound()
    {
        if (!started)
            videoPlayer.GetComponent<VideoPlayer>().Prepare();
        else
            videoPlayer.GetComponent<VideoPlayer>().Play();
    }

    override
    protected void OnTrackingLost()
    {
        videoPlayer.GetComponent<VideoPlayer>().Pause();
    }
}
