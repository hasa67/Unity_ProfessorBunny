using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private AudioSource audioSource;

    public AudioClip correctClip;
    public AudioClip wrongClip;

    private void Awake() {
        DontDestroyOnLoad(this);

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
}
