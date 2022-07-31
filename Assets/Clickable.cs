using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerDownHandler {

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private SandwichGameManager sandwichGameManager;
    private TrainQuestionCard card;
    private Image image;

    public string answer;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        sandwichGameManager = FindObjectOfType<SandwichGameManager>();
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        canvasGroup.alpha = 0.3f;
        canvasGroup.blocksRaycasts = false;

        sandwichGameManager.SetAnswerSlot(card);
    }

    public void Initialize() {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void SetQuestionCard(TrainQuestionCard inputCard) {
        card = inputCard;
        answer = card.answer;
        GetComponent<Image>().sprite = card.sprite;
        audioSource.clip = card.audioClip;
    }
}
