using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BellQuestionCard : MonoBehaviour {

    public Text cardText;

    // public Sprite backSprite;
    // public Sprite frontSprite;
    public string answer;

    private BellGameManager bellGameManager;
    private QuestionCard card;
    private Image image;


    private void Awake() {
        bellGameManager = FindObjectOfType<BellGameManager>();
        image = GetComponent<Image>();
    }

    // public void FlipCard() {
    //     StartCoroutine(FlipCardCo());
    // }

    // IEnumerator FlipCardCo() {
    //     GetComponent<Animator>().SetTrigger("flip");
    //     yield return new WaitForSeconds(0.5f);

    //     if (image.sprite == frontSprite) {
    //         image.sprite = backSprite;
    //         cardText.enabled = false;
    //     } else {
    //         image.sprite = frontSprite;
    //         cardText.enabled = true;
    //     }
    // }

    // public void SetQuestionCard(QuestionCard inputCard, bool isBacked) {
    //     card = inputCard;
    //     answer = card.answer;
    //     cardText.text = answer;

    //     if (isBacked) {
    //         GetComponent<Image>().sprite = backSprite;
    //         cardText.enabled = false;
    //     } else {
    //         GetComponent<Image>().sprite = frontSprite;
    //         cardText.enabled = true;
    //     }
    // }

    public void SetQuestionCard(QuestionCard inputCard) {
        card = inputCard;
        answer = card.answer;
        cardText.text = answer;
    }

    public void SetQuestionCardColor(bool isCorrect) {
        if (isCorrect) {
            image.color = new Color32(0, 255, 0, 255);
        } else {
            image.color = new Color32(255, 0, 0, 255);
        }
    }

    // public QuestionCard GetQuestionCard() {
    //     return card;
    // }
}
