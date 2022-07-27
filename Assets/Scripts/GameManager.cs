using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {

    public Text scoreText;
    public List<TrainQuestionCard> trainCards = new List<TrainQuestionCard>();
    public GameObject questionPrefab;

    private int score;
    private PanelManager panelManager;
    private List<DragDrop> currentQuestionCards = new List<DragDrop>();
    private List<AnswerSlot> answerSlots = new List<AnswerSlot>();
    private GameObject[] questionSlots;
    private bool isRoundFinished;
    private bool isGameFinished;

    private void Awake() {
        panelManager = FindObjectOfType<PanelManager>();
    }

    void Start() {
        panelManager.ShowMainPanel();
    }

    public void StratTrainGame() {
        isRoundFinished = false;
        MyFunctions.ShuffleTrainQuestionsList(trainCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot");

        answerSlots = FindObjectsOfType<AnswerSlot>().ToList();
        foreach (var slot in answerSlots) {
            slot.Initialize();
        }
        answerSlots = answerSlots.OrderBy(go => go.name).ToList();

        foreach (var slot in questionSlots) {
            if (trainCards.Count > 0) {
                Vector3 position = slot.GetComponent<RectTransform>().position;
                GameObject card = Instantiate(questionPrefab, position, Quaternion.identity) as GameObject;
                card.transform.SetParent(slot.transform.parent);
                card.transform.localScale = Vector3.one;

                currentQuestionCards.Add(card.GetComponent<DragDrop>());
                card.GetComponent<DragDrop>().SetQuestionCard(trainCards[0]);
                trainCards.RemoveAt(0);
            }
        }

        MyFunctions.ShuffleDragDropList(currentQuestionCards);

        for (int i = 0; i < currentQuestionCards.Count; i++) {
            answerSlots[i].answer = currentQuestionCards[i].answer;
        }

        StartCoroutine(PlayAudiosCo());
    }

    IEnumerator PlayAudiosCo() {
        IsControllable(false);
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < currentQuestionCards.Count; i++) {
            float waitTime = currentQuestionCards[i].GetComponent<AudioSource>().clip.length;
            currentQuestionCards[i].GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(waitTime);
            yield return new WaitForSeconds(1f);
        }
        IsControllable(true);
    }

    public void AddScore(int value) {
        score += value;
        scoreText.text = score.ToString();
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
        IsRoundFinished();
        return output;
    }

    public bool IsRoundFinished() {
        bool output = false;
        int i = 0;
        foreach (var slot in answerSlots) {
            if (slot.isFull == true) {
                i++;
            }
        }
        if (i == answerSlots.Count) {
            output = true;
            IsControllable(false);
            isRoundFinished = true;
            if (trainCards.Count > 0) {
                StratTrainGame();
            } else {
                panelManager.HideAllPanels();
            }

        }
        return output;
    }

    public void IsControllable(bool isControllable) {
        panelManager.IsControllable(isControllable);
    }
}
