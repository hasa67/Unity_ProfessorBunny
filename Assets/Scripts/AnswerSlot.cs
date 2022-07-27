using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnswerSlot : MonoBehaviour, IDropHandler {

    private GameManager gameManager;

    public string answer;

    //[HideInInspector]
    public bool isFull;
    public bool isCorrect;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        Initialize();
    }

    public void OnDrop(PointerEventData eventData) {
        if (!isFull) {
            if (eventData.pointerDrag != null) {
                isFull = true;
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.GetComponent<DragDrop>().answerSlot = this;

                if (eventData.pointerDrag.GetComponent<DragDrop>().answer == answer) {
                    isCorrect = true;
                }

                if (gameManager.IsCorrect()) {
                    gameManager.AddScore(1);
                }
            }
        } else {
            eventData.pointerDrag.GetComponent<DragDrop>().PlaceBack();
        }
    }

    public void Initialize() {
        isFull = false;
        isCorrect = false;
    }

}
