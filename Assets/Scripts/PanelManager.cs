using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {
    public GameObject mainPanel;
    public GameObject trainPanel;
    public GameObject blockPanel;
    public GameObject sandwichPanel;
    public GameObject reversePanel;

    private List<GameObject> panelsList = new List<GameObject>();
    private GameManager gameManager;

    private void Awake() {
        panelsList.Add(mainPanel);
        panelsList.Add(trainPanel);
        panelsList.Add(sandwichPanel);
        panelsList.Add(blockPanel);
        panelsList.Add(reversePanel);

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

    public void IsControllable(bool isControllable) {
        blockPanel.SetActive(!isControllable);
    }
}
