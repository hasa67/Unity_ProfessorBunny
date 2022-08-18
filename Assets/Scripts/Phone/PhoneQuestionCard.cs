using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PhoneQuestionCard : MonoBehaviour, IPointerDownHandler {

    public AudioClip[] dialClips;
    public Text dialText;
    public Text questionText;
    public string answer;

    private AudioSource audioSource;
    private PhoneGameManager phoneGameManager;
    private QuestionCard card;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        phoneGameManager = FindObjectOfType<PhoneGameManager>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        GetComponent<Animator>().SetBool("isPressed", true);

        audioSource.clip = dialClips[Random.Range(0, dialClips.Length)];
        audioSource.Play();
        phoneGameManager.AddAnswer(card.answer, dialText.text);
    }

    public void Initialize(int i) {
        dialText.text = i.ToString();
        GetComponent<Animator>().SetBool("isPressed", false);
    }

    public void ResetKey() {
        GetComponent<Animator>().SetBool("isPressed", false);
    }

    public void SetQuestionCard(QuestionCard inputCard) {
        card = inputCard;
        answer = card.answer;
        questionText.text = card.answer;
    }
}
