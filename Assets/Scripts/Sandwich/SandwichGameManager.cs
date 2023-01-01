using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SandwichGameManager : MonoBehaviour {
    public List<QuestionCard> sandwichCards = new List<QuestionCard>();
    public GameObject sandwichQuestionPrefab;
    public GameObject plate;
    public GameObject topBread;
    public GameObject questionSlotsPanel;

    private int score1;
    private int score2;
    private int remainingRounds;
    private int totalRounds;
    private GameManager gameManager;
    private List<SandwichQuestionCard> currentQuestionCards = new List<SandwichQuestionCard>();
    private List<SandwichAnswerSlot> answerSlots = new List<SandwichAnswerSlot>();
    private GameObject[] questionSlots;

    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.sandwichGameManager = this;
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
        score1 = 0;
        score2 = 0;
        gameManager.UpdateScoreText(score1, totalRounds);

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot");

        answerSlots = FindObjectsOfType<SandwichAnswerSlot>().ToList();
        answerSlots = answerSlots.OrderBy(go => go.name).ToList();
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore("sandwich", score1, score2, totalRounds, 0);
            gameManager.EndGame();
            return;
        }
        BreadOff();
        remainingRounds--;

        MyFunctions.ShuffleQuestionCard(sandwichCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        foreach (var slot in answerSlots) {
            slot.Initialize();
        }

        for (int i = 0; i < questionSlots.Length; i++) {
            GameObject card = Instantiate(sandwichQuestionPrefab) as GameObject;
            card.transform.SetParent(questionSlots[i].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentQuestionCards.Add(card.GetComponent<SandwichQuestionCard>());
            card.GetComponent<SandwichQuestionCard>().SetQuestionCard(sandwichCards[i]);
        }

        MyFunctions.ShuffleSandwichQuestionCard(currentQuestionCards);

        for (int i = 0; i < currentQuestionCards.Count; i++) {
            answerSlots[i].answer = currentQuestionCards[i].answer;
        }

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

    public void SetAnswerSlot(QuestionCard card) {
        for (int i = 0; i < answerSlots.Count; i++) {
            if (!answerSlots[i].isFull) {
                SandwichAnswerSlot slot = answerSlots[i];
                slot.image.sprite = card.sprite;
                slot.image.enabled = true;
                slot.isFull = true;
                slot.GetComponent<Animator>().SetTrigger("add");

                if (slot.answer == card.answer) {
                    slot.isCorrect = true;
                }

                if (i == answerSlots.Count - 1) {
                    StartCoroutine(RoundFinishedCo());
                }
                return;
            }
        }
    }

    IEnumerator RoundFinishedCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        if (IsCorrect()) {
            score1++;
            gameManager.UpdateScoreText(score1, totalRounds);
        }


        yield return new WaitForSeconds(1f);
        float delay = BreadOn();
        yield return new WaitForSeconds(delay + 0.5f);

        delay = PlateLeave();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.roundsWaitTime);

        topBread.GetComponent<Animator>().SetTrigger("remove");
        NextRound();
    }

    public bool IsCorrect() {
        bool output = false;
        int i = 0;
        foreach (var slot in answerSlots) {
            if (slot.isCorrect == true) {
                i++;
            }
        }
        if (i == answerSlots.Count) {
            output = true;
        }
        score2 += i;
        return output;
    }

    private float BreadOn() {
        topBread.GetComponent<Animator>().SetBool("on", true);
        float length = plate.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float BreadOff() {
        topBread.GetComponent<Animator>().SetBool("on", false);
        float length = plate.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float PlateArrive() {
        plate.GetComponent<AudioSource>().Play();
        plate.GetComponent<Animator>().SetBool("on", true);
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", true);
        float length = plate.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float PlateLeave() {
        plate.GetComponent<AudioSource>().Play();
        plate.GetComponent<Animator>().SetBool("on", false);
        questionSlotsPanel.GetComponent<Animator>().SetBool("on", false);
        float length = plate.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }
}
