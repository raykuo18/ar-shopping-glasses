using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class playVideo : MonoBehaviour
{
    GameObject startButton, videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        startButton = GameObject.Find("Button_start");
        videoPlayer = GameObject.Find("Video Player");
        startButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startPlay()
    {
        startButton.gameObject.SetActive(false);
        videoPlayer.GetComponent<VideoPlayer>().Play();
        Debug.Log("play!");
    }
}
