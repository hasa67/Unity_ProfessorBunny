using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BellGameManager : MonoBehaviour {
    public List<QuestionCard> bellCards1 = new List<QuestionCard>();
    public List<QuestionCard> bellCards2 = new List<QuestionCard>();
    public List<QuestionCard> bellCards3 = new List<QuestionCard>();
    public GameObject bellQuestionPrefab;
    public GameObject questionSlotsPanel;
    public GameObject bell;

    public int score;
    public int remainingRounds;
    public int totalRounds;
    public int gameLevel;
    public int questionCardsCount = 5;
    public bool isCorrect;
    public bool bellRing;
    public GameManager gameManager;
    public List<QuestionCard> currentBellCards = new List<QuestionCard>();
    public List<BellQuestionCard> currentQuestionCards = new List<BellQuestionCard>();
    public BellQuestionCard answerCard;
    public GameObject questionSlot;
    public GameObject answerSlot;
    public AudioManager audioManager;
    public List<List<QuestionCard>> bellCards = new List<List<QuestionCard>>();


    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.bellGameManager = this;
        audioManager = FindObjectOfType<AudioManager>();

        bellCards.Add(bellCards1);
        bellCards.Add(bellCards2);
        bellCards.Add(bellCards3);
    }

    public void StartGame(int roundCount, int level) {
        gameLevel = level;
        remainingRounds = roundCount;
        totalRounds = roundCount;
        Initialize();

        if (remainingRounds > 0) {
            NextRound();
        } else {
            gameManager.EndGame();
        }
    }

    private void Initialize() {
        score = 0;
        gameManager.UpdateScoreText(score, totalRounds);

        questionSlot = GameObject.FindGameObjectWithTag("QuestionSlot");
        answerSlot = GameObject.FindGameObjectWithTag("AnswerSlot");
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore("bell", score, totalRounds, gameLevel);
            gameManager.EndGame();
            return;
        }
        remainingRounds--;
        isCorrect = false;
        bellRing = false;

        MyFunctions.ShuffleQuestionCard(bellCards[gameLevel - 1]);
        currentBellCards.Clear();
        for (int i = 0; i < questionCardsCount; i++) {
            currentBellCards.Add(bellCards[gameLevel - 1][i]);
        }

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();
        if (answerCard != null) {
            Destroy(answerCard.gameObject);
        }

        StartCoroutine(ShowCardsCo());
    }

    IEnumerator ShowCardsCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        float delay = CardsArrive();
        yield return new WaitForSeconds(delay + 1f);

        InstantiateAnswerCard();
        yield return new WaitForSeconds(gameManager.previewWaitTime / 2);

        MyFunctions.ShuffleQuestionCard(currentBellCards);
        for (int i = 0; i < currentBellCards.Count; i++) {
            gameManager.Stopwatch(false);
            gameManager.IsControllable(false);

            InstantiateQuestionCard(i);
            yield return new WaitForSeconds(1f);

            gameManager.Stopwatch(true);
            gameManager.IsControllable(true);
            yield return new WaitForSeconds(gameManager.previewWaitTime / 2);

            gameManager.Stopwatch(false);
            gameManager.IsControllable(false);
            if (isCorrect) {
                break;
            }
        }

        IsCorrect();
    }

    public void IsCorrect() {
        if (isCorrect && bellRing) {
            score++;
            gameManager.UpdateScoreText(score, totalRounds);
            audioManager.PlayCorrectClip();
            StartCoroutine(NextRoundCo());
        } else {
            audioManager.PlayWrongClip();
            StartCoroutine(NextRoundCo());
        }
    }

    IEnumerator NextRoundCo() {
        yield return new WaitForSeconds(1f);

        float delay = CardsLeave();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.roundsWaitTime);

        NextRound();
    }

    private float CardsArrive() {
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        float length = questionSlotsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float CardsLeave() {
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        float length = questionSlotsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private void InstantiateAnswerCard() {
        Vector3 position = answerSlot.GetComponent<RectTransform>().anchoredPosition;
        GameObject card = Instantiate(bellQuestionPrefab) as GameObject;
        card.transform.SetParent(answerSlot.transform);
        card.transform.localPosition = Vector3.zero;
        card.transform.localScale = Vector3.one;

        answerCard = card.GetComponent<BellQuestionCard>();
        answerCard.SetQuestionCard(currentBellCards[0]);
        audioManager.PlayCardDealSound();
    }

    private void InstantiateQuestionCard(int i) {
        GameObject card = Instantiate(bellQuestionPrefab) as GameObject;
        card.transform.SetParent(questionSlot.transform);
        card.transform.localPosition = Vector3.zero;
        card.transform.localScale = Vector3.one;

        currentQuestionCards.Add(card.GetComponent<BellQuestionCard>());
        card.GetComponent<BellQuestionCard>().SetQuestionCard(currentBellCards[i]);
        audioManager.PlayCardDealSound();

        if (currentQuestionCards.Last().answer == answerCard.answer) {
            isCorrect = true;
        }
    }

    public void RingBell() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);
        StopAllCoroutines();

        bell.GetComponent<Animator>().SetTrigger("ring");
        bell.GetComponent<AudioSource>().Play();
        bellRing = true;
        IsCorrect();
    }
}
