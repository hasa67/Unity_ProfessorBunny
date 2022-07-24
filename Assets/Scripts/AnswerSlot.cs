using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnswerSlot : MonoBehaviour, IDropHandler {

    private GameManager gameManager;
    private AudioManager audioManager;


    public string answer;
    public bool hasAudio;

    //[HideInInspector]
    public bool isFull;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
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
                        audioManager.PlayCorrectClip();
                    }
                    AddScore(1);
                } else {
                    if (hasAudio) {
                        audioManager.PlayWrongClip();
                        isFull = false;
                        eventData.pointerDrag.GetComponent<DragDrop>().PlaceBack();
                    }
                    AddScore(-1);
                }
            }
        } else {
            eventData.pointerDrag.GetComponent<DragDrop>().PlaceBack();

            audioManager.PlayWrongClip();
        }
    }

    private void AddScore(int score) {
        if (gameManager != null) {
            gameManager.score += score;
        }
    }

}
