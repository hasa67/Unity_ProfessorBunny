using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {
    public bool isDebugging;
    public string playerName;
    public Text scoreText;
    public Text timerText;
    public Text playerNameText;
    public Slider levelSlider;
    public Text levelText;
    public Slider roundsSlider;
    public Text roundsText;
    public float roundsWaitTime;
    public float previewWaitTime;
    public Button startButton;
    public Button repeatButton;
    public Slider starsSlider;
    public ParticleSystem[] particles;

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
    private List<bool> replayList = new List<bool>();
    private bool isTestFunction = false;
    private int score;
    private int gameStage;
    private int gameCount;
    private PanelManager panelManager;
    private AudioManager audioManager;
    private VideoManager videoManager;
    private SaveManager saveManager;
    private Coroutine helpCoroutine;
    private string currentGameName;
    private bool stopWatch;
    private float timer;
    private string formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdtfiPjd8YddVyV0anqfKGyKtB2WhrF62qJsPmlnzcH-aFxBQ/formResponse";

    private void Awake() {
        // PlayerPrefs.DeleteAll();

        panelManager = FindObjectOfType<PanelManager>();
        audioManager = FindObjectOfType<AudioManager>();
        videoManager = FindObjectOfType<VideoManager>();
        saveManager = FindObjectOfType<SaveManager>();
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
        gameCount = 1;
        gameStage = 1;
        playerName = PlayerPrefs.GetString("PlayerName");

        if (isDebugging) {
            panelManager.ShowMainPanel();
        } else {
            panelManager.ShowStartPanel();
            audioManager.PlayMenuBackgrounMusic();
        }

        playerNameText.text = playerName;
        Stopwatch(false);
    }

    private void Update() {
        if (stopWatch == true) {
            timer += Time.deltaTime;
        }

        if (isDebugging) {
            timerText.text = timer.ToString();
            levelText.text = "Level " + levelSlider.value.ToString();
            roundsText.text = roundsSlider.value.ToString() + " Rounds";
        }
    }

    private void ShowPausePanel() {
        panelManager.ShowPausePanel();
        bunnyImageHolder.sprite = bunnySprites[Random.Range(0, bunnySprites.Length)];
    }

    public void StartGameButton() {
        StopCoroutine(helpCoroutine);
        audioManager.StopAllSources();
        videoManager.ClearVideoClip();
        if (isDebugging) {
            rounds = Mathf.RoundToInt(roundsSlider.value);
            if (!isTestFunction) {
                level = Mathf.RoundToInt(levelSlider.value);
            }
        }

        StartCoroutine(StartButtonCo());
    }

    IEnumerator StartButtonCo() {
        panelManager.ShowCountPanel();
        audioManager.PlayCountClip();
        yield return new WaitForSeconds(3f);

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
            case "pairs":
                panelManager.ShowPairsPanel();
                pairsGameManager.StartGame(rounds, level);
                break;
            case "colors":
                panelManager.ShowColorsPanel();
                colorsGameManager.StartGame(rounds, level);
                break;
            case "worm":
                panelManager.ShowWormPanel();
                wormGameManager.StartGame(rounds, level);
                break;
            case "clouds1":
                panelManager.ShowCloudsPanel();
                cloudsGameManeger.StartGame(rounds, level, 1);
                break;
            case "clouds2":
                panelManager.ShowCloudsPanel();
                cloudsGameManeger.StartGame(rounds, level, 2);
                break;
            case "bell":
                panelManager.ShowBellPanel();
                bellGameManager.StartGame(rounds, level);
                break;
            case "phone":
                panelManager.ShowPhonePanel();
                phoneGameManager.StartGame(rounds, level);
                break;
            case "rhyme":
                panelManager.ShowRhymePanel();
                rhymeGameManager.StartGame(rounds);
                break;
            default:
                Debug.Log("StartGameButton: currentGameName not found!");
                break;
        }
    }

    public void RepeatHelpVoice() {
        StopCoroutine(helpCoroutine);
        audioManager.StopAllSources();
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    IEnumerator StartGameCo() {
        audioManager.SetGameClip(currentGameName);
        videoManager.SetVideoClip(currentGameName);

        float waitTime = audioManager.PlaySource1();
        yield return new WaitForSeconds(waitTime);
    }

    public void StartTestButton() {
        isTestFunction = true;

        int count = PlayerPrefs.GetInt("gameCount", 0);
        int stage = PlayerPrefs.GetInt("gameStage", 0);

        if (count != 0) {
            LoadGame();
            if (count == 1 && stage == 2) {
                scoreList.Clear();
            }
        }

        if (stage == 3) {
            audioManager.StopAllSources();
            audioManager.PlayWrongClip();
            return;
        }
        StartTest();
    }

    public void StartTest() {
        audioManager.StopAllSources();
        ShowPausePanel();

        if (isDebugging) {
            rounds = Mathf.RoundToInt(roundsSlider.value);
        } else {
            rounds = 3;
        }

        // stage finished
        if (gameCount > 23) {
            isTestFunction = false;
            if (isDebugging) {
                panelManager.ShowMainPanel();
            } else {
                panelManager.ShowUploadPanel();
            }
            return;
        }

        if (gameStage == 2) {
            if (replayList[gameCount - 1] == false) {
                gameCount++;
                StartTest();
                return;
            }
        }

        if (gameCount <= 3) {
            currentGameName = "pairs";
            level = gameCount;
        } else if (gameCount <= 6) {
            currentGameName = "reverse";
            level = gameCount - 3;
        } else if (gameCount <= 7) {
            currentGameName = "sandwich";
        } else if (gameCount <= 8) {
            currentGameName = "train";
        } else if (gameCount <= 9) {
            currentGameName = "clouds1";
        } else if (gameCount <= 10) {
            currentGameName = "clouds2";
        } else if (gameCount <= 13) {
            currentGameName = "colors";
            level = gameCount - 10;
        } else if (gameCount <= 16) {
            currentGameName = "bell";
            level = gameCount - 13;
        } else if (gameCount <= 17) {
            currentGameName = "rhyme";
        } else if (gameCount <= 20) {
            currentGameName = "worm";
            level = gameCount - 17;
        } else if (gameCount <= 23) {
            currentGameName = "phone";
            level = gameCount - 20;
        }

        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void NextStage() {
        scoreList.Clear();
        if (gameStage == 2) {
            isTestFunction = true;
            StartTest();
        } else {
            Button playButton = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
            Text playButtonText = playButton.GetComponentInChildren<Text>();
            playButtonText.text = "بازی تمام شد";
            playButton.interactable = false;
        }
    }

    public void StartTrainGame() {
        ShowPausePanel();
        currentGameName = "train";
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void StartSandwichGame() {
        ShowPausePanel();
        currentGameName = "sandwich";
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void StartReverseGame() {
        ShowPausePanel();
        currentGameName = "reverse";
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void StartPairsGame() {
        ShowPausePanel();
        currentGameName = "pairs";
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void StartColorsGame() {
        ShowPausePanel();
        currentGameName = "colors";
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void StartWormGame() {
        ShowPausePanel();
        currentGameName = "worm";
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void StartCloudsGame1() {
        ShowPausePanel();
        currentGameName = "clouds1";
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void StartCloudsGame2() {
        ShowPausePanel();
        currentGameName = "clouds2";
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void StartBellGame() {
        ShowPausePanel();
        currentGameName = "bell";
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void StartPhoneGame() {
        ShowPausePanel();
        currentGameName = "phone";
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void StartRhymeGame() {
        ShowPausePanel();
        currentGameName = "rhyme";
        helpCoroutine = StartCoroutine(StartGameCo());
    }

    public void EndGame() {
        gameCount++;
        if (isTestFunction) {
            SaveGame();
        }
        StartCoroutine(EndGameCo());
    }

    IEnumerator EndGameCo() {
        panelManager.ShowStarsPanel();

        float newMin = 0.5f;
        float newMax = 5f;
        float oldMin = 0f;
        float oldMax = scoreList[scoreList.Count - 1].maxScore;

        float oldScore = scoreList[scoreList.Count - 1].score2;
        float newScore = (((oldScore - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + 0.5f;

        audioManager.PlayStarChargingSound();
        int starsFilled = 0;
        float filler = 0f;
        while (filler < newScore) {
            filler += 2f * Time.deltaTime;
            starsSlider.value = filler;
            if (starsFilled == 0 && filler >= 1) {
                audioManager.PlayStarClip(starsFilled);
                particles[starsFilled].Play();
                starsFilled++;
            } else if (starsFilled == 1 && filler >= 2) {
                audioManager.PlayStarClip(starsFilled);
                particles[starsFilled].Play();
                starsFilled++;
            } else if (starsFilled == 2 && filler >= 3) {
                audioManager.PlayStarClip(starsFilled);
                particles[starsFilled].Play();
                starsFilled++;
            } else if (starsFilled == 3 && filler >= 4) {
                audioManager.PlayStarClip(starsFilled);
                particles[starsFilled].Play();
                starsFilled++;
            } else if (starsFilled == 4 && filler >= 5) {
                audioManager.PlayStarClip(starsFilled);
                particles[starsFilled].Play();
                starsFilled++;
            }
            yield return null;
        }
        audioManager.StopSource1();
        yield return new WaitForSeconds(2f);

        if (isTestFunction) {
            StartTest();
        } else {
            panelManager.ShowMainPanel();
        }
    }

    public void UpdateScoreText(int score, int totalRounds) {
        scoreText.text = score.ToString() + "/" + totalRounds.ToString();
    }

    public void AddScore(string name, int score1, int rounds, int score2, int maxScore, int level, int optional = 0) {
        Score newScore = new Score();
        newScore.name = name;
        newScore.score1 = score1;
        newScore.rounds = rounds;
        newScore.score2 = score2;
        newScore.maxScore = maxScore;
        newScore.level = level;
        newScore.time = timer;
        newScore.optional = optional;
        scoreList.Add(newScore);

        if (isTestFunction && gameStage == 1) {
            UpdateReplayList();
        }

        ResetStopwatch();
    }

    private void UpdateReplayList() {
        Score score = scoreList[scoreList.Count - 1];
        bool replay = true;
        switch (score.name) {
            case "train":
                if (score.time <= 20 && score.score1 >= 2) {
                    replay = false;
                }
                break;
            case "sandwich":
                if (score.time <= 10 && score.score1 >= 2) {
                    replay = false;
                }
                break;
            case "reverse":
                if ((score.level == 1 && score.time <= 6 && score.score1 >= 2) ||
                    (score.level == 2 && score.time <= 10 && score.score1 >= 2) ||
                    (score.level == 3 && score.time <= 16 && score.score2 >= 4)) {
                    replay = false;
                }
                break;
            case "pairs":
                if ((score.level == 1 && score.time <= 7 && score.optional >= -1) ||
                    (score.level == 2 && score.time <= 12 && score.optional >= -2) ||
                    (score.level == 3 && score.time <= 22 && score.optional >= -2)) {
                    replay = false;
                }
                break;
            case "colors":
                if ((score.level == 1 && score.time <= 16 && score.score1 >= 2) ||
                    (score.level == 2 && score.time <= 21 && score.score1 >= 2) ||
                    (score.level == 3 && score.time <= 26 && score.score1 >= 2)) {
                    replay = false;
                }
                break;
            case "worm":
                if ((score.level == 1 && score.time <= 22 && score.score1 >= 2) ||
                    (score.level == 2 && score.time <= 23 && score.score1 >= 2) ||
                    (score.level == 3 && score.time <= 25 && score.score1 >= 2)) {
                    replay = false;
                }
                break;
            case "clouds1":
                if (score.time <= 5 && score.score1 >= 2) {
                    replay = false;
                }
                break;
            case "clouds2":
                if (score.time <= 8 && score.score1 >= 2) {
                    replay = false;
                }
                break;
            case "bell":
                if (score.time <= 3.6f && score.score1 >= 2) {
                    replay = false;
                }
                break;
            case "phone":
                if ((score.level == 1 && score.time <= 7 && score.score1 >= 2) ||
                    (score.level == 2 && score.time <= 10 && score.score1 >= 2) ||
                    (score.level == 3 && score.time <= 16 && score.score1 >= 2)) {
                    replay = false;
                }
                break;
            case "rhyme":
                if (score.time <= 10 && score.score1 >= 2) {
                    replay = false;
                }
                break;
            default:
                Debug.Log("replayList: gameName not found!");
                break;
        }
        replayList.Add(replay);
    }

    public void UploadScore() {
        if (scoreList.Count > 0) {
            string scoreString = saveManager.ScoreList2String(scoreList);
            // Debug.Log(scoreString);
            StartCoroutine(UploadScoreCo(scoreString));
        } else {
            // Debug.Log("score list empty");
            audioManager.PlayWrongClip();
        }
    }

    IEnumerator UploadScoreCo(string scoreString) {
        Button uploadButton = GameObject.FindGameObjectWithTag("UploadButton").GetComponent<Button>();
        Button playButton = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();

        string uploadingText;
        string errorText;
        string doneText;

        bool isUploaded = false;

        if (isDebugging) {
            uploadingText = "uploading ...";
            errorText = "error!";
            doneText = "done!";
        } else {
            uploadingText = "در حال بارگذاری";
            errorText = "خطا";
            doneText = "اتمام بارگذاری";
        }

        Text uploadButtonText = uploadButton.GetComponentInChildren<Text>();
        uploadButtonText.text = uploadingText;
        uploadButton.interactable = false;

        WWWForm form = new WWWForm();
        form.AddField("entry.1429677245", playerName + " - " + gameStage.ToString());
        form.AddField("entry.454959095", scoreString);
        byte[] rawData = form.data;

        UnityWebRequest www = UnityWebRequest.Post(formUrl, form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError) {
            Debug.Log(www.error);
            uploadButtonText.text = errorText;
            audioManager.PlayWrongClip();
            isUploaded = false;
        } else {
            Debug.Log("upload completed");
            uploadButtonText.text = doneText;
            audioManager.PlayCorrectClip();
            isUploaded = true;

            gameCount = 1;
            gameStage++;
            SaveGame();
        }

        yield return new WaitForSeconds(2f);

        if (isDebugging) {
            uploadButton.interactable = true;
            uploadButtonText.text = "Upload";
            playButton.interactable = true;
        } else {
            if (isUploaded) {
                playButton.interactable = true;
            } else {
                uploadButton.interactable = true;
                uploadButtonText.text = "بارگذاری نتایج";
            }
        }
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

    private void SaveGame() {
        saveManager.SaveGame(scoreList, replayList, gameCount, gameStage);
    }

    private void LoadGame() {
        string scoreString = PlayerPrefs.GetString("scoreString");
        scoreList = saveManager.String2ScoreList(scoreString);

        string replayString = PlayerPrefs.GetString("replayString");
        replayList = saveManager.String2ReplayList(replayString);

        gameCount = PlayerPrefs.GetInt("gameCount");
        gameStage = PlayerPrefs.GetInt("gameStage");
    }
}
