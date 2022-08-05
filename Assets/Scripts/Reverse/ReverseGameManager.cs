using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReverseGameManager : MonoBehaviour {
    public List<QuestionCard> reverseCards = new List<QuestionCard>();
    public GameObject reverseQuestionPrefab;
    public GameObject answerSlotsPanel;
    public GameObject questionSlotsPanel;
    public float waitTime = 3f;

    private int score;
    private int selectedCards;
    private int remainingRounds;
    private int totalRounds;
    private int reverseLevel;
    private GameManager gameManager;
    private List<ReverseQuestionCard> currentQuestionCards = new List<ReverseQuestionCard>();
    private List<ReverseQuestionCard> currentAnswerCards = new List<ReverseQuestionCard>();
    private List<string> gameAnswers = new List<string>();
    private List<string> playerAnswers = new List<string>();
    private List<GameObject> questionSlots = new List<GameObject>();
    private List<GameObject> answerSlots = new List<GameObject>();
    private AudioManager audioManager;


    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.reverseGameManager = this;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void StartGame(int roundCount, int level) {
        reverseLevel = level;
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
            slot.transform.SetParent(answerSlotsPanel.transform);
        }
        if (reverseLevel == 1) {
            questionSlots[0].transform.SetParent(questionSlotsPanel.transform.parent);
            questionSlots.RemoveAt(0);

            answerSlots[0].transform.SetParent(answerSlotsPanel.transform.parent);
            answerSlots.RemoveAt(0);
        }
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore("reverse", score, totalRounds, reverseLevel);
            gameManager.EndGame();
            return;
        }
        remainingRounds--;
        gameAnswers.Clear();
        playerAnswers.Clear();
        selectedCards = 0;

        MyFunctions.ShuffleQuestionCard(reverseCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        foreach (var card in currentAnswerCards) {
            Destroy(card.gameObject);
        }
        currentAnswerCards.Clear();

        for (int i = 0; i < questionSlots.Count; i++) {
            Vector3 position = questionSlots[i].GetComponent<RectTransform>().anchoredPosition;
            GameObject card = Instantiate(reverseQuestionPrefab) as GameObject;
            card.transform.SetParent(questionSlots[i].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentQuestionCards.Add(card.GetComponent<ReverseQuestionCard>());
            card.GetComponent<ReverseQuestionCard>().SetQuestionCard(reverseCards[i], false);
        }

        MyFunctions.ShuffleReverseQuestionCard(currentQuestionCards);
        for (int i = 0; i < answerSlots.Count; i++) {
            Vector3 position = answerSlots[i].GetComponent<RectTransform>().anchoredPosition;
            GameObject card = Instantiate(reverseQuestionPrefab) as GameObject;
            card.transform.SetParent(answerSlots[i].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentAnswerCards.Add(card.GetComponent<ReverseQuestionCard>());
            card.GetComponent<ReverseQuestionCard>().SetQuestionCard(currentQuestionCards[i].GetQuestionCard(), true);
            gameAnswers.Add(card.GetComponent<ReverseQuestionCard>().answer);
        }

        StartCoroutine(FlipCardsCo());
    }

    IEnumerator FlipCardsCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);
        float delay = CardsArrive();
        yield return new WaitForSeconds(delay + 0.5f);

        yield return new WaitForSeconds(waitTime);
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
        yield return new WaitForSeconds(delay + 0.5f);

        NextRound();
    }

    private float CardsArrive() {
        answerSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        float length = answerSlotsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float CardsLeave() {
        answerSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        float length = answerSlotsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

}
