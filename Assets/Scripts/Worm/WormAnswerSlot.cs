using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WormAnswerSlot : MonoBehaviour, IDropHandler {

    public string answer;
    public bool isFull;
    public bool isCorrect;

    private WormGameManager wormGameManager;

    private void Awake() {
        wormGameManager = FindObjectOfType<WormGameManager>();
        Initialize();
    }

    public void OnDrop(PointerEventData eventData) {
        if (!isFull) {
            isFull = true;
            eventData.pointerDrag.GetComponent<WormQuestionCard>().newParent = this.transform;
            eventData.pointerDrag.GetComponent<WormQuestionCard>().answerSlot = this;

            if (eventData.pointerDrag.GetComponent<WormQuestionCard>().answer == answer) {
                isCorrect = true;
            }

            wormGameManager.IsRoundFinished();
        }

        eventData.pointerDrag.GetComponent<WormQuestionCard>().PlaceBack();
    }

    public void Initialize() {
        isFull = false;
        isCorrect = false;
    }
}
