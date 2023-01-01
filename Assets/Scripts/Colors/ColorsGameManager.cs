using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ColorsGameManager : MonoBehaviour {

    public List<QuestionCard> colorsCards = new List<QuestionCard>();
    public GameObject colorsQuestionPrefab;
    public GameObject questionSlotsPanel;
    public GameObject answerSlotsPanel;
    public GameObject questionsCover;

    [HideInInspector] public int additionalTouch;

    private int remainingRounds;
    private int totalRounds;
    private int gameLevel;
    private int score1;
    private int score2;
    private int maxScore;
    private GameManager gameManager;
    private List<ColorsQuestionCard> currentQuestionCards = new List<ColorsQuestionCard>();
    private List<ColorsAnswerSlot> answerSlots = new List<ColorsAnswerSlot>();
    private List<GameObject> previewAnswersList = new List<GameObject>();
    private List<GameObject> questionSlots = new List<GameObject>();

    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.colorsGameManager = this;
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

        answerSlots = FindObjectsOfType<ColorsAnswerSlot>().ToList();
        answerSlots = answerSlots.OrderBy(go => go.name).ToList();

        for (int i = 0; i < questionSlots.Count; i++) {
            questionSlots[i].transform.SetParent(questionSlotsPanel.transform.Find("Image").transform);
            answerSlots[i].transform.SetParent(answerSlotsPanel.transform);
            answerSlots[i].GetComponent<Image>().enabled = true;
            answerSlots[i].GetComponent<Image>().raycastTarget = true;
        }
        int extraSlots = (3 - gameLevel);
        if (extraSlots > 0) {
            for (int i = 0; i < extraSlots; i++) {
                questionSlots[0].transform.SetParent(questionSlotsPanel.transform.parent);
                questionSlots.RemoveAt(0);
                answerSlots[0].transform.SetParent(answerSlotsPanel.transform.parent);
                answerSlots[0].GetComponent<Image>().enabled = false;
                answerSlots[0].GetComponent<Image>().raycastTarget = false;
                answerSlots.RemoveAt(0);
            }
        }
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            AddScore();
            gameManager.EndGame();
            return;
        }
        remainingRounds--;

        MyFunctions.ShuffleQuestionCard(colorsCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        foreach (var slot in answerSlots) {
            slot.Initialize();
        }

        int j = 0;
        foreach (var slot in questionSlots) {
            GameObject card = Instantiate(colorsQuestionPrefab) as GameObject;
            card.transform.SetParent(slot.transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentQuestionCards.Add(card.GetComponent<ColorsQuestionCard>());
            card.GetComponent<ColorsQuestionCard>().SetQuestionCard(colorsCards[j]);
            j++;
        }

        MyFunctions.ShuffleColorsQuestionCard(currentQuestionCards);

        for (int i = 0; i < currentQuestionCards.Count; i++) {
            answerSlots[i].answer = currentQuestionCards[i].answer;
        }

        StartCoroutine(NextRoundCo());
    }

    IEnumerator NextRoundCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        PreviewAnswersOn();
        float delay = PaletteArrive();
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

        float delay = PaletteLeave();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.roundsWaitTime);

        NextRound();
    }

    private void AddScore() {
        maxScore = answerSlots.Count * totalRounds;
        gameManager.AddScore("colors", score1, totalRounds, score2, maxScore, gameLevel, additionalTouch);
    }

    private float PaletteArrive() {
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

    private float PaletteLeave() {
        answerSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        float length = answerSlotsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private void PreviewAnswersOn() {
        foreach (var slot in answerSlots) {
            for (int i = 0; i < colorsCards.Count; i++) {
                if (slot.GetComponent<ColorsAnswerSlot>().answer == colorsCards[i].answer) {
                    GameObject card = Instantiate(colorsQuestionPrefab) as GameObject;
                    card.transform.SetParent(slot.transform);
                    card.transform.localPosition = Vector3.zero;
                    card.transform.localScale = Vector3.one;

                    previewAnswersList.Add(card);
                    card.GetComponent<ColorsQuestionCard>().SetQuestionCard(colorsCards[i]);
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
