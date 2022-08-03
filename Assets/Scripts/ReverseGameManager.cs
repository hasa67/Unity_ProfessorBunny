using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReverseGameManager : MonoBehaviour {
    public List<QuestionCard> reverseCards = new List<QuestionCard>();
    public GameObject reverseQuestionPrefab;

    private int score;
    private int selectedCards;
    private int remainingRounds;
    private GameManager gameManager;
    private List<ReverseQuestionCard> currentQuestionCards = new List<ReverseQuestionCard>();
    private List<ReverseQuestionCard> currentAnswerCards = new List<ReverseQuestionCard>();
    private GameObject[] questionSlots;
    private GameObject[] answerSlots;


    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.reverseGameManager = this;
    }

    public void StartGame(int roundCount) {
        selectedCards = 0;
        score = 0;
        gameManager.UpdateScoreText(score);
        remainingRounds = roundCount;

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot");
        answerSlots = GameObject.FindGameObjectsWithTag("AnswerSlot");

        if (remainingRounds > 0) {
            NextRound();
        } else {
            gameManager.EndReverseGame();
        }
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore(score);
            gameManager.EndReverseGame();
            return;
        }
        remainingRounds--;

        MyFunctions.ShuffleQuestionCard(reverseCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        foreach (var card in currentAnswerCards) {
            Destroy(card.gameObject);
        }
        currentAnswerCards.Clear();

        for (int i = 0; i < questionSlots.Length; i++) {
            Vector3 position = questionSlots[i].GetComponent<RectTransform>().anchoredPosition;
            GameObject card = Instantiate(reverseQuestionPrefab) as GameObject;
            card.transform.SetParent(questionSlots[i].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentQuestionCards.Add(card.GetComponent<ReverseQuestionCard>());
            card.GetComponent<ReverseQuestionCard>().SetQuestionCard(reverseCards[i], false);
        }

        MyFunctions.ShuffleReverseQuestionCard(currentQuestionCards);
        for (int i = 0; i < answerSlots.Length; i++) {
            Vector3 position = answerSlots[i].GetComponent<RectTransform>().anchoredPosition;
            GameObject card = Instantiate(reverseQuestionPrefab) as GameObject;
            card.transform.SetParent(answerSlots[i].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentAnswerCards.Add(card.GetComponent<ReverseQuestionCard>());
            card.GetComponent<ReverseQuestionCard>().SetQuestionCard(currentQuestionCards[i].GetQuestionCard(), true);
        }

        StartCoroutine(FlipCardsCo());
    }

    IEnumerator FlipCardsCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        yield return new WaitForSeconds(2f);
        foreach (var card in currentQuestionCards) {
            card.GetComponent<ReverseQuestionCard>().FlipCard();
        }
        yield return new WaitForSeconds(1f);

        foreach (var card in currentAnswerCards) {
            card.GetComponent<ReverseQuestionCard>().FlipCard();
        }
        yield return new WaitForSeconds(1f);

        gameManager.IsControllable(true);
        gameManager.Stopwatch(true);
    }

    public bool IsCorrect(string answer) {
        bool output = false;
        foreach (var card in currentAnswerCards) {
            if (answer == card.answer) {
                currentAnswerCards.Remove(card);
                output = true;
            }
        }
        selectedCards++;
        if (selectedCards == currentAnswerCards.Count) {
            StartCoroutine(NextRoundCo());
        }
        return output;
    }

    IEnumerator NextRoundCo() {
        gameManager.IsControllable(false);

        int i = 0;
        foreach (var answerCard in currentAnswerCards) {
            foreach (var questionCard in currentQuestionCards) {

            }
        }
        yield return new WaitForSeconds(1f);
        NextRound();
    }
}
