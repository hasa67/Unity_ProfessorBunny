using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioClip menuBackgroundMusic;
    public AudioClip correctClip;
    public AudioClip wrongClip;
    public AudioClip countClip;
    public AudioClip starChargingClip;
    public AudioClip[] StarClip;
    public AudioClip[] cardDealClip;
    public AudioClip helpClip;
    public AudioClip trainClip;
    public AudioClip sandwichClip;
    public AudioClip reverseClip;
    public AudioClip pairsClip;
    public AudioClip colorsClip;
    public AudioClip wormClip;
    public AudioClip cloudsClip;
    public AudioClip bellClip;
    public AudioClip phoneClip;
    public AudioClip rhymeClip;
    public AudioClip pickupClip;
    public AudioClip dropClip;
    public AudioClip[] leafClips;
    public AudioClip wormBackgrounMusic;

    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private AudioSource musicSource;

    private void Awake() {
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource1.playOnAwake = false;

        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource2.playOnAwake = false;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
    }

    public void PlayMenuBackgrounMusic() {
        musicSource.clip = menuBackgroundMusic;
        musicSource.Play();
    }

    public void PlayWormBackgroundMusic() {
        musicSource.clip = wormBackgrounMusic;
        musicSource.Play();
    }

    public void PlayCorrectClip() {
        audioSource1.clip = correctClip;
        audioSource1.Play();
    }

    public void PlayWrongClip() {
        audioSource1.clip = wrongClip;
        audioSource1.Play();
    }

    public void PlayCountClip() {
        audioSource1.clip = countClip;
        audioSource1.Play();
    }

    public void PlayCardDealSound() {
        audioSource1.clip = cardDealClip[Random.Range(0, cardDealClip.Length)];
        audioSource1.Play();
    }

    public void PlayStarChargingSound() {
        audioSource1.clip = starChargingClip;
        audioSource1.Play();
    }

    public void PlayStarClip(int starNumber) {
        audioSource2.clip = StarClip[starNumber];
        audioSource2.Play();
    }

    public void PlayPickupClip() {
        audioSource1.clip = pickupClip;
        audioSource1.Play();
    }

    public void PlayDropClip() {
        audioSource2.clip = dropClip;
        audioSource2.Play();
    }

    public void PlayLeafClip() {
        audioSource1.clip = leafClips[Random.Range(0, leafClips.Length)];
        audioSource1.Play();
    }

    public void SetGameClip(string gameName) {
        audioSource2.clip = helpClip;
        switch (gameName) {
            case "train":
                audioSource1.clip = trainClip;
                break;
            case "sandwich":
                audioSource1.clip = sandwichClip;
                break;
            case "reverse":
                audioSource1.clip = reverseClip;
                break;
            case "pairs":
                audioSource1.clip = pairsClip;
                break;
            case "colors":
                audioSource1.clip = colorsClip;
                break;
            case "worm":
                audioSource1.clip = wormClip;
                break;
            case "clouds1":
                audioSource1.clip = cloudsClip;
                break;
            case "clouds2":
                audioSource1.clip = cloudsClip;
                break;
            case "bell":
                audioSource1.clip = bellClip;
                break;
            case "phone":
                audioSource1.clip = phoneClip;
                break;
            case "rhyme":
                audioSource1.clip = rhymeClip;
                break;
            default:
                Debug.Log("SetGameClip: gameName not found!");
                break;
        }
    }

    public float PlaySource1() {
        audioSource1.Play();
        return audioSource1.clip.length;
    }

    public float PlaySource2() {
        audioSource2.Play();
        return audioSource1.clip.length;
    }

    public void StopAllSources() {
        audioSource1.Stop();
        audioSource2.Stop();
        musicSource.Stop();
    }

    public void StopSource1() {
        audioSource1.Stop();
    }

    public void StopSource2() {
        audioSource1.Stop();
    }
}
