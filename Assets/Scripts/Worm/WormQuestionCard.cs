using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WormQuestionCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public Text cardText;
    public string answer;
    public WormAnswerSlot answerSlot;

    [HideInInspector] public Transform newParent;

    // private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Transform initialParent;
    private WormGameManager wormGameManager;
    private AudioManager audioManager;


    private void Awake() {
        canvas = FindObjectOfType<Canvas>();

        // rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        wormGameManager = FindObjectOfType<WormGameManager>();
        audioManager = FindObjectOfType<AudioManager>();

        answerSlot = null;
    }

    private void Start() {
        initialParent = this.transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        audioManager.PlayPickupClip();
        transform.SetParent(transform.root);
        transform.localScale = Vector3.one * 1.3f;

        if (answerSlot != null) {
            answerSlot.isFull = false;
            answerSlot.isCorrect = false;
            answerSlot = null;

            wormGameManager.additionalTouch--;
        }

        canvasGroup.blocksRaycasts = false;
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
    }

    public void SetQuestionCard(QuestionCard inputCard) {
        answer = inputCard.answer;
        cardText.text = answer;
    }
}
