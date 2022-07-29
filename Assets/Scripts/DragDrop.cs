using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler {

    public string answer;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 initiaPosition;
    private AudioSource audioSource;
    private Canvas canvas;
    private Transform initialParent;

    //[HideInInspector]
    public TrainAnswerSlot answerSlot;


    private void Awake() {
        canvas = FindObjectOfType<Canvas>();
        //gameManager = FindObjectOfType<GameManager>();

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

        answerSlot = null;
    }

    private void Start() {
        initialParent = this.transform.parent;
        initiaPosition = rectTransform.anchoredPosition;
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
            if (eventData.pointerEnter.GetComponent<TrainAnswerSlot>() == null) {
                PlaceBack();
            }
        } else {
            PlaceBack();
        }

        canvasGroup.blocksRaycasts = true;
    }

    public void PlaceBack() {
        this.transform.SetParent(initialParent);
        rectTransform.localPosition = Vector3.zero;
        rectTransform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void SetQuestionCard(TrainQuestionCard card) {
        answer = card.answer;
        GetComponent<Image>().sprite = card.image;
        audioSource.clip = card.audioClip;
    }

}


