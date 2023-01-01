using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WormGameManager : MonoBehaviour {

    public List<QuestionCard> wormCards1 = new List<QuestionCard>();
    public List<QuestionCard> wormCards2 = new List<QuestionCard>();
    public List<QuestionCard> wormCards3 = new List<QuestionCard>();
    public GameObject wormQuestionPrefab;
    public GameObject questionSlotsPanel;
    public GameObject answerSlotsPanel;
    public GameObject questionsCover;

    public int additionalTouch;

    private int remainingRounds;
    private int totalRounds;
    private int gameLevel;
    private int score1;
    private int score2;
    private int maxScore;
    private GameManager gameManager;
    private List<WormQuestionCard> currentQuestionCards = new List<WormQuestionCard>();
    private List<WormAnswerSlot> answerSlots = new List<WormAnswerSlot>();
    private List<GameObject> previewAnswersList = new List<GameObject>();
    private List<GameObject> questionSlots = new List<GameObject>();
    private List<List<QuestionCard>> wormCards = new List<List<QuestionCard>>();


    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.wormGameManager = this;

        wormCards.Add(wormCards1);
        wormCards.Add(wormCards2);
        wormCards.Add(wormCards3);
    }

    public void StartGame(int roundCount, int level) {
        remainingRounds = roundCount;
        totalRounds = roundCount;
        gameLevel = level;
        Initialize();

        if (remainingRounds > 0) {
            NextRound();
        } else {
            gameManager.EndGame();
        }
    }

    private void Initialize() {
        score1 = 0;
        score2 = 0;
        maxScore = 0;
        additionalTouch = 0;
        gameManager.UpdateScoreText(score1, totalRounds);

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot").ToList();
        questionSlots = questionSlots.OrderBy(go => go.name).ToList();

        answerSlots = FindObjectsOfType<WormAnswerSlot>().ToList();
        answerSlots = answerSlots.OrderBy(go => go.name).ToList();
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            AddScore();
            gameManager.EndGame();
            return;
        }
        remainingRounds--;

        MyFunctions.ShuffleQuestionCard(wormCards[gameLevel - 1]);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        foreach (var slot in answerSlots) {
            slot.Initialize();
        }

        MyFunctions.ShuffleQuestionCard(wormCards[gameLevel - 1]);
        int j = 0;
        foreach (var slot in questionSlots) {
            GameObject card = Instantiate(wormQuestionPrefab) as GameObject;
            card.transform.SetParent(slot.transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentQuestionCards.Add(card.GetComponent<WormQuestionCard>());
            card.GetComponent<WormQuestionCard>().SetQuestionCard(wormCards[gameLevel - 1][j]);
            j++;
        }

        MyFunctions.ShuffleWormQuestionCard(currentQuestionCards);

        for (int i = 0; i < currentQuestionCards.Count; i++) {
            answerSlots[i].answer = currentQuestionCards[i].answer;
        }

        StartCoroutine(NextRoundCo());
    }

    IEnumerator NextRoundCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        PreviewAnswersOn();
        float delay = WormArrive();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.previewWaitTime);
        PreviewAnswersOff();

        yield return new WaitForSeconds(delay);
        delay = CoverOff();
        yield return new WaitForSeconds(delay);

        gameManager.IsControllable(true);
        gameManager.Stopwatch(true);
    }

    public void IsRoundFinished() {
        int i = 0;
        int j = 0;
        foreach (var slot in answerSlots) {
            if (slot.isFull == true) {
                i++;
            }
            if (slot.isCorrect == true) {
                j++;
            }
        }

        if (j == answerSlots.Count) {
            score1++;
            gameManager.UpdateScoreText(score1, totalRounds);
        }

        if (i == answerSlots.Count) {
            score2 += j;
            StartCoroutine(IsRoundFinishedCo());
        }
    }

    IEnumerator IsRoundFinishedCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        float delay = WormLeave();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.roundsWaitTime);

        NextRound();
    }

    private void AddScore() {
        maxScore = 5 * totalRounds;
        gameManager.AddScore("worm", score1, totalRounds, score2, maxScore, gameLevel, additionalTouch);
    }

    private float WormArrive() {
        answerSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        questionsCover.GetComponent<Animator>().SetBool("on", true);
        float length = questionSlotsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float CoverOff() {
        questionsCover.GetComponent<Animator>().SetBool("on", false);
        float length = questionsCover.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float WormLeave() {
        answerSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        float length = answerSlotsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private void PreviewAnswersOn() {
        foreach (var slot in answerSlots) {
            for (int i = 0; i < wormCards[gameLevel - 1].Count; i++) {
                if (slot.GetComponent<WormAnswerSlot>().answer == wormCards[gameLevel - 1][i].answer) {
                    GameObject card = Instantiate(wormQuestionPrefab) as GameObject;
                    card.transform.SetParent(slot.transform);
                    card.transform.localPosition = Vector3.zero;
                    card.transform.localScale = Vector3.one;

                    previewAnswersList.Add(card);
                    card.GetComponent<WormQuestionCard>().SetQuestionCard(wormCards[gameLevel - 1][i]);
                    break;
                }
            }
        }
    }

    private void PreviewAnswersOff() {
        foreach (var item in previewAnswersList) {
            Destroy(item);
        }
        previewAnswersList.Clear();
    }

}
