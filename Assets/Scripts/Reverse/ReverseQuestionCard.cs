using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReverseQuestionCard : MonoBehaviour, IPointerDownHandler {

    public string answer;
    public Image cardImage;
    public Image backImage;

    private ReverseGameManager reverseGameManager;
    private QuestionCard card;
    private bool isControllable;


    private void Awake() {
        reverseGameManager = FindObjectOfType<ReverseGameManager>();
        isControllable = true;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (isControllable) {
            FlipCard();
            reverseGameManager.IsCorrect(card.answer);
            isControllable = false;
        }
    }

    public void FlipCard() {
        StartCoroutine(FlipCardCo());
    }

    IEnumerator FlipCardCo() {
        GetComponent<Animator>().SetTrigger("flip");
        yield return new WaitForSeconds(0.5f);

        if (backImage.enabled == false) {
            backImage.enabled = true;
        } else {
            backImage.enabled = false;
        }
    }

    public void SetQuestionCard(QuestionCard inputCard, bool isBacked) {
        card = inputCard;
        answer = card.answer;
        cardImage.sprite = card.sprite;

        if (isBacked) {
            backImage.enabled = true;
            isControllable = false;
        } else {
            backImage.enabled = false;
            isControllable = true;
        }
    }

    public QuestionCard GetQuestionCard() {
        return card;
    }
}
