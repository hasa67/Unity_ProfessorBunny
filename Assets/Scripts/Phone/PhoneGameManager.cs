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

    private int score;
    private int remainingRounds;
    private int totalRounds;
    private int gameLevel;
    private int questionsCount = 5;
    private AudioSource audioSource;
    private GameManager gameManager;
    private AudioManager audioManager;
    private List<PhoneQuestionCard> dialKeys = new List<PhoneQuestionCard>();
    private List<List<QuestionCard>> phoneCards = new List<List<QuestionCard>>();
    private List<QuestionCard> currentQuestionCards = new List<QuestionCard>();
    public List<string> currentAnswers = new List<string>();
    public List<string> selectedAnswers = new List<string>();
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

        dialKeys = FindObjectsOfType<PhoneQuestionCard>().ToList();
        dialKeys = dialKeys.OrderBy(go => go.name).ToList();

        for (int i = 0; i < dialKeys.Count; i++) {
            dialKeys[i].Initialize(i);
        }

        answerSlots = GameObject.FindGameObjectsWithTag("AnswerSlot").ToList();
        answerSlots = answerSlots.OrderBy(go => go.name).ToList();
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore("phone", score, totalRounds, gameLevel);
            gameManager.EndGame();
            return;
        }
        remainingRounds--;

        ResetPhone();
        currentQuestionCards.Clear();
        currentAnswers.Clear();
        selectedAnswers.Clear();

        MyFunctions.ShuffleQuestionCard(phoneCards[gameLevel - 1]);
        for (int i = 0; i < dialKeys.Count; i++) {
            dialKeys[i].SetQuestionCard(phoneCards[gameLevel - 1][i]);
        }
        for (int i = 0; i < questionsCount; i++) {
            currentQuestionCards.Add(phoneCards[gameLevel - 1][i]);
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
            yield return new WaitForSeconds(delay + 0.5f);
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
            score++;
            gameManager.UpdateScoreText(score, totalRounds);
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
            if (selectedAnswers[i] != currentAnswers[i]) {
                output = false;
                break;
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
