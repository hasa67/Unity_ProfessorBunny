using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrainAnswerSlot : MonoBehaviour, IDropHandler {

    private TrainGameManager trainGameManager;

    public string answer;
    public bool isFull;
    public bool isCorrect;

    private void Awake() {
        trainGameManager = FindObjectOfType<TrainGameManager>();
        Initialize();
    }

    public void OnDrop(PointerEventData eventData) {
        if (!isFull) {
            if (eventData.pointerDrag != null) {
                isFull = true;
                eventData.pointerDrag.transform.SetParent(this.transform.parent);
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.GetComponent<RectTransform>().rotation = GetComponent<RectTransform>().rotation;
                eventData.pointerDrag.GetComponent<DragDrop>().answerSlot = this;

                if (eventData.pointerDrag.GetComponent<DragDrop>().answer == answer) {
                    isCorrect = true;
                }

                if (trainGameManager.IsCorrect()) {
                    trainGameManager.AddScore(1);
                }
            }
        } else {
            eventData.pointerDrag.GetComponent<DragDrop>().PlaceBack();
        }

        trainGameManager.AnswerSlotsBlink();
    }

    public void Initialize() {
        isFull = false;
        isCorrect = false;
    }

}