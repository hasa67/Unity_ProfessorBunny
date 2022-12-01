using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour {
    public VideoPlayer videoPlayer;
    public VideoClip idleClip;
    public VideoClip trainClip;
    public VideoClip sandwichClip;
    public VideoClip reverseClip;
    public VideoClip pairsClip;
    public VideoClip colorsClip;
    public VideoClip wormClip;
    public VideoClip cloudsClip;
    public VideoClip bellClip;
    public VideoClip phoneClip;
    public VideoClip rhymeClip;

    public void SetVideoClip(string gameName) {
        switch (gameName) {
            case "train":
                videoPlayer.clip = trainClip;
                break;
            case "sandwich":
                videoPlayer.clip = sandwichClip;
                break;
            case "reverse":
                videoPlayer.clip = reverseClip;
                break;
            case "pairs":
                videoPlayer.clip = pairsClip;
                break;
            case "colors":
                videoPlayer.clip = colorsClip;
                break;
            case "worm":
                videoPlayer.clip = wormClip;
                break;
            case "clouds1":
                videoPlayer.clip = cloudsClip;
                break;
            case "clouds2":
                videoPlayer.clip = cloudsClip;
                break;
            case "bell":
                videoPlayer.clip = bellClip;
                break;
            case "phone":
                videoPlayer.clip = phoneClip;
                break;
            case "rhyme":
                videoPlayer.clip = rhymeClip;
                break;
            default:
                Debug.Log("SetVideoClip: gameName not found!");
                break;
        }
    }

    public void ClearVideoClip() {
        videoPlayer.clip = idleClip;
    }
}
