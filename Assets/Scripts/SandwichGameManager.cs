using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SandwichGameManager : MonoBehaviour {
    public List<TrainQuestionCard> sandwichCards = new List<TrainQuestionCard>();
    public GameObject questionPrefab;
    public GameObject plate;
    public GameObject bread;

    private GameManager gameManager;
    private List<Clickable> currentQuestionCards = new List<Clickable>();
    private List<SandwichAnswerSlot> answerSlots = new List<SandwichAnswerSlot>();
    private GameObject[] questionSlots;

    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.sandwichGameManager = this;
    }

    public void StartGame() {
        MyFunctions.ShuffleTrainQuestionsList(sandwichCards);

        foreach (var card in currentQuestionCards) {
            Destroy(card.gameObject);
        }
        currentQuestionCards.Clear();

        questionSlots = GameObject.FindGameObjectsWithTag("QuestionSlot");

        answerSlots = FindObjectsOfType<SandwichAnswerSlot>().ToList();
        foreach (var slot in answerSlots) {
            slot.Initialize();
        }
        answerSlots = answerSlots.OrderBy(go => go.name).ToList();

        for (int i = 0; i < questionSlots.Length; i++) {
            Vector3 position = questionSlots[i].GetComponent<RectTransform>().anchoredPosition;
            GameObject card = Instantiate(questionPrefab) as GameObject;
            card.transform.SetParent(questionSlots[i].transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;

            currentQuestionCards.Add(card.GetComponent<Clickable>());
            card.GetComponent<Clickable>().SetQuestionCard(sandwichCards[i]);
        }

        MyFunctions.ShuffleClickableList(currentQuestionCards);

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

    public void SetAnswerSlot(TrainQuestionCard card) {
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
                    gameManager.Stopwatch(false);
                    StartCoroutine(RoundFinishedCo());
                }
                return;
            }
        }
    }

    IEnumerator RoundFinishedCo() {
        if (IsCorrect()) {
            gameManager.AddScore(1);
        }

        gameManager.IsControllable(false);
        yield return new WaitForSeconds(1f);
        float delay = PlaceBread();
        yield return new WaitForSeconds(delay + 0.5f);

        delay = PlateLeave();
        yield return new WaitForSeconds(delay + 0.5f);

        bread.GetComponent<Animator>().SetTrigger("remove");
        gameManager.IsControllable(true);
        StartGame();
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
        return output;
    }

    // public bool IsRoundFinished() {
    //     bool output = false;
    //     int i = 0;
    //     foreach (var slot in answerSlots) {
    //         if (slot.isFull == true) {
    //             i++;
    //         }
    //     }
    //     if (i == answerSlots.Count) {
    //         output = true;
    //         StartCoroutine(IsRoundFinishedCo());
    //     }
    //     return output;
    // }

    // IEnumerator IsRoundFinishedCo() {
    //     gameManager.IsControllable(false);
    //     if (sandwichCards.Count > 0) {
    //         float delay = PlateLeave();
    //         yield return new WaitForSeconds(delay + 0.5f);
    //         StartGame();
    //     } else {
    //         float delay = PlateLeave();
    //         yield return new WaitForSeconds(delay + 0.5f);
    //         gameManager.EndTrainGame();
    //     }
    // }

    private float PlaceBread() {
        bread.GetComponent<Animator>().SetTrigger("place");
        float length = plate.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float PlateArrive() {
        plate.GetComponent<AudioSource>().Play();
        plate.GetComponent<Animator>().SetTrigger("arrive");
        float length = plate.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }

    private float PlateLeave() {
        plate.GetComponent<AudioSource>().Play();
        plate.GetComponent<Animator>().SetTrigger("leave");
        float length = plate.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        return length;
    }
}