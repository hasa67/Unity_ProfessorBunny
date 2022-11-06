using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

    public InputField playerNameInput;
    public Button acceptButton;

    private GameManager gameManager;

    void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start() {
        acceptButton.interactable = false;
    }

    public void CheckNameLength() {
        if (playerNameInput.text.Replace(" ", "").Length >= 3) {
            acceptButton.interactable = true;
        } else {
            acceptButton.interactable = false;
        }
    }

    public void SetPlayerName() {
        PlayerPrefs.SetString("PlayerName", playerNameInput.text);
        gameManager.Initialize();
    }
}
