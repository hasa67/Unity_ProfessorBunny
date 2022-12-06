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

    // private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private Canvas canvas;
    private Transform initialParent;
    private TrainGameManager trainGameManager;


    private void Awake() {
        canvas = FindObjectOfType<Canvas>();

        // rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        trainGameManager = FindObjectOfType<TrainGameManager>();

        answerSlot = null;
    }

    private void Start() {
        initialParent = this.transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        // canvasGroup.alpha = 0.6f;
        // rectTransform.localScale = Vector3.one * 1.3f;
        transform.localScale = Vector3.one * 1.3f;

        if (answerSlot != null) {
            answerSlot.isFull = false;
            answerSlot.isCorrect = false;
            answerSlot = null;
        }

        trainGameManager.AnswerSlotsBlink();
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData) {
        // rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        // if (eventData.pointerEnter != null) {
        //     if (eventData.pointerEnter.GetComponent<TrainAnswerSlot>() == null) {
        //         PlaceBack();
        //     }
        // } else {
        //     PlaceBack();
        // }

        PlaceBack();
        // canvasGroup.alpha = 1f;
        // rectTransform.localScale = Vector3.one;
        transform.localScale = Vector3.one;
        canvasGroup.blocksRaycasts = true;
    }

    public void PlaceBack() {
        if (answerSlot == null) {
            this.transform.SetParent(initialParent);
        } else {
            this.transform.SetParent(newParent);
        }

        transform.rotation = initialParent.transform.rotation;
        // rectTransform.localPosition = Vector3.zero;
        // rectTransform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void SetQuestionCard(QuestionCard card) {
        answer = card.answer;
        cardImage.sprite = card.sprite;
        audioSource.clip = card.audioClip;
    }

}


