using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BellQuestionCard : MonoBehaviour {

    public Text cardText;

    // public Sprite backSprite;
    public string answer;

    private BellGameManager bellGameManager;
    private QuestionCard card;
    // private Image image;
    // private Sprite frontSprite;


    private void Awake() {
        bellGameManager = FindObjectOfType<BellGameManager>();
        // image = GetComponent<Image>();
    }

    // public void FlipCard() {
    //     StartCoroutine(FlipCardCo());
    // }

    // IEnumerator FlipCardCo() {
    //     GetComponent<Animator>().SetTrigger("flip");
    //     yield return new WaitForSeconds(0.5f);

    //     if (image.sprite == frontSprite) {
    //         image.sprite = backSprite;
    //     } else {
    //         image.sprite = frontSprite;
    //     }
    // }

    // public void SetQuestionCard(QuestionCard inputCard, bool isBacked) {
    //     card = inputCard;
    //     answer = card.answer;
    //     frontSprite = card.sprite;

    //     if (isBacked) {
    //         GetComponent<Image>().sprite = backSprite;
    //     } else {
    //         GetComponent<Image>().sprite = frontSprite;
    //     }
    // }

    public void SetQuestionCard(QuestionCard inputCard) {
        card = inputCard;
        answer = card.answer;
        cardText.text = answer;
    }

    public QuestionCard GetQuestionCard() {
        return card;
    }
}
