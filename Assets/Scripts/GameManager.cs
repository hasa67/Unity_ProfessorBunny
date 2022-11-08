using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    public string playerName;
    public Text scoreText;
    public Text timerText;
    public Text playerNameText;
    public Slider levelSlider;
    public Text levelText;
    public Slider roundsSlider;
    public Text roundsText;
    public Button uploadButton;
    public float roundsWaitTime;
    public float previewWaitTime;
    public Button startButton;
    public Button repeatButton;

    [Range(1, 10)] public int rounds;
    [Range(1, 3)] public int level;

    public Image bunnyImageHolder;
    public Sprite[] bunnySprites;

    [HideInInspector] public TrainGameManager trainGameManager;
    [HideInInspector] public SandwichGameManager sandwichGameManager;
    [HideInInspector] public ReverseGameManager reverseGameManager;
    [HideInInspector] public PairsGameManager pairsGameManager;
    [HideInInspector] public ColorsGameManager colorsGameManager;
    [HideInInspector] public WormGameManager wormGameManager;
    [HideInInspector] public CloudsGameManager cloudsGameManeger;
    [HideInInspector] public BellGameManager bellGameManager;
    [HideInInspector] public PhoneGameManager phoneGameManager;
    [HideInInspector] public RhymeGameManager rhymeGameManager;

    private List<Score> scoreList = new List<Score>();
    private int score;
    private PanelManager panelManager;
    private AudioManager audioManager;
    private VideoManager videoManager;
    private string currentGameName;
    private bool stopWatch;
    private float timer;
    private string formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdtfiPjd8YddVyV0anqfKGyKtB2WhrF62qJsPmlnzcH-aFxBQ/formResponse";

    private void Awake() {
        // PlayerPrefs.DeleteAll();

        panelManager = FindObjectOfType<PanelManager>();
        audioManager = FindObjectOfType<AudioManager>();
        videoManager = FindObjectOfType<VideoManager>();
    }

    void Start() {
        playerName = PlayerPrefs.GetString("PlayerName");
        if (playerName == "") {
            panelManager.ShowLoginPanel();
        } else {
            Initialize();
        }
    }

    public void Initialize() {
        playerName = PlayerPrefs.GetString("PlayerName");
        panelManager.ShowMainPanel();
        playerNameText.text = playerName;

        Stopwatch(false);
    }

    private void Update() {
        if (stopWatch == true) {
            timer += Time.deltaTime;
        }
        timerText.text = timer.ToString();

        levelText.text = "Level " + levelSlider.value.ToString();
        level = Mathf.RoundToInt(levelSlider.value);

        roundsText.text = roundsSlider.value.ToString() + " Rounds";
        rounds = Mathf.RoundToInt(roundsSlider.value);
    }

    private void ShowPausePanel() {
        panelManager.ShowPausePanel();
        bunnyImageHolder.sprite = bunnySprites[Random.Range(0, bunnySprites.Length)];
    }

    public void StartGameButton() {
        switch (currentGameName) {
            case "train":
                panelManager.ShowTrainPanel();
                trainGameManager.StartGame(rounds);
                break;
            case "sandwich":
                panelManager.ShowSandwichPanel();
                sandwichGameManager.StartGame(rounds);
                break;
            case "reverse":
                panelManager.ShowReversePanel();
                reverseGameManager.StartGame(rounds, level);
                break;
            default:
                Debug.Log("StartGameButton: currentGameName not found!");
                break;
        }
    }

    public void RepeatHelpVoice() {
        StartCoroutine(StartGameCo());
    }

    IEnumerator StartGameCo() {
        audioManager.SetGameClip(currentGameName);
        videoManager.SetVideoClip(currentGameName);

        startButton.interactable = false;
        repeatButton.interactable = false;

        float waitTime = audioManager.PlayClip();
        yield return new WaitForSeconds(waitTime);
        yield return new WaitForSeconds(0.5f);
        waitTime = audioManager.PlayHelpClip();
        yield return new WaitForSeconds(waitTime);

        startButton.interactable = true;
        repeatButton.interactable = true;
    }

    public void StartTrainGame() {
        ShowPausePanel();
        currentGameName = "train";
        StartCoroutine(StartGameCo());
    }

    public void StartSandwichGame() {
        ShowPausePanel();
        currentGameName = "sandwich";
        StartCoroutine(StartGameCo());
    }

    public void StartReverseGame() {
        ShowPausePanel();
        currentGameName = "reverse";
        StartCoroutine(StartGameCo());
    }

    public void StartPairsGame() {
        panelManager.ShowPairsPanel();
        pairsGameManager.StartGame(rounds, level);
    }

    public void StartColorsGame() {
        panelManager.ShowColorsPanel();
        colorsGameManager.StartGame(rounds, level);
    }

    public void StartWormGame() {
        panelManager.ShowWormPanel();
        wormGameManager.StartGame(rounds, level);
    }

    public void StartCloudsGame1() {
        panelManager.ShowCloudsPanel();
        cloudsGameManeger.StartGame(rounds, level, 1);
    }

    public void StartCloudsGame2() {
        panelManager.ShowCloudsPanel();
        cloudsGameManeger.StartGame(rounds, level, 2);
    }

    public void StartBellGame() {
        panelManager.ShowBellPanel();
        bellGameManager.StartGame(rounds, level);
    }

    public void StartPhoneGame() {
        panelManager.ShowPhonePanel();
        phoneGameManager.StartGame(rounds, level);
    }

    public void StartRhymeGame() {
        panelManager.ShowRhymePanel();
        rhymeGameManager.StartGame(rounds);
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
