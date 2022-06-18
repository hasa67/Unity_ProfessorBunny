using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int score;

    void Start() {
        DontDestroyOnLoad(this);
    }

    public void StartButton() {
        SceneManager.LoadScene("Level1");
    }
}
