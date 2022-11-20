using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainGameManager : MonoBehaviour {

    public List<QuestionCard> trainCards = new List<QuestionCard>();
    public GameObject trainQuestionPrefab;
    public GameObject train;
    public GameObject questionSlotsPanel;

    private int remainingRounds;
    private int totalRounds;
    private int score;
    private GameManager gameManager;
    private List<QuestionCard> currentTrainCards = new List<QuestionCard>();
    private List<TrainQuestionCard> currentQuestionCards = new List<TrainQuestionCard>();
    private List<TrainAnswerSlot> answerSlots = new List<TrainAnswerSlot>();
    private GameObject[] questionSlots;

    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.trainGameManager = this;
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

        currentTrainCards.Clear();
        foreach (var card in trainCards) {
            currentTrainCards.Add(card);
        }

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot");

        answerSlots = FindObjectsOfType<TrainAnswerSlot>().ToList();
        answerSlots = answerSlots.OrderBy(go => go.name).ToList();
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            AddScore();
            gameManager.EndGame();
            return;
        }
        remainingRounds--;

        MyFunctions.ShuffleQuestionCard(currentTrainCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        foreach (var slot in answerSlots) {
            slot.Initialize();
        }

        if (currentTrainCards.Count < answerSlots.Count) {
            foreach (var card in trainCards) {
                if (!currentTrainCards.Contains(card)) {
                    currentTrainCards.Add(card);
                }
            }
        }

        foreach (var slot in questionSlots) {
            if (currentTrainCards.Count > 0) {
                GameObject card = Instantiate(trainQuestionPrefab) as GameObject;
                card.transform.SetParent(slot.transform);
                card.transform.localPosition = Vector3.zero;
                card.transform.localScale = Vector3.one;

                currentQuestionCards.Add(card.GetComponent<TrainQuestionCard>());
                card.GetComponent<TrainQuestionCard>().SetQuestionCard(currentTrainCards[0]);
                currentTrainCards.RemoveAt(0);
            }
        }

        MyFunctions.ShuffleTrainQuestionCard(currentQuestionCards);

        for (int i = 0; i < currentQuestionCards.Count; i++) {
            answerSlots[i].answer = currentQuestionCards[i].answer;
        }

        StartCoroutine(PlaySoundsCo());
    }

    IEnumerator PlaySoundsCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);
        AnswerSlotsBlink();
        float delay = TrainArrive();
        yield return new WaitForSeconds(delay + 0.5f);

        for (int i = 0; i < currentQuestionCards.Count; i++) {
            delay = currentQuestionCards[i].GetComponent<AudioSource>().clip.length;
            currentQuestionCards[i].GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(delay + 0.5f);
        }
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
            score++;
            gameManager.UpdateScoreText(score, totalRounds);
        }

        if (i == answerSlots.Count) {
            StartCoroutine(IsRoundFinishedCo());
        }
    }

    IEnumerator IsRoundFinishedCo() {
        gameManager.IsControllable(false);
        gameManager.Stopwatch(false);

        float delay = TrainLeave();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.roundsWaitTime);

        NextRound();
    }

    private void AddScore() {
        gameManager.AddScore("train", score, totalRounds, 0);
    }

    private float TrainArrive() {
        train.GetComponent<AudioSource>().Play();
        train.GetComponent<Animator>().SetBool("on", true);
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        float length = train.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float TrainLeave() {
        train.GetComponent<AudioSource>().Play();
        train.GetComponent<Animator>().SetBool("on", false);
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        float length = train.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    public void AnswerSlotsBlink() {
        foreach (var slot in answerSlots) {
            slot.GetComponent<Animator>().SetBool("blink", false);
        }
        for (int i = 0; i < answerSlots.Count; i++) {
            if (!answerSlots[i].isFull) {
                answerSlots[i].GetComponent<Animator>().SetBool("blink", true);
                return;
            }
        }
    }

}
