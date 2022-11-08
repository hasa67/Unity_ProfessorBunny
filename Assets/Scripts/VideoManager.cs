using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour {
    public VideoPlayer videoPlayer;
    public VideoClip trainClip;

    public void SetVideoClip(string gameName) {
        switch (gameName) {
            case "train":
                videoPlayer.clip = trainClip;
                break;
            default:
                Debug.Log("SetVideoClip: gameName not found!");
                break;
        }
    }
}
