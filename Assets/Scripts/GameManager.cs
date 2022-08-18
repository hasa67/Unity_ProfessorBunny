using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    public string playerName = "testPlayer";
    public Text scoreText;
    public Text timerText;
    public Slider levelSlider;
    public Text levelText;
    public Button uploadButton;
    public TrainGameManager trainGameManager;
    public SandwichGameManager sandwichGameManager;
    public ReverseGameManager reverseGameManager;
    public PairsGameManager pairsGameManager;
    public ColorsGameManager colorsGameManager;
    public WormGameManager wormGameManager;
    public CloudsGameManager cloudsGameManeger;
    public BellGameManager bellGameManager;
    public PhoneGameManager phoneGameManager;
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
    [Range(1, 5)] public int bellRounds;
    [Range(1, 3)] public int bellLevel;
    [Range(1, 5)] public int phoneRounds;
    [Range(1, 3)] public int phoneLevel;



    public List<Score> scoreList = new List<Score>();
    private int score;
    private PanelManager panelManager;
    private AudioManager audioManager;
    private bool stopWatch;
    private float timer;
    private string formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdtfiPjd8YddVyV0anqfKGyKtB2WhrF62qJsPmlnzcH-aFxBQ/formResponse";

    private void Awake() {
        panelManager = FindObjectOfType<PanelManager>();
        audioManager = FindObjectOfType<AudioManager>();
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
        bellLevel = Mathf.RoundToInt(levelSlider.value);
        phoneLevel = Mathf.RoundToInt(levelSlider.value);
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

    public void StartBellGame() {
        panelManager.ShowBellPanel();
        bellGameManager.StartGame(bellRounds, bellLevel);
    }

    public void StartPhoneGame() {
        panelManager.ShowPhonePanel();
        phoneGameManager.StartGame(phoneRounds, phoneLevel);
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

    public void UploadScore() {
        if (scoreList.Count > 0) {
            string scoreString = "";
            for (int i = 0; i < scoreList.Count; i++) {
                scoreString += scoreList[i].name + ",";
                scoreString += scoreList[i].score.ToString() + ",";
                scoreString += scoreList[i].rounds.ToString() + ",";
                scoreString += scoreList[i].level.ToString() + ",";
                scoreString += scoreList[i].time.ToString();
                if (i != scoreList.Count - 1) {
                    scoreString += "\n";
                }
            }
            // Debug.Log(scoreString);
            StartCoroutine(UploadScoreCo(scoreString));
        } else {
            // Debug.Log("score list empty");
            audioManager.PlayWrongClip();
        }
    }

    IEnumerator UploadScoreCo(string scoreString) {
        Text buttonText = uploadButton.GetComponentInChildren<Text>();
        buttonText.text = "uploading ...";
        uploadButton.interactable = false;

        WWWForm form = new WWWForm();
        form.AddField("entry.1429677245", playerName);
        form.AddField("entry.454959095", scoreString);
        byte[] rawData = form.data;

        UnityWebRequest www = UnityWebRequest.Post(formUrl, form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError) {
            Debug.Log(www.error);
            buttonText.text = "error!";
            audioManager.PlayWrongClip();
        } else {
            Debug.Log("upload completed");
            buttonText.text = "done!";
            audioManager.PlayCorrectClip();
        }

        yield return new WaitForSeconds(2f);
        uploadButton.interactable = true;
        buttonText.text = "Upload";
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
