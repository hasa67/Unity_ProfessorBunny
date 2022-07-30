using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text scoreText;
    public Text timerText;
    public TrainGameManager trainGameManager;

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
        trainGameManager.StartGame();
    }

    public void EndTrainGame() {
        panelManager.HideAllPanels();
    }

    public void AddScore(int value) {
        score += value;
        scoreText.text = score.ToString();
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
