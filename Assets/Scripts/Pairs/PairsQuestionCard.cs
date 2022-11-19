using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PairsQuestionCard : MonoBehaviour, IPointerDownHandler
{

    public string answer;
    public Image cardImage;
    public Image backImage;

    private PairsGameManager pairsGameManager;
    private QuestionCard card;
    private bool isControllable;
    private float flipTime;

    private void Awake()
    {
        pairsGameManager = FindObjectOfType<PairsGameManager>();
        isControllable = true;
        flipTime = GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;

        if (pairsGameManager.cardFlipTime != flipTime)
        {
            pairsGameManager.cardFlipTime = flipTime;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isControllable)
        {
            FlipCard(false);
            pairsGameManager.AddSelectedCard(this);
        }
    }

    public void FlipCard(bool isFlippable)
    {
        isControllable = isFlippable;
        StartCoroutine(FlipCardCo());
    }

    IEnumerator FlipCardCo()
    {
        GetComponent<Animator>().SetTrigger("flip");
        yield return new WaitForSeconds(flipTime / 2);

        if (backImage.enabled == false)
        {
            backImage.enabled = true;
        }
        else
        {
            backImage.enabled = false;
        }
    }

    public void SetQuestionCard(QuestionCard inputCard)
    {
        card = inputCard;
        answer = card.answer;
        cardImage.sprite = card.sprite;
        isControllable = true;
        backImage.enabled = false;
    }
}
