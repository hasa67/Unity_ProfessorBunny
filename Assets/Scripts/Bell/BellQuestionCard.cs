using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BellQuestionCard : MonoBehaviour {

    public Text cardText;
    public string answer;

    private Image image;


    private void Awake() {
        image = GetComponent<Image>();
    }

    public void SetQuestionCard(QuestionCard inputCard) {
        answer = inputCard.answer;
        cardText.text = answer;
    }

    public void SetQuestionCardColor(bool isCorrect) {
        if (isCorrect) {
            image.color = new Color32(0, 255, 0, 255);
        } else {
            image.color = new Color32(255, 0, 0, 255);
            cardText.color = new Color32(255, 255, 255, 255);
        }
    }
}
