using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    public int score;
    public List<TrainQuestionCard> trainCards = new List<TrainQuestionCard>();
    public GameObject questionPrefab;

    private PanelManager panelManager;
    private List<DragDrop> currentQuestions = new List<DragDrop>();
    private EventSystem eventSystem;

    private void Awake() {
        panelManager = FindObjectOfType<PanelManager>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    void Start() {
        panelManager.ShowMainPanel();
    }

    public void StratTrainGame() {
        MyFunctions.ShuffleTrainQuestionsList(trainCards);
        currentQuestions.Clear();

        DragDrop[] cards = FindObjectsOfType<DragDrop>();
        QuestionSlot[] questionSlots = FindObjectsOfType<QuestionSlot>();
        AnswerSlot[] answerSlots = FindObjectsOfType<AnswerSlot>();

        foreach (var slot in questionSlots) {
            Vector3 position = slot.GetComponent<RectTransform>().position;
            GameObject card = Instantiate(questionPrefab, position, Quaternion.identity) as GameObject;
            card.transform.SetParent(slot.transform.parent);
            card.transform.localScale = Vector3.one;

            currentQuestions.Add(card.GetComponent<DragDrop>());
            card.GetComponent<DragDrop>().SetQuestionCard(trainCards[0]);
            trainCards.RemoveAt(0);
        }

        MyFunctions.ShuffleDragDropList(currentQuestions);

        for (int i = 0; i < currentQuestions.Count; i++) {
            answerSlots[currentQuestions.Count - 1 - i].answer = currentQuestions[i].answer;
        }

        StartCoroutine(PlayAudiosCo());
    }

    IEnumerator PlayAudiosCo() {
        eventSystem.enabled = false;
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < currentQuestions.Count; i++) {
            float waitTime = currentQuestions[i].GetComponent<AudioSource>().clip.length;
            currentQuestions[i].GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(waitTime);
            yield return new WaitForSeconds(1f);
        }
        eventSystem.enabled = true;
    }
}
