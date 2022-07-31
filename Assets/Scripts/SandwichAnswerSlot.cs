using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandwichAnswerSlot : MonoBehaviour {
    private SandwichGameManager sandwichGameManager;

    public string answer;
    public bool isFull;
    public bool isCorrect;
    public Image image;

    private void Awake() {
        sandwichGameManager = FindObjectOfType<SandwichGameManager>();
        image = GetComponent<Image>();

        Initialize();
    }

    public void Initialize() {
        image.enabled = false;
        isFull = false;
        isCorrect = false;
    }
}
