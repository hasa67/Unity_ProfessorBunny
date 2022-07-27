using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler {

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 initiaPosition;
    private AudioSource audioSource;
    private Canvas canvas;
    //private GameManager gameManager;

    public string answer;

    [HideInInspector]
    public AnswerSlot answerSlot;
    public TrainQuestionCard trainQuestionCard;

    private void Awake() {
        canvas = FindObjectOfType<Canvas>();
        //gameManager = FindObjectOfType<GameManager>();

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

        initiaPosition = rectTransform.anchoredPosition;

        answerSlot = null;
    }

    public void OnPointerDown(PointerEventData eventData) {
        canvasGroup.alpha = 0.6f;
        rectTransform.localScale = Vector3.one * 1.3f;

        if (answerSlot != null) {
            answerSlot.isFull = false;
            if (answerSlot.isCorrect) {
                // gameManager.AddScore(-1);
                answerSlot.isCorrect = false;
            }
            answerSlot = null;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        canvasGroup.alpha = 1f;
        rectTransform.localScale = Vector3.one;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            if (eventData.pointerEnter.GetComponent<AnswerSlot>() == null) {
                PlaceBack();
            }
        } else {
            PlaceBack();
        }

        canvasGroup.blocksRaycasts = true;
    }

    public void PlaceBack() {
        rectTransform.position = initiaPosition;
    }

    public void SetQuestionCard(TrainQuestionCard card) {
        answer = card.answer;
        GetComponent<Image>().sprite = card.image;
        audioSource.clip = card.audioClip;
    }

}


