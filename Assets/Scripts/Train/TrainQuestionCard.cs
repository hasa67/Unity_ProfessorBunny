using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrainQuestionCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public string answer;
    public TrainAnswerSlot answerSlot;
    public Image cardImage;

    [HideInInspector] public Transform newParent;

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private Canvas canvas;
    private Transform initialParent;
    private TrainGameManager trainGameManager;
    private AudioManager audioManager;


    private void Awake() {
        canvas = FindObjectOfType<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        trainGameManager = FindObjectOfType<TrainGameManager>();
        audioManager = FindObjectOfType<AudioManager>();

        answerSlot = null;
    }

    private void Start() {
        initialParent = this.transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        audioManager.PlayPickupClip();
        transform.localScale = Vector3.one * 1.3f;

        if (answerSlot != null) {
            answerSlot.isFull = false;
            answerSlot.isCorrect = false;
            answerSlot = null;

            trainGameManager.additionalTouch--;
        }

        trainGameManager.AnswerSlotsBlink();
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        PlaceBack();
        transform.localScale = Vector3.one;
        canvasGroup.blocksRaycasts = true;
        audioManager.PlayDropClip();
    }

    public void PlaceBack() {
        if (answerSlot == null) {
            this.transform.SetParent(initialParent);
        } else {
            this.transform.SetParent(newParent);
        }

        transform.rotation = initialParent.transform.rotation;
    }

    public void SetQuestionCard(QuestionCard card) {
        answer = card.answer;
        cardImage.sprite = card.sprite;
        audioSource.clip = card.audioClip;
    }

}


