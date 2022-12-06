using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorsAnswerSlot : MonoBehaviour, IDropHandler {

    public string answer;
    public bool isFull;
    public bool isCorrect;

    private ColorsGameManager colorsGameManager;


    private void Awake() {
        colorsGameManager = FindObjectOfType<ColorsGameManager>();
        Initialize();
    }

    public void OnDrop(PointerEventData eventData) {
        if (!isFull) {
            isFull = true;
            eventData.pointerDrag.GetComponent<ColorsQuestionCard>().newParent = this.transform;
            eventData.pointerDrag.GetComponent<ColorsQuestionCard>().answerSlot = this;

            if (eventData.pointerDrag.GetComponent<ColorsQuestionCard>().answer == answer) {
                isCorrect = true;
            }

            colorsGameManager.IsRoundFinished();
        }

        eventData.pointerDrag.GetComponent<ColorsQuestionCard>().PlaceBack();
    }

    public void Initialize() {
        isFull = false;
        isCorrect = false;
    }
}
