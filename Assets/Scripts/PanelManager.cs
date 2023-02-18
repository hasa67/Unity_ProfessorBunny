using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour {
    public GameObject infoPanel;
    public GameObject loginPanel;
    public GameObject mainPanel;
    public GameObject startPanel;
    public GameObject trainPanel;
    public GameObject blockPanel;
    public GameObject sandwichPanel;
    public GameObject reversePanel;
    public GameObject pairsPanel;
    public GameObject colorsPanel;
    public GameObject wormPanel;
    public GameObject cloudsPanel;
    public GameObject bellPanel;
    public GameObject phonePanel;
    public GameObject rhymePanel;
    public GameObject pausePanel;
    public GameObject countPanel;
    public GameObject starsPanel;
    public GameObject uploadPanel;

    private List<GameObject> panelsList = new List<GameObject>();
    private GameManager gameManager;

    private void Awake() {
        panelsList.Add(loginPanel);
        panelsList.Add(mainPanel);
        panelsList.Add(startPanel);
        panelsList.Add(trainPanel);
        panelsList.Add(sandwichPanel);
        panelsList.Add(blockPanel);
        panelsList.Add(reversePanel);
        panelsList.Add(pairsPanel);
        panelsList.Add(colorsPanel);
        panelsList.Add(wormPanel);
        panelsList.Add(cloudsPanel);
        panelsList.Add(bellPanel);
        panelsList.Add(phonePanel);
        panelsList.Add(rhymePanel);
        panelsList.Add(pausePanel);
        panelsList.Add(countPanel);
        panelsList.Add(starsPanel);
        panelsList.Add(uploadPanel);

        gameManager = FindObjectOfType<GameManager>();
    }

    public void HideAllPanels() {
        foreach (var panel in panelsList) {
            panel.SetActive(false);
        }
    }

    public void ShowLoginPanel() {
        HideAllPanels();
        loginPanel.SetActive(true);
    }

    public void ShowMainPanel() {
        HideAllPanels();
        mainPanel.SetActive(true);
    }

    public void ShowStartPanel() {
        HideAllPanels();
        startPanel.SetActive(true);
    }

    public void ShowPausePanel() {
        HideAllPanels();
        pausePanel.SetActive(true);
    }

    public void ShowCountPanel() {
        HideAllPanels();
        countPanel.SetActive(true);
    }

    public void ShowStarsPanel() {
        HideAllPanels();
        starsPanel.SetActive(true);
    }

    public void ShowUploadPanel() {
        HideAllPanels();
        uploadPanel.SetActive(true);
        Button uploadButton = GameObject.FindGameObjectWithTag("UploadButton").GetComponent<Button>();
        uploadButton.GetComponentInChildren<Text>().text = "بارگذاری نتایج";
        uploadButton.interactable = true;
        Button playButton = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
        playButton.interactable = false;
    }

    public void ShowTrainPanel() {
        HideAllPanels();
        trainPanel.SetActive(true);
    }

    public void ShowSandwichPanel() {
        HideAllPanels();
        sandwichPanel.SetActive(true);
    }

    public void ShowReversePanel() {
        HideAllPanels();
        reversePanel.SetActive(true);
    }

    public void ShowPairsPanel() {
        HideAllPanels();
        pairsPanel.SetActive(true);
    }

    public void ShowColorsPanel() {
        HideAllPanels();
        colorsPanel.SetActive(true);
    }

    public void ShowWormPanel() {
        HideAllPanels();
        wormPanel.SetActive(true);
    }

    public void ShowCloudsPanel() {
        HideAllPanels();
        cloudsPanel.SetActive(true);
    }

    public void ShowBellPanel() {
        HideAllPanels();
        bellPanel.SetActive(true);
    }

    public void ShowPhonePanel() {
        HideAllPanels();
        phonePanel.SetActive(true);
    }

    public void ShowRhymePanel() {
        HideAllPanels();
        rhymePanel.SetActive(true);
    }

    public void IsControllable(bool isControllable) {
        blockPanel.SetActive(!isControllable);
    }

    public void InfoPanel() {
        if (infoPanel.activeInHierarchy == true) {
            infoPanel.SetActive(false);
        } else {
            infoPanel.SetActive(true);
        }
    }
}
