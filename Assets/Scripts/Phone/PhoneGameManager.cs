using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PhoneGameManager : MonoBehaviour {

    public List<QuestionCard> phoneCards1 = new List<QuestionCard>();
    public List<QuestionCard> phoneCards2 = new List<QuestionCard>();
    public List<QuestionCard> phoneCards3 = new List<QuestionCard>();
    public GameObject phoneQuestionPrefab;
    public GameObject phone;
    public Image numbersPanel;
    public GameObject trashBin;

    private int score1;
    private int score2;
    private int maxScore;
    private int remainingRounds;
    private int totalRounds;
    private int gameLevel;
    private int questionsCount;
    private AudioSource audioSource;
    private GameManager gameManager;
    private AudioManager audioManager;
    private List<PhoneQuestionCard> dialKeys = new List<PhoneQuestionCard>();
    private List<List<QuestionCard>> phoneCards = new List<List<QuestionCard>>();
    private List<QuestionCard> currentDialCards = new List<QuestionCard>();
    private List<QuestionCard> currentQuestionCards = new List<QuestionCard>();
    private List<string> currentAnswers = new List<string>();
    private List<string> selectedAnswers = new List<string>();
    private List<GameObject> answerSlots = new List<GameObject>();

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        gameManager.phoneGameManager = this;

        phoneCards.Add(phoneCards1);
        phoneCards.Add(phoneCards2);
        phoneCards.Add(phoneCards3);
    }

    public void StartGame(int roundCount, int level) {
        remainingRounds = roundCount;
        totalRounds = roundCount;
        gameLevel = level;
        questionsCount = gameLevel + 2;
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
        gameManager.UpdateScoreText(score1, totalRounds);

        dialKeys = FindObjectsOfType<PhoneQuestionCard>().ToList();
        dialKeys = dialKeys.OrderBy(go => go.name).ToList();
        for (int i = 0; i < dialKeys.Count; i++) {
            dialKeys[i].Initialize(i);
        }

        answerSlots = GameObject.FindGameObjectsWithTag("AnswerSlot").ToList();
        answerSlots = answerSlots.OrderBy(go => go.name).ToList();
        foreach (var slot in answerSlots) {
            slot.transform.SetParent(numbersPanel.transform);
        }
        if (gameLevel <= 2) {
            answerSlots[answerSlots.Count - 1].transform.SetParent(trashBin.transform);
            answerSlots.RemoveAt(answerSlots.Count - 1);
            if (gameLevel <= 1) {
                answerSlots[answerSlots.Count - 1].transform.SetParent(trashBin.transform);
                answerSlots.RemoveAt(answerSlots.Count - 1);
            }
        }
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            maxScore = (gameLevel + 2) * totalRounds;
            gameManager.AddScore("phone", score1, totalRounds, score2, maxScore, gameLevel);
            gameManager.EndGame();
            return;
        }
        remainingRounds--;

        ResetPhone();
        currentDialCards.Clear();
        currentQuestionCards.Clear();
        currentAnswers.Clear();
        selectedAnswers.Clear();

        MyFunctions.ShuffleQuestionCard(phoneCards[gameLevel - 1]);
        for (int i = 0; i < dialKeys.Count; i++) {
            dialKeys[i].SetQuestionCard(phoneCards[gameLevel - 1][i]);
            currentDialCards.Add(phoneCards[gameLevel - 1][i]);
        }

        MyFunctions.ShuffleQuestionCard(currentDialCards);
        for (int i = 0; i < questionsCount; i++) {
            currentQuestionCards.Add(currentDialCards[i]);
        }

        StartCoroutine(PlaySoundsCo());
    }

    IEnumerator PlaySoundsCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        float delay = PhoneArrive();
        yield return new WaitForSeconds(delay + 0.5f);

        MyFunctions.ShuffleQuestionCard(currentQuestionCards);
        for (int i = 0; i < currentQuestionCards.Count; i++) {
            audioSource.clip = currentQuestionCards[i].audioClip;
            audioSource.Play();
            currentAnswers.Add(currentQuestionCards[i].answer);

            delay = audioSource.clip.length;
            yield return new WaitForSeconds(delay + 1f);
        }
        gameManager.IsControllable(true);
        gameManager.Stopwatch(true);
    }

    public void AddAnswer(string answer, string dialNumber) {
        selectedAnswers.Add(answer);
        answerSlots[selectedAnswers.Count - 1].GetComponentInChildren<Text>().text = dialNumber;
        IsRoundFinished();
    }

    private void IsRoundFinished() {
        if (selectedAnswers.Count == currentAnswers.Count) {
            StartCoroutine(RoundFinishedCo());
        }
    }

    IEnumerator RoundFinishedCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);
        yield return new WaitForSeconds(1f);

        if (IsCorrect()) {
            score1++;
            gameManager.UpdateScoreText(score1, totalRounds);
            audioManager.PlayCorrectClip();
            numbersPanel.color = new Color32(0, 255, 0, 200);
        } else {
            audioManager.PlayWrongClip();
            numbersPanel.color = new Color32(255, 0, 0, 200);
        }

        yield return new WaitForSeconds(1f);

        float delay = PhoneLeave();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.roundsWaitTime);

        NextRound();
    }

    private bool IsCorrect() {
        bool output = true;
        for (int i = 0; i < selectedAnswers.Count; i++) {
            if (selectedAnswers[i] == currentAnswers[i]) {
                score2++;
            }
            if (selectedAnswers[i] != currentAnswers[i]) {
                output = false;
                // break;
            }
        }
        return output;
    }

    private float PhoneArrive() {
        phone.GetComponent<Animator>().SetBool("on", true);
        float length = phone.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float PhoneLeave() {
        phone.GetComponent<Animator>().SetBool("on", false);
        float length = phone.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private void ResetPhone() {
        foreach (var slot in answerSlots) {
            slot.GetComponentInChildren<Text>().text = "-";
        }
        foreach (var key in dialKeys) {
            key.ResetKey();
        }
        numbersPanel.color = new Color32(0, 0, 0, 60);
    }
}
