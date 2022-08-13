using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorsQuestionCard : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler {

    public string answer;
    public ColorsAnswerSlot answerSlot;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Transform initialParent;
    private ColorsGameManager colorsGameManager;


    private void Awake() {
        canvas = FindObjectOfType<Canvas>();

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        colorsGameManager = FindObjectOfType<ColorsGameManager>();

        answerSlot = null;
    }

    private void Start() {
        initialParent = this.transform.parent;
    }

    public void OnPointerDown(PointerEventData eventData) {
        canvasGroup.alpha = 0.6f;
        rectTransform.localScale = Vector3.one * 1.3f;

        if (answerSlot != null) {
            answerSlot.isFull = false;
            if (answerSlot.isCorrect) {
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
            if (eventData.pointerEnter.GetComponent<ColorsAnswerSlot>() == null) {
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

    public void SetQuestionCard(QuestionCard card) {
        answer = card.answer;
        GetComponent<Image>().sprite = card.sprite;
    }
}