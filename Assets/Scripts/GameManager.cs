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
    public int trainRounds;
    public int sandwichRounds;
    public int reverseRounds;

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

    public void EndTrainGame() {
        StartSandwichGame();
    }

    public void StartSandwichGame() {
        panelManager.ShowSandwichPanel();
        sandwichGameManager.StartGame(sandwichRounds);
    }

    public void EndSandwichGame() {
        StartReverseGame();
    }

    public void StartReverseGame() {
        panelManager.ShowReversePanel();
        reverseGameManager.StartGame(reverseRounds);
    }

    public void EndReverseGame() {
        panelManager.HideAllPanels();
    }

    public void UpdateScoreText(int score) {
        scoreText.text = score.ToString();
    }

    public void AddScore(int scoreValue) {
        Score newScore = new Score();
        newScore.score = scoreValue;
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
