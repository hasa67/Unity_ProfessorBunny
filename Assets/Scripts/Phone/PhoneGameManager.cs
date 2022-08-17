using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PhoneGameManager : MonoBehaviour {

    public List<QuestionCard> phoneCards1 = new List<QuestionCard>();
    public List<QuestionCard> phoneCards2 = new List<QuestionCard>();
    public List<QuestionCard> phoneCards3 = new List<QuestionCard>();

    public GameObject phoneQuestionPrefab;
    public GameObject phone;
    // public GameObject topBread;
    // public GameObject questionSlotsPanel;

    private int score;
    private int remainingRounds;
    private int totalRounds;
    private int gameLevel;
    private GameManager gameManager;
    private List<PhoneQuestionCard> currentQuestionCards = new List<PhoneQuestionCard>();
    private List<List<QuestionCard>> phoneCards = new List<List<QuestionCard>>();
    // private List<SandwichAnswerSlot> answerSlots = new List<SandwichAnswerSlot>();
    private GameObject[] questionSlots;

    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
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

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot");

        // answerSlots = FindObjectsOfType<SandwichAnswerSlot>().ToList();
        // answerSlots = answerSlots.OrderBy(go => go.name).ToList();
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore("phone", score, totalRounds, gameLevel);
            gameManager.EndGame();
            return;
        }
        BreadOff();
        remainingRounds--;

        // MyFunctions.ShuffleQuestionCard(phoneCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        // foreach (var slot in answerSlots) {
        //     slot.Initialize();
        // }

        for (int i = 0; i < questionSlots.Length; i++) {
            GameObject card = Instantiate(phoneQuestionPrefab) as GameObject;
            card.transform.SetParent(questionSlots[i].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            // currentQuestionCards.Add(card.GetComponent<SandwichQuestionCard>());
            // card.GetComponent<SandwichQuestionCard>().SetQuestionCard(phoneCards[i]);
        }

        // MyFunctions.ShuffleSandwichQuestionCard(currentQuestionCards);

        // for (int i = 0; i < currentQuestionCards.Count; i++) {
        //     answerSlots[i].answer = currentQuestionCards[i].answer;
        // }

        StartCoroutine(PlaySoundsCo());
    }

    IEnumerator PlaySoundsCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        float delay = PlateArrive();
        yield return new WaitForSeconds(delay + 0.5f);

        for (int i = 0; i < currentQuestionCards.Count; i++) {
            delay = currentQuestionCards[i].GetComponent<AudioSource>().clip.length;
            currentQuestionCards[i].GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(delay + 0.5f);
        }
        gameManager.IsControllable(true);
        gameManager.Stopwatch(true);
    }

    // public void SetAnswerSlot(QuestionCard card) {
    //     for (int i = 0; i < answerSlots.Count; i++) {
    //         if (!answerSlots[i].isFull) {
    //             SandwichAnswerSlot slot = answerSlots[i];
    //             slot.image.sprite = card.sprite;
    //             slot.image.enabled = true;
    //             slot.isFull = true;
    //             slot.GetComponent<Animator>().SetTrigger("add");

    //             if (slot.answer == card.answer) {
    //                 slot.isCorrect = true;
    //             }

    //             if (i == answerSlots.Count - 1) {
    //                 StartCoroutine(RoundFinishedCo());
    //             }
    //             return;
    //         }
    //     }
    // }

    IEnumerator RoundFinishedCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        if (IsCorrect()) {
            score++;
            gameManager.UpdateScoreText(score, totalRounds);
        }


        yield return new WaitForSeconds(1f);
        float delay = BreadOn();
        yield return new WaitForSeconds(delay + 0.5f);

        delay = PlateLeave();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.roundsWaitTime);

        // topBread.GetComponent<Animator>().SetTrigger("remove");
        NextRound();
    }

    public bool IsCorrect() {
        bool output = false;
        int i = 0;
        // foreach (var slot in answerSlots) {
        //     if (slot.isCorrect == true) {
        //         i++;
        //     }
        // }
        // if (i == answerSlots.Count) {
        //     output = true;
        // }
        return output;
    }

    private float BreadOn() {
        // topBread.GetComponent<Animator>().SetBool("on", true);
        float length = phone.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float BreadOff() {
        // topBread.GetComponent<Animator>().SetBool("on", false);
        float length = phone.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float PlateArrive() {
        phone.GetComponent<AudioSource>().Play();
        phone.GetComponent<Animator>().SetBool("on", true);
        // questionSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        float length = phone.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float PlateLeave() {
        phone.GetComponent<AudioSource>().Play();
        phone.GetComponent<Animator>().SetBool("on", false);
        // questionSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        float length = phone.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }
}
