using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PhoneQuestionCard : MonoBehaviour, IPointerDownHandler {

    public AudioClip[] dialClips;
    public string answer;

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private PhoneGameManager phoneGameManager;
    private QuestionCard card;
    private Image image;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        phoneGameManager = FindObjectOfType<PhoneGameManager>();
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        // canvasGroup.alpha = 0.3f;
        // canvasGroup.blocksRaycasts = false;

        // phoneGameManager.SetAnswerSlot(card);
        audioSource.clip = dialClips[Random.Range(0, dialClips.Length)];
        audioSource.Play();
    }

    public void Initialize() {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void SetQuestionCard(QuestionCard inputCard) {
        card = inputCard;
        answer = card.answer;
        GetComponent<Image>().sprite = card.sprite;
        audioSource.clip = card.audioClip;
    }
}
