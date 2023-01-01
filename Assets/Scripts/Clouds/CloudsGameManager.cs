using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CloudsGameManager : MonoBehaviour {

    public List<QuestionCard> cloudsCards1 = new List<QuestionCard>();
    public List<QuestionCard> cloudsCards2 = new List<QuestionCard>();
    public List<QuestionCard> cloudsCards3 = new List<QuestionCard>();
    public GameObject cloudsQuestionPrefab;
    public GameObject questionSlotsPanel;
    public GameObject cloudsCover;

    private int score;
    private int remainingRounds;
    private int totalRounds;
    private int gameLevel;
    private int countsLevel;
    private string gameName;
    private GameManager gameManager;
    private List<CloudsQuestionCard> currentQuestionCards = new List<CloudsQuestionCard>();
    private QuestionCard answerCard;
    private List<GameObject> questionSlots = new List<GameObject>();
    private AudioManager audioManager;
    private List<List<QuestionCard>> cloudsCards = new List<List<QuestionCard>>();

    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.cloudsGameManeger = this;
        audioManager = FindObjectOfType<AudioManager>();

        cloudsCards.Add(cloudsCards1);
        cloudsCards.Add(cloudsCards2);
        cloudsCards.Add(cloudsCards3);
    }

    public void StartGame(int roundCount, int level, int questionsCount) {
        gameLevel = level;
        remainingRounds = roundCount;
        totalRounds = roundCount;
        countsLevel = questionsCount;
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
        questionSlots = questionSlots.OrderBy(go => go.name).ToList();
        foreach (var slot in questionSlots) {
            slot.transform.SetParent(questionSlotsPanel.transform);
        }
        if (countsLevel == 1) {
            questionSlots.Last().transform.SetParent(questionSlotsPanel.transform.parent);
            questionSlots.RemoveAt(questionSlots.Count - 1);
            gameName = "clouds1";
        } else {
            gameName = "clouds2";
        }
    }

    public void NextRound() {
        if (remainingRounds <= 0) {
            gameManager.AddScore(gameName, score, totalRounds, gameLevel);
            gameManager.EndGame();
            return;
        }
        remainingRounds--;

        // questionSlots = questionSlots.OrderBy(go => go.name).ToList();
        MyFunctions.ShuffleGameObjects(questionSlots);
        MyFunctions.ShuffleQuestionCard(cloudsCards[gameLevel - 1]);
        answerCard = cloudsCards[gameLevel - 1][questionSlots.Count - 1];

        DestroyCurrectCards();
        InstantiateNewCards(1);
        StartCoroutine(NextRoundCo());
    }

    IEnumerator NextRoundCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);

        float delay = CardsArrive();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.previewWaitTime);
        delay = CloudsCoverIn();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(0.1f);
        ShuffleCards();
        yield return new WaitForSeconds(0.1f);
        delay = CloudsCoverOut();
        yield return new WaitForSeconds(delay);

        gameManager.Stopwatch(true);
        gameManager.IsControllable(true);
    }

    public void IsCorrect(CloudsQuestionCard card) {
        if (card.answer == answerCard.answer) {
            score++;
            gameManager.UpdateScoreText(score, totalRounds);
            audioManager.PlayCorrectClip();
            card.CorrectSelect();
        } else {
            audioManager.PlayWrongClip();
            card.WrongSelect();
            foreach (var item in currentQuestionCards) {
                if (item.answer == answerCard.answer) {
                    item.CorrectSelect();
                }
            }
        }
        StartCoroutine(RoundFinishedCo());
    }

    IEnumerator RoundFinishedCo() {
        gameManager.IsControllable(false);
        gameManager.Stopwatch(false);

        yield return new WaitForSeconds(1f);

        float delay = CardsLeave();
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(gameManager.roundsWaitTime);

        NextRound();
    }

    public void ShuffleCards() {
        DestroyCurrectCards();
        InstantiateNewCards(0);
    }

    public void DestroyCurrectCards() {
        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();
    }

    public void InstantiateNewCards(int j) {
        // j = 1 for initial cards show
        // j = 0 for all cards show
        if (j == 0) {
            MyFunctions.ShuffleGameObjects(questionSlots);
        }
        for (int i = 0; i < questionSlots.Count; i++) {
            GameObject card = Instantiate(cloudsQuestionPrefab) as GameObject;
            card.transform.SetParent(questionSlots[i].transform);
            card.transform.localPosition = new Vector3(0, Random.Range(-20f, 20f), 0);
            card.transform.localScale = Vector3.one;
            currentQuestionCards.Add(card.GetComponent<CloudsQuestionCard>());
            if (i == questionSlots.Count - 1 && j == 1) {
                card.GetComponent<CloudsQuestionCard>().SetEmptyCloud();
            } else {
                card.GetComponent<CloudsQuestionCard>().SetQuestionCard(cloudsCards[gameLevel - 1][i]);
            }
        }
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

    private float CloudsCoverIn() {
        cloudsCover.GetComponent<Animator>().SetBool("on", true);
        float length = cloudsCover.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        cloudsCover.GetComponent<AudioSource>().Play();
        return length;
    }

    private float CloudsCoverOut() {
        cloudsCover.GetComponent<Animator>().SetBool("on", false);
        float length = cloudsCover.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

}
