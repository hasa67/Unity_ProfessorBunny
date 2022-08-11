using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text scoreText;
    public Text timerText;
    public Slider levelSlider;
    public Text levelText;
    public TrainGameManager trainGameManager;
    public SandwichGameManager sandwichGameManager;
    public ReverseGameManager reverseGameManager;
    public PairsGameManager pairsGameManager;
    public ColorsGameManager colorsGameManager;
    public WormGameManager wormGameManager;
    public CloudsGameManager cloudsGameManeger;
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
    [Range(1, 5)] public int wormRounds;
    [Range(1, 3)] public int wormLevel;
    [Range(1, 5)] public int cloudsRounds;
    [Range(1, 3)] public int cloudsLevel;


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

        levelText.text = "Level " + levelSlider.value.ToString();
        reverseLevel = Mathf.RoundToInt(levelSlider.value);
        pairsLevel = Mathf.RoundToInt(levelSlider.value);
        colorsLevel = Mathf.RoundToInt(levelSlider.value);
        wormLevel = Mathf.RoundToInt(levelSlider.value);
        cloudsLevel = Mathf.RoundToInt(levelSlider.value);
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

    public void StratWormGame() {
        panelManager.ShowWormPanel();
        wormGameManager.StartGame(wormRounds, wormLevel);
    }

    public void StartCloudsGame1() {
        panelManager.ShowCloudsPanel();
        cloudsGameManeger.StartGame(cloudsRounds, cloudsLevel, 1);
    }

    public void StartCloudsGame2() {
        panelManager.ShowCloudsPanel();
        cloudsGameManeger.StartGame(cloudsRounds, cloudsLevel, 2);
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
