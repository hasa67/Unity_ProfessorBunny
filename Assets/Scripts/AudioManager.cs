using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioSource audioSource;
    public AudioSource helpAudioSource;

    public AudioClip correctClip;
    public AudioClip wrongClip;
    public AudioClip[] cardDealClip;

    public AudioClip helpClip;
    public AudioClip trainClip;
    public AudioClip sandwichClip;
    public AudioClip reverseClip;


    public void PlayCorrectClip() {
        audioSource.clip = correctClip;
        audioSource.Play();
    }

    public void PlayWrongClip() {
        audioSource.clip = wrongClip;
        audioSource.Play();
    }

    public void PlayCardDealSound() {
        audioSource.clip = cardDealClip[Random.Range(0, cardDealClip.Length)];
        audioSource.Play();
    }

    public void SetGameClip(string gameName) {
        switch (gameName) {
            case "train":
                audioSource.clip = trainClip;
                break;
            case "sandwich":
                audioSource.clip = sandwichClip;
                break;
            case "reverse":
                audioSource.clip = reverseClip;
                break;
            default:
                Debug.Log("SetGameClip: gameName not found!");
                break;
        }
    }

    public float PlayClip() {
        audioSource.Play();
        return audioSource.clip.length;
    }

    public float PlayHelpClip() {
        helpAudioSource.Play();
        return helpClip.length;
    }

    public void StopPlay() {
        audioSource.Stop();
        helpAudioSource.Stop();
    }
}
