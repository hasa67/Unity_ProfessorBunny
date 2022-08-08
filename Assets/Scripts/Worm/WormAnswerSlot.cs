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
            if (eventData.pointerDrag != null) {
                isFull = true;
                eventData.pointerDrag.transform.SetParent(this.transform);
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                eventData.pointerDrag.GetComponent<WormQuestionCard>().answerSlot = this;

                if (eventData.pointerDrag.GetComponent<WormQuestionCard>().answer == answer) {
                    isCorrect = true;
                }

                wormGameManager.IsRoundFinished();
            }
        } else {
            eventData.pointerDrag.GetComponent<WormQuestionCard>().PlaceBack();
        }
    }

    public void Initialize() {
        isFull = false;
        isCorrect = false;
    }
}
