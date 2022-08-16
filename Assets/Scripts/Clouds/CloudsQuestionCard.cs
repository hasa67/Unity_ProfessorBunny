using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CloudsQuestionCard : MonoBehaviour, IPointerDownHandler {

    public Text cardText;
    public string answer;

    private CloudsGameManager cloudsGameManager;
    private Image image;


    private void Awake() {
        cloudsGameManager = FindObjectOfType<CloudsGameManager>();
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        cloudsGameManager.IsCorrect(this);
    }

    public void SetQuestionCard(QuestionCard inputCard) {
        answer = inputCard.answer;
        cardText.text = answer;
    }

    public void SetEmptyCloud() {
        answer = "";
        cardText.text = "";
        image.color = new Color32(180, 180, 180, 255);
    }

    public void WrongSelect() {
        image.color = new Color32(255, 0, 0, 255);
    }

    public void CorrectSelect() {
        image.color = new Color32(0, 255, 0, 255);
    }
}
