using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorsQuestionCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public string answer;
    public ColorsAnswerSlot answerSlot;

    [HideInInspector] public Transform newParent;

    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Transform initialParent;
    private ColorsGameManager colorsGameManager;


    private void Awake() {
        canvas = FindObjectOfType<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        colorsGameManager = FindObjectOfType<ColorsGameManager>();

        answerSlot = null;
    }

    private void Start() {
        initialParent = this.transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        transform.SetParent(transform.root);
        transform.localScale = Vector3.one * 1.3f;

        if (answerSlot != null) {
            answerSlot.isFull = false;
            answerSlot.isCorrect = false;
            answerSlot = null;
        }

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        PlaceBack(); ;
        transform.localScale = Vector3.one;
        canvasGroup.blocksRaycasts = true;
    }

    public void PlaceBack() {
        if (answerSlot == null) {
            this.transform.SetParent(initialParent);
        } else {
            this.transform.SetParent(newParent);
        }
    }

    public void SetQuestionCard(QuestionCard card) {
        answer = card.answer;
        GetComponent<Image>().sprite = card.sprite;
    }
}
