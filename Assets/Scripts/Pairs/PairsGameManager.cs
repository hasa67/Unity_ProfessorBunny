using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PairsGameManager : MonoBehaviour {
    public List<QuestionCard> pairsCards = new List<QuestionCard>();
    public GameObject pairsQuestionPrefab;
    public GameObject questionSlotsPanel;
    public float cardFlipTime;

    private float previewWaitTime = 4f;
    private int score1;
    private int score2;
    private int realScore;
    private int remainingRounds;
    private int totalRounds;
    private int gameLevel;
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
        score1 = 0;
        score2 = 0;
        gameManager.UpdateScoreText(score1, totalRounds);
        realScore = 8;

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot").ToList();
        foreach (var slot in questionSlots) {
            slot.transform.SetParent(questionSlotsPanel.transform);
        }
        int extraQuestionSlots = (3 - gameLevel) * 2;
        if (extraQuestionSlots > 0) {
            for (int i = 0; i < extraQuestionSlots; i++) {
                questionSlots[0].transform.SetParent(questionSlotsPanel.transform.parent);
                questionSlots.RemoveAt(0);
            }
        }
        realScore -= extraQuestionSlots;
        realScore *= totalRounds;
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore("pairs", score1, score2, totalRounds, gameLevel, realScore / 2);
            gameManager.EndGame();
            return;
        }
        remainingRounds--;
        correctAnswers = 0;
        selectedCards.Clear();

        MyFunctions.ShuffleQuestionCard(pairsCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        MyFunctions.ShuffleGameObjects(questionSlots);
        for (int i = 0; i < questionSlots.Count; i++) {
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
        yield return new WaitForSeconds(delay);

        // yield return new WaitForSeconds(gameManager.previewWaitTime / 2);
        yield return new WaitForSeconds(previewWaitTime);
        foreach (var card in currentQuestionCards) {
            card.GetComponent<PairsQuestionCard>().FlipCard(true);
        }
        yield return new WaitForSeconds(cardFlipTime);

        gameManager.IsControllable(true);
        gameManager.Stopwatch(true);
    }

    public void IsCorrect() {
        score1--;
        realScore--;
        gameManager.UpdateScoreText(score1, totalRounds);
        if (selectedCards.Count == 2) {
            StartCoroutine(IsCorrectCo());
        }
    }

    IEnumerator IsCorrectCo() {
        gameManager.IsControllable(false);
        gameManager.Stopwatch(false);

        yield return new WaitForSeconds(cardFlipTime);
        if (selectedCards[0].answer == selectedCards[1].answer) {
            audioManager.PlayCorrectClip();
            correctAnswers++;
        } else {
            audioManager.PlayWrongClip();
            yield return new WaitForSeconds(1f);
            foreach (var card in selectedCards) {
                card.FlipCard(true);
            }
            yield return new WaitForSeconds(cardFlipTime);
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

    public void AddSelectedCard(PairsQuestionCard card) {
        selectedCards.Add(card);
        IsCorrect();
    }
}
