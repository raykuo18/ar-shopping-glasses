using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
 
public class OnlineVideoLoader : MonoBehaviour
{
     
    public VideoPlayer videoPlayer;
    public string videoUrl = "yourvideourl";

    // Start is called before the first frame update
    public void Start()
    {
        
 
    }
 
    // Update is called once per frame
    void Update()
    {
         
    }

    public void Setting(){
        videoPlayer.url = videoUrl;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.Prepare();
        Debug.Log("video prepared");
    }
}