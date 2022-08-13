using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BellGameManager : MonoBehaviour {
    public List<QuestionCard> bellCards1 = new List<QuestionCard>();
    public List<QuestionCard> bellCards2 = new List<QuestionCard>();
    public List<QuestionCard> bellCards3 = new List<QuestionCard>();
    public GameObject bellQuestionPrefab;
    // public GameObject answerSlotsPanel;
    public GameObject questionSlotsPanel;
    public GameObject bellPanel;

    private int score;
    private int selectedCards;
    private int remainingRounds;
    private int totalRounds;
    private int gameLevel;
    private GameManager gameManager;
    private List<BellQuestionCard> currentQuestionCards = new List<BellQuestionCard>();
    private List<BellQuestionCard> currentAnswerCards = new List<BellQuestionCard>();
    private List<string> gameAnswers = new List<string>();
    private List<string> playerAnswers = new List<string>();
    private List<GameObject> questionSlots = new List<GameObject>();
    private List<GameObject> answerSlots = new List<GameObject>();
    private AudioManager audioManager;
    private List<List<QuestionCard>> bellCards = new List<List<QuestionCard>>();


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

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot").ToList();
        answerSlots = GameObject.FindGameObjectsWithTag("AnswerSlot").ToList();
        foreach (var slot in questionSlots) {
            slot.transform.SetParent(questionSlotsPanel.transform);
        }
        foreach (var slot in answerSlots) {
            // slot.transform.SetParent(answerSlotsPanel.transform);
        }
        if (gameLevel == 1) {
            questionSlots[0].transform.SetParent(questionSlotsPanel.transform.parent);
            questionSlots.RemoveAt(0);

            // answerSlots[0].transform.SetParent(answerSlotsPanel.transform.parent);
            answerSlots.RemoveAt(0);
        }
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore("bell", score, totalRounds, gameLevel);
            gameManager.EndGame();
            return;
        }
        remainingRounds--;
        gameAnswers.Clear();
        playerAnswers.Clear();
        selectedCards = 0;

        MyFunctions.ShuffleQuestionCard(bellCards[gameLevel - 1]);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        foreach (var card in currentAnswerCards) {
            Destroy(card.gameObject);
        }
        currentAnswerCards.Clear();

        for (int i = 0; i < questionSlots.Count; i++) {
            GameObject card = Instantiate(bellQuestionPrefab) as GameObject;
            card.transform.SetParent(questionSlots[i].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentQuestionCards.Add(card.GetComponent<BellQuestionCard>());
            // card.GetComponent<BellQuestionCard>().SetQuestionCard(bellCards[gameLevel - 1][i], false);
        }

        MyFunctions.ShuffleBellQuestionCard(currentQuestionCards);
        for (int i = 0; i < answerSlots.Count; i++) {
            Vector3 position = answerSlots[i].GetComponent<RectTransform>().anchoredPosition;
            GameObject card = Instantiate(bellQuestionPrefab) as GameObject;
            card.transform.SetParent(answerSlots[i].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentAnswerCards.Add(card.GetComponent<BellQuestionCard>());
            // card.GetComponent<BellQuestionCard>().SetQuestionCard(currentQuestionCards[i].GetQuestionCard(), true);
            gameAnswers.Add(card.GetComponent<BellQuestionCard>().answer);
        }

        StartCoroutine(FlipCardsCo());
    }

    IEnumerator FlipCardsCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        float delay = CardsArrive();
        yield return new WaitForSeconds(delay);

        yield return new WaitForSeconds(gameManager.previewWaitTime);
        foreach (var card in currentQuestionCards) {
            // card.GetComponent<BellQuestionCard>().FlipCard();
        }
        yield return new WaitForSeconds(1f);

        foreach (var card in currentAnswerCards) {
            // card.GetComponent<BellQuestionCard>().FlipCard();
        }
        yield return new WaitForSeconds(1f);

        gameManager.IsControllable(true);
        gameManager.Stopwatch(true);
    }

    public void IsCorrect(string answer) {
        playerAnswers.Add(answer);
        selectedCards++;
        if (selectedCards == currentAnswerCards.Count) {
            StartCoroutine(NextRoundCo());
        }
    }

    IEnumerator NextRoundCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        yield return new WaitForSeconds(1f);
        int i = 0;
        foreach (var answer in playerAnswers) {
            if (gameAnswers.Contains(answer)) {
                gameAnswers.Remove(answer);
                i++;
            }
        }
        if (i == answerSlots.Count()) {
            score++;
            gameManager.UpdateScoreText(score, totalRounds);
            audioManager.PlayCorrectClip();
        } else {
            audioManager.PlayWrongClip();
        }
        yield return new WaitForSeconds(1f);

        float delay = CardsLeave();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.roundsWaitTime);

        NextRound();
    }

    private float CardsArrive() {
        // answerSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        float length = questionSlotsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float CardsLeave() {
        // answerSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        float length = questionSlotsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }
}
