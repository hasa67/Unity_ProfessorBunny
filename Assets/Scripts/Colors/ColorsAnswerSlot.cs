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
            if (eventData.pointerDrag != null) {
                isFull = true;
                eventData.pointerDrag.transform.SetParent(this.transform);
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                eventData.pointerDrag.GetComponent<ColorsQuestionCard>().answerSlot = this;

                if (eventData.pointerDrag.GetComponent<ColorsQuestionCard>().answer == answer) {
                    isCorrect = true;
                }

                colorsGameManager.IsRoundFinished();
            }
        } else {
            eventData.pointerDrag.GetComponent<ColorsQuestionCard>().PlaceBack();
        }
    }

    public void Initialize() {
        isFull = false;
        isCorrect = false;
    }
}
