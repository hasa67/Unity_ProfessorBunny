using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler {

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 initiaPosition;
    private AudioSource audioSource;
    private Canvas canvas;
    private AudioManager am;

    public string answer;

    //[HideInInspector]
    public AnswerSlot answerSlot;

    private void Awake() {
        canvas = FindObjectOfType<Canvas>();
        am = FindObjectOfType<AudioManager>();

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

        initiaPosition = rectTransform.anchoredPosition;

        answerSlot = null;
    }

    public void OnPointerDown(PointerEventData eventData) {
        canvasGroup.alpha = 0.6f;
        rectTransform.localScale = Vector3.one * 1.3f;
        audioSource.Play();

        if (answerSlot != null) {
            answerSlot.isFull = false;
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
                am.PlayWrongClip();
            }
        } else {
            PlaceBack();
            am.PlayWrongClip();
        }

        canvasGroup.blocksRaycasts = true;
    }

    public void PlaceBack() {
        rectTransform.anchoredPosition = initiaPosition;
    }

}


