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
    public ColorsGameManager colorsGameManager;
    public float roundsWaitTime;
    public float previewWaitTime;

    [Range(1, 5)] public int trainRounds;
    [Range(1, 5)] public int sandwichRounds;
    [Range(1, 5)] public int reverseRounds;
    [Range(1, 2)] public int reverseLevel;
    [Range(1, 5)] public int pairsRounds;
    [Range(1, 3)] public int pairsLevel;
    [Range(1, 5)] public int colorsRounds;
    [Range(1, 3)] public int colorsLevel;

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
        reverseGameManager.StartGame(reverseRounds, reverseLevel);
    }

    public void StartPairsGame() {
        panelManager.ShowPairsPanel();
        pairsGameManager.StartGame(pairsRounds, pairsLevel);
    }

    public void StratColorsGame() {
        panelManager.ShowColorsPanel();
        colorsGameManager.StartGame(colorsRounds, colorsLevel);
    }

    public void EndGame() {
        panelManager.ShowMainPanel();
    }

    public void UpdateScoreText(int score, int totalRounds) {
        scoreText.text = score.ToString() + "/" + totalRounds.ToString();
    }

    public void AddScore(string name, int score, int rounds, int level) {
        Score newScore = new Score();
        newScore.name = name;
        newScore.score = score;
        newScore.rounds = rounds;
        newScore.level = level;

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
