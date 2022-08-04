using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReverseQuestionCard : MonoBehaviour, IPointerDownHandler {

    private ReverseGameManager reverseGameManager;
    private QuestionCard card;
    private Image image;
    private Sprite frontSprite;
    private bool isControllable;

    public Sprite backSprite;
    public string answer;


    private void Awake() {
        reverseGameManager = FindObjectOfType<ReverseGameManager>();
        image = GetComponent<Image>();
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

        if (image.sprite == frontSprite) {
            image.sprite = backSprite;
        } else {
            image.sprite = frontSprite;
        }
    }

    public void SetQuestionCard(QuestionCard inputCard, bool isBacked) {
        card = inputCard;
        answer = card.answer;
        frontSprite = card.sprite;

        if (isBacked) {
            GetComponent<Image>().sprite = backSprite;
            isControllable = false;
        } else {
            GetComponent<Image>().sprite = frontSprite;
            isControllable = true;
        }
    }

    public QuestionCard GetQuestionCard() {
        return card;
    }
}
