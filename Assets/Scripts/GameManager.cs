using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {

    public Text scoreText;
    public List<TrainQuestionCard> trainCards = new List<TrainQuestionCard>();
    public GameObject questionPrefab;

    private int score;
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

        List<AnswerSlot> answerSlots = new List<AnswerSlot>(FindObjectsOfType<AnswerSlot>());
        answerSlots = answerSlots.OrderBy(go => go.name).ToList();

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
            answerSlots[i].answer = currentQuestions[i].answer;
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

    public void AddScore(int value) {
        score += value;
        scoreText.text = score.ToString();
    }
}
