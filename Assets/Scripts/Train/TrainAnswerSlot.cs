using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrainAnswerSlot : MonoBehaviour, IDropHandler {

    public string answer;
    public bool isFull;
    public bool isCorrect;

    private TrainGameManager trainGameManager;

    private void Awake() {
        trainGameManager = FindObjectOfType<TrainGameManager>();
        Initialize();
    }

    public void OnDrop(PointerEventData eventData) {
        if (!isFull) {
            isFull = true;
            eventData.pointerDrag.GetComponent<TrainQuestionCard>().newParent = this.transform;
            // eventData.pointerDrag.transform.SetParent(this.transform);
            // eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.transform.rotation = transform.rotation;
            eventData.pointerDrag.GetComponent<TrainQuestionCard>().answerSlot = this;

            if (eventData.pointerDrag.GetComponent<TrainQuestionCard>().answer == answer) {
                isCorrect = true;
            }

            trainGameManager.IsRoundFinished();
            // if (eventData.pointerDrag != null) {

            // }
        }

        eventData.pointerDrag.GetComponent<TrainQuestionCard>().PlaceBack();
        trainGameManager.AnswerSlotsBlink();
    }

    public void Initialize() {
        isFull = false;
        isCorrect = false;
    }

}
