using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler {
    [SerializeField] public Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 initiaPosition;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initiaPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData) {
        //Debug.Log("OnPointerDown");
        canvasGroup.alpha = 0.6f;
        rectTransform.localScale = Vector3.one * 1.3f;
    }

    public void OnPointerUp(PointerEventData eventData) {
        //Debug.Log("OnPointerUp");
        canvasGroup.alpha = 1f;
        rectTransform.localScale = Vector3.one;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        //Debug.Log("OnBeginDrag");
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            if (eventData.pointerEnter.GetComponent<AnswerSlot>() == null) {
                rectTransform.anchoredPosition = initiaPosition;
            }
        } else {
            rectTransform.anchoredPosition = initiaPosition;
        }

        //Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
    }
}


