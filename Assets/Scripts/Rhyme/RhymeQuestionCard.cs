using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RhymeQuestionCard : MonoBehaviour, IPointerDownHandler {

    public Text cardText;
    public string answer;

    private RhymeGameManager rhymeGameManager;
    private Image image;
    private bool isClickable;


    private void Awake() {
        rhymeGameManager = FindObjectOfType<RhymeGameManager>();
        image = GetComponent<Image>();
        isClickable = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (isClickable) {
            image.color = new Color32(190, 190, 190, 255);
            isClickable = false;
            rhymeGameManager.IsCorrect(this);
        }
    }

    public void SetQuestionCard(string inputAnswer, bool isActive) {
        answer = inputAnswer;
        cardText.text = answer;
        isClickable = isActive;
    }

    public void WrongSelect() {
        image.color = new Color32(255, 0, 0, 255);
    }

    public void CorrectSelect() {
        image.color = new Color32(0, 255, 0, 255);
    }
}
