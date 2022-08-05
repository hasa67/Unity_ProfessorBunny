using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PairsGameManager : MonoBehaviour {
    public List<QuestionCard> pairsCards = new List<QuestionCard>();
    public GameObject pairsQuestionPrefab;
    public GameObject questionSlotsPanel;
    public float waitTime = 3f;

    private int score;
    private int remainingRounds;
    private int totalRounds;
    private int pairsLevel;
    private int correctAnswers;
    private GameManager gameManager;
    private List<PairsQuestionCard> selectedCards = new List<PairsQuestionCard>();
    private List<PairsQuestionCard> currentQuestionCards = new List<PairsQuestionCard>();
    private List<GameObject> questionSlots = new List<GameObject>();
    private AudioManager audioManager;


    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.pairsGameManager = this;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void StartGame(int roundCount, int level) {
        pairsLevel = level;
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
        foreach (var slot in questionSlots) {
            slot.transform.SetParent(questionSlotsPanel.transform);
        }
        int extraQuestionSlots = (3 - pairsLevel) * 2;
        if (extraQuestionSlots > 0) {
            for (int i = 0; i < extraQuestionSlots; i++) {
                questionSlots[0].transform.SetParent(questionSlotsPanel.transform.parent);
                questionSlots.RemoveAt(0);
            }
        }
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore("pairs", score, totalRounds, pairsLevel);
            gameManager.EndGame();
            return;
        }
        remainingRounds--;
        correctAnswers = 0;
        selectedCards.Clear();
        // selectedCards = 0;

        MyFunctions.ShuffleQuestionCard(pairsCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        MyFunctions.ShuffleGameObjects(questionSlots);
        for (int i = 0; i < questionSlots.Count; i++) {
            Vector3 position = questionSlots[i].GetComponent<RectTransform>().anchoredPosition;
            GameObject card = Instantiate(pairsQuestionPrefab) as GameObject;
            card.transform.SetParent(questionSlots[i].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentQuestionCards.Add(card.GetComponent<PairsQuestionCard>());
            card.GetComponent<PairsQuestionCard>().SetQuestionCard(pairsCards[Mathf.CeilToInt(i / 2)]);
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
            card.GetComponent<PairsQuestionCard>().FlipCard(true);
        }
        yield return new WaitForSeconds(1f);

        gameManager.IsControllable(true);
        gameManager.Stopwatch(true);
    }

    public void IsCorrect() {
        score--;
        gameManager.UpdateScoreText(score, totalRounds);
        if (selectedCards.Count == 2) {
            StartCoroutine(IsCorrectCo());
        }
        // playerAnswers.Add(answer);
        // selectedCards++;
        // if (selectedCards == currentAnswerCards.Count) {
        //     StartCoroutine(NextRoundCo());
        // }
    }

    IEnumerator IsCorrectCo() {
        gameManager.IsControllable(false);
        gameManager.Stopwatch(false);

        yield return new WaitForSeconds(1f);
        if (selectedCards[0].answer == selectedCards[1].answer) {
            audioManager.PlayCorrectClip();
            correctAnswers++;
        } else {
            audioManager.PlayWrongClip();
            yield return new WaitForSeconds(1f);
            foreach (var card in selectedCards) {
                card.FlipCard(true);
            }
            yield return new WaitForSeconds(1f);
        }
        selectedCards.Clear();
        gameManager.IsControllable(true);
        gameManager.Stopwatch(true);

        if (correctAnswers * 2 == questionSlots.Count) {
            StartCoroutine(NextRoundCo());
        }
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
        // yield return new WaitForSeconds(1f);

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

    public void AddSelectedCard(PairsQuestionCard card) {
        selectedCards.Add(card);
        IsCorrect();
    }
}
