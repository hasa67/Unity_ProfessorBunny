using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text scoreText;
    public Text timerText;
    public TrainGameManager trainGameManager;
    public SandwichGameManager sandwichGameManager;
    public ReverseGameManager reverseGameManager;
    public PairsGameManager pairsGameManager;
    public int trainRounds;
    public int sandwichRounds;
    public int reverseRounds;
    public int pairsRounds;
    public int pairsLevel;

    public List<Score> scoreList = new List<Score>();
    private int score;
    private PanelManager panelManager;
    private bool stopWatch;
    private float timer;

    private void Awake() {
        panelManager = FindObjectOfType<PanelManager>();
    }

    void Start() {
        panelManager.ShowMainPanel();
        Stopwatch(false);
    }

    private void Update() {
        if (stopWatch == true) {
            timer += Time.deltaTime;
        }
        timerText.text = timer.ToString();
    }

    public void StartTrainGame() {
        panelManager.ShowTrainPanel();
        trainGameManager.StartGame(trainRounds);
    }

    public void StartSandwichGame() {
        panelManager.ShowSandwichPanel();
        sandwichGameManager.StartGame(sandwichRounds);
    }

    public void StartReverseGame() {
        panelManager.ShowReversePanel();
        reverseGameManager.StartGame(reverseRounds);
    }

    public void StartPairsGame() {
        panelManager.ShowPairsPanel();
        pairsGameManager.StartGame(pairsRounds, pairsLevel);
    }

    public void EndGame() {
        panelManager.ShowMainPanel();
    }

    public void UpdateScoreText(int score, int totalRounds) {
        scoreText.text = score.ToString() + "/" + totalRounds.ToString();
    }

    public void AddScore(string name, int score, int rounds) {
        Score newScore = new Score();
        newScore.name = name;
        newScore.score = score;
        newScore.rounds = rounds;

        newScore.time = timer;
        scoreList.Add(newScore);

        ResetStopwatch();
    }

    public void IsControllable(bool isControllable) {
        panelManager.IsControllable(isControllable);
    }

    public void ResetStopwatch() {
        timer = 0;
    }

    public void Stopwatch(bool isActive) {
        stopWatch = isActive;
    }
}
