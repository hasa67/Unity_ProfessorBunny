using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReverseAnswerSlot : MonoBehaviour {

    public string answer;
    public bool isFull;
    public bool isCorrect;

    private void Awake() {
        Initialize();
    }

    public void Initialize() {
        isFull = false;
        isCorrect = false;
    }
}
