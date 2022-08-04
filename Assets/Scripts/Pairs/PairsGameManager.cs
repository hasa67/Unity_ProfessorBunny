using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PairsGameManager : MonoBehaviour {
    public List<QuestionCard> reverseCards = new List<QuestionCard>();
    public GameObject pairsQuestionPrefab;
    public GameObject questionSlotsPanel;
    public float waitTime = 3f;

    private int score;
    private int selectedCards;
    private int remainingRounds;
    private int totalRounds;
    private int pairsLevel;
    private GameManager gameManager;
    private List<ReverseQuestionCard> currentQuestionCards = new List<ReverseQuestionCard>();
    private List<GameObject> questionSlots = new List<GameObject>();
    private AudioManager audioManager;


    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.pairsGameManager = this;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void StartGame(int roundCount, int level) {
        score = 0;
        pairsLevel = level;
        remainingRounds = roundCount;
        totalRounds = roundCount;
        gameManager.UpdateScoreText(score, totalRounds);

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot").ToList();
        int extraQuestionSlots = (3 - pairsLevel) * 2;
        for (int i = 0; i < extraQuestionSlots; i++) {
            questionSlots[0].SetActive(false);
            questionSlots.RemoveAt(0);
        }

        if (remainingRounds > 0) {
            NextRound();
        } else {
            gameManager.EndGame();
        }
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore("reverse", score, totalRounds);
            gameManager.EndGame();
            return;
        }
        remainingRounds--;
        selectedCards = 0;

        MyFunctions.ShuffleQuestionCard(reverseCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        for (int i = 0; i < questionSlots.Count; i++) {
            Vector3 position = questionSlots[i].GetComponent<RectTransform>().anchoredPosition;
            GameObject card = Instantiate(pairsQuestionPrefab) as GameObject;
            card.transform.SetParent(questionSlots[i].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentQuestionCards.Add(card.GetComponent<ReverseQuestionCard>());
            card.GetComponent<ReverseQuestionCard>().SetQuestionCard(reverseCards[i], false);
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

        gameManager.IsControllable(true);
        gameManager.Stopwatch(true);
    }

    public void IsCorrect(string answer) {
        // playerAnswers.Add(answer);
        // selectedCards++;
        // if (selectedCards == currentAnswerCards.Count) {
        //     StartCoroutine(NextRoundCo());
        // }
    }

    IEnumerator NextRoundCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        yield return new WaitForSeconds(1f);
        // int i = 0;
        // foreach (var answer in playerAnswers) {
        //     if (gameAnswers.Contains(answer)) {
        //         gameAnswers.Remove(answer);
        //         i++;
        //     }
        // }
        // if (i == answerSlots.Count()) {
        //     score++;
        //     gameManager.UpdateScoreText(score, totalRounds);
        //     audioManager.PlayCorrectClip();
        // } else {
        //     audioManager.PlayWrongClip();
        // }
        yield return new WaitForSeconds(1f);

        float delay = CardsLeave();
        yield return new WaitForSeconds(delay + 0.5f);

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
}
