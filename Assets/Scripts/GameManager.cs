using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {

    public Text scoreText;
    public TrainGameManager trainGameManager;

    private int score;
    private PanelManager panelManager;

    private void Awake() {
        panelManager = FindObjectOfType<PanelManager>();
    }

    void Start() {
        panelManager.ShowMainPanel();
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
}
