using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score {
    public string name;
    public int score1; // reduced correct
    public int score2; // total correct
    public int rounds;
    public int level;
    public float time;
    public int optional; // additional info (touch, etc.)
}
