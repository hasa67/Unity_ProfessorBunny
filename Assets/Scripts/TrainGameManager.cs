using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainGameManager : MonoBehaviour {

    public List<TrainQuestionCard> trainCards = new List<TrainQuestionCard>();
    public GameObject questionPrefab;
    public GameObject train;
    public GameObject questionSlotsPanel;

    private GameManager gameManager;
    private List<DragDrop> currentQuestionCards = new List<DragDrop>();
    private List<TrainAnswerSlot> answerSlots = new List<TrainAnswerSlot>();
    private GameObject[] questionSlots;
    // private bool isRoundFinished;
    // private bool isGameFinished;

    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.trainGameManager = this;
    }

    public void StartGame() {
        // isRoundFinished = false;
        MyFunctions.ShuffleTrainQuestionsList(trainCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot");

        answerSlots = FindObjectsOfType<TrainAnswerSlot>().ToList();
        foreach (var slot in answerSlots) {
            slot.Initialize();
        }
        answerSlots = answerSlots.OrderBy(go => go.name).ToList();

        foreach (var slot in questionSlots) {
            if (trainCards.Count > 0) {
                Vector3 position = slot.GetComponent<RectTransform>().anchoredPosition;
                //GameObject card = Instantiate(questionPrefab, position, Quaternion.identity) as GameObject;
                GameObject card = Instantiate(questionPrefab) as GameObject;
                card.transform.SetParent(slot.transform);
                card.transform.localPosition = Vector3.zero;
                //card.transform.SetParent(slot.transform.parent);
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
        gameManager.IsControllable(false);
        TrainArrive();
        QuestionSlotsIn();
        AnswerSlotsBlink();
        yield return new WaitForSeconds(2.5f);

        for (int i = 0; i < currentQuestionCards.Count; i++) {
            float waitTime = currentQuestionCards[i].GetComponent<AudioSource>().clip.length;
            currentQuestionCards[i].GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(waitTime);
            yield return new WaitForSeconds(0.5f);
        }
        gameManager.IsControllable(true);
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
            StartCoroutine(IsRoundFinishedCo());
        }
        return output;
    }

    IEnumerator IsRoundFinishedCo() {
        gameManager.IsControllable(false);
        // isRoundFinished = true;
        if (trainCards.Count > 0) {
            TrainLeave();
            yield return new WaitForSeconds(2f);
            StartGame();
        } else {
            TrainLeave();
            yield return new WaitForSeconds(2f);
            gameManager.EndTrainGame();
        }
    }

    public void AddScore(int score) {
        gameManager.AddScore(score);
    }

    private void TrainArrive() {
        train.GetComponent<AudioSource>().Play();
        train.GetComponent<Animator>().SetTrigger("arrive");
    }

    private void TrainLeave() {
        train.GetComponent<AudioSource>().Play();
        train.GetComponent<Animator>().SetTrigger("leave");
    }

    private void QuestionSlotsIn() {
        questionSlotsPanel.GetComponent<Animator>().SetTrigger("in");
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
