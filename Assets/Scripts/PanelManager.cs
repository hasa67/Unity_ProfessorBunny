using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {
    public GameObject mainPanel;
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

    private List<GameObject> panelsList = new List<GameObject>();
    private GameManager gameManager;

    private void Awake() {
        panelsList.Add(mainPanel);
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

        gameManager = FindObjectOfType<GameManager>();
    }

    public void HideAllPanels() {
        foreach (var panel in panelsList) {
            panel.SetActive(false);
        }
    }

    public void ShowMainPanel() {
        HideAllPanels();
        mainPanel.SetActive(true);
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

    public void IsControllable(bool isControllable) {
        blockPanel.SetActive(!isControllable);
    }
}
