using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {
    public GameObject mainPanel;
    public GameObject trainPanel;

    private List<GameObject> panelsList = new List<GameObject>();
    private GameManager gameManager;

    private void Awake() {
        panelsList.Add(mainPanel);
        panelsList.Add(trainPanel);

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

        gameManager.StratTrainGame();
    }
}
