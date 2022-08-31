using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RhymeGameManager : MonoBehaviour {

    public List<QuestionCard> rhymeCards = new List<QuestionCard>();
    public GameObject rhymeQuestionPrefab;
    public GameObject questionSlotsPanel;
    public GameObject answerSlotsPanel;

    private int score;
    private int remainingRounds;
    private int totalRounds;
    private GameManager gameManager;
    private List<RhymeQuestionCard> currentQuestionCards = new List<RhymeQuestionCard>();
    private List<RhymeQuestionCard> currentAnswerCard = new List<RhymeQuestionCard>();
    private List<string> currentQuestions = new List<string>();
    private List<GameObject> questionSlots = new List<GameObject>();
    private GameObject answerSlot;
    private AudioManager audioManager;

    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.rhymeGameManager = this;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void StartGame(int roundCount) {
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

        MyFunctions.ShuffleQuestionCard(rhymeCards);

        answerSlot = GameObject.FindGameObjectWithTag("AnswerSlot");

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot").ToList();
        questionSlots = questionSlots.OrderBy(go => go.name).ToList();
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore("rhyme", score, totalRounds, 0);
            gameManager.EndGame();
            return;
        }
        remainingRounds--;

        currentQuestions = rhymeCards[remainingRounds].answer.Split('.').ToList();

        MyFunctions.ShuffleGameObjects(questionSlots);



        DestroyCurrectCards();
        InstantiateNewCards();
        StartCoroutine(NextRoundCo());
    }

    IEnumerator NextRoundCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        float delay = CardsArrive();
        yield return new WaitForSeconds(delay);

        gameManager.Stopwatch(true);
        gameManager.IsControllable(true);
    }

    public void IsCorrect(RhymeQuestionCard card) {
        if (card.answer == currentQuestions[1]) {
            score++;
            gameManager.UpdateScoreText(score, totalRounds);
            audioManager.PlayCorrectClip();
            card.CorrectSelect();
        } else {
            audioManager.PlayWrongClip();
            card.WrongSelect();
            foreach (var item in currentQuestionCards) {
                if (item.answer == currentQuestions[1]) {
                    item.CorrectSelect();
                }
            }
        }
        StartCoroutine(RoundFinishedCo());
    }

    IEnumerator RoundFinishedCo() {
        gameManager.IsControllable(false);
        gameManager.Stopwatch(false);

        yield return new WaitForSeconds(1f);

        float delay = CardsLeave();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.roundsWaitTime);

        NextRound();
    }

    public void DestroyCurrectCards() {
        foreach (var card in currentAnswerCard) {
            Destroy(card.gameObject);
        }
        currentAnswerCard.Clear();

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();
    }

    public void InstantiateNewCards() {
        GameObject card = Instantiate(rhymeQuestionPrefab) as GameObject;
        card.transform.SetParent(answerSlot.transform);
        card.transform.localPosition = Vector3.zero;
        card.transform.localScale = Vector3.one;
        currentAnswerCard.Add(card.GetComponent<RhymeQuestionCard>());
        currentAnswerCard[0].SetQuestionCard(currentQuestions[0], false);

        MyFunctions.ShuffleGameObjects(questionSlots);
        for (int i = 0; i < questionSlots.Count; i++) {
            GameObject card1 = Instantiate(rhymeQuestionPrefab) as GameObject;
            card1.transform.SetParent(questionSlots[i].transform);
            card1.transform.localPosition = Vector3.zero;
            card1.transform.localScale = Vector3.one;
            currentQuestionCards.Add(card1.GetComponent<RhymeQuestionCard>());
            card1.GetComponent<RhymeQuestionCard>().SetQuestionCard(currentQuestions[i + 1], true);
        }
    }

    private float CardsArrive() {
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        answerSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        float length = questionSlotsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float CardsLeave() {
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        answerSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        float length = questionSlotsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }
}
