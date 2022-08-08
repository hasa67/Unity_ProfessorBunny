using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PairsQuestionCard : MonoBehaviour, IPointerDownHandler {
    private PairsGameManager pairsGameManager;
    private QuestionCard card;
    private Image image;
    private Sprite frontSprite;
    private bool isControllable;

    public Sprite backSprite;
    public string answer;


    private void Awake() {
        pairsGameManager = FindObjectOfType<PairsGameManager>();
        image = GetComponent<Image>();
        isControllable = true;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (isControllable) {
            FlipCard(false);
            pairsGameManager.AddSelectedCard(this);
        }
    }

    public void FlipCard(bool isFlippable) {
        isControllable = isFlippable;
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

    public void SetQuestionCard(QuestionCard inputCard) {
        card = inputCard;
        answer = card.answer;
        frontSprite = card.sprite;
        GetComponent<Image>().sprite = frontSprite;
        isControllable = true;
    }
}
