using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainGameManager : MonoBehaviour {

    public List<TrainQuestionCard> trainCards = new List<TrainQuestionCard>();
    public GameObject questionPrefab;
    public GameObject train;

    private GameManager gameManager;
    private List<DragDrop> currentQuestionCards = new List<DragDrop>();
    private List<TrainAnswerSlot> answerSlots = new List<TrainAnswerSlot>();
    private GameObject[] questionSlots;

    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.trainGameManager = this;
    }

    public void StartGame() {
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
                GameObject card = Instantiate(questionPrefab) as GameObject;
                card.transform.SetParent(slot.transform);
                card.transform.localPosition = Vector3.zero;
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

        StartCoroutine(PlaySoundsCo());
    }

    IEnumerator PlaySoundsCo() {
        gameManager.Stopwatch(false);
        gameManager.IsControllable(false);
        float delay = TrainArrive();
        AnswerSlotsBlink();
        yield return new WaitForSeconds(delay + 0.5f);

        for (int i = 0; i < currentQuestionCards.Count; i++) {
            delay = currentQuestionCards[i].GetComponent<AudioSource>().clip.length;
            currentQuestionCards[i].GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(delay + 0.5f);
        }
        gameManager.IsControllable(true);
        gameManager.Stopwatch(true);
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
        if (trainCards.Count > 0) {
            float delay = TrainLeave();
            yield return new WaitForSeconds(delay + 0.5f);
            StartGame();
        } else {
            float delay = TrainLeave();
            yield return new WaitForSeconds(delay + 0.5f);
            gameManager.EndTrainGame();
        }
    }

    public void AddScore(int score) {
        gameManager.AddScore(score);
    }

    private float TrainArrive() {
        train.GetComponent<AudioSource>().Play();
        train.GetComponent<Animator>().SetTrigger("arrive");
        float length = train.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float TrainLeave() {
        gameManager.Stopwatch(false);
        train.GetComponent<AudioSource>().Play();
        train.GetComponent<Animator>().SetTrigger("leave");
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
