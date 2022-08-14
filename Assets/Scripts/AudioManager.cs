using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private AudioSource audioSource;

    public AudioClip correctClip;
    public AudioClip wrongClip;
    public AudioClip[] cardDealClip;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

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
}
