using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnswerSlot : MonoBehaviour, IDropHandler {

    private GameManager gm;
    private AudioManager am;


    public string answer;
    public bool hasAudio;

    //[HideInInspector]
    public bool isFull;

    private void Awake() {
        gm = FindObjectOfType<GameManager>();
        am = FindObjectOfType<AudioManager>();
        isFull = false;
    }

    public void OnDrop(PointerEventData eventData) {
        if (!isFull) {
            if (eventData.pointerDrag != null) {
                isFull = true;
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.GetComponent<DragDrop>().answerSlot = this;

                if (eventData.pointerDrag.GetComponent<DragDrop>().answer == answer) {
                    if (hasAudio) {
                        am.PlayCorrectClip();
                    }
                    AddScore(1);
                } else {
                    if (hasAudio) {
                        am.PlayWrongClip();
                        isFull = false;
                        eventData.pointerDrag.GetComponent<DragDrop>().PlaceBack();
                    }
                    AddScore(-1);
                }
            }
        } else {
            eventData.pointerDrag.GetComponent<DragDrop>().PlaceBack();

            am.PlayWrongClip();
        }

    }

    private void AddScore(int score) {
        if (gm != null) {
            gm.score += score;
        }
    }

}
