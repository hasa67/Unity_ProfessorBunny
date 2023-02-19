using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public string ScoreList2String(List<Score> scoreList) {
        string scoreString = "";
        for (int i = 0; i < scoreList.Count; i++) {
            scoreString += scoreList[i].name + ",";
            scoreString += scoreList[i].score1.ToString() + ",";
            scoreString += scoreList[i].rounds.ToString() + ",";
            scoreString += scoreList[i].score2.ToString() + ",";
            scoreString += scoreList[i].maxScore.ToString() + ",";
            scoreString += scoreList[i].level.ToString() + ",";
            scoreString += scoreList[i].time.ToString() + ",";
            scoreString += scoreList[i].optional.ToString();
            if (i != scoreList.Count - 1) {
                scoreString += "\n";
            }
        }
        return scoreString;
    }

    public List<Score> String2ScoreList(string scoreString) {
        List<Score> scoreList = new List<Score>();
        scoreList.Clear();

        string[] lines = scoreString.Split('\n');
        for (int i = 0; i < lines.Length; i++) {
            string[] items = lines[i].Split(',');
            Score score = new Score();
            for (int j = 0; j < items.Length; j++) {
                score.name = items[0];
                score.score1 = int.Parse(items[1]);
                score.rounds = int.Parse(items[2]);
                score.score2 = int.Parse(items[3]);
                score.maxScore = int.Parse(items[4]);
                score.level = int.Parse(items[5]);
                score.time = float.Parse(items[6]);
                score.optional = int.Parse(items[7]);
            }
            scoreList.Add(score);
        }
        return scoreList;
    }

    public string ReplayList2String(List<bool> replayList) {
        string replayString = "";
        for (int i = 0; i < replayList.Count; i++) {
            int value = 0;
            if (replayList[i] == true) {
                value = 1;
            }
            replayString += value.ToString();
            if (i != replayList.Count - 1) {
                replayString += ",";
            }
        }
        return replayString;
    }

    public List<bool> String2ReplayList(string replayString) {
        List<bool> replayList = new List<bool>();
        replayList.Clear();

        string[] values = replayString.Split(',');
        for (int i = 0; i < values.Length; i++) {
            if (values[i] == "1") {
                replayList.Add(true);
            } else {
                replayList.Add(false);
            }
        }
        return replayList;
    }

    public void SaveGame(List<Score> scoreList, List<bool> replayList, int gameCount, int gameStage) {
        string scoreString = ScoreList2String(scoreList);
        PlayerPrefs.SetString("scoreString", scoreString);

        string replayString = ReplayList2String(replayList);
        PlayerPrefs.SetString("replayString", replayString);

        PlayerPrefs.SetInt("gameCount", gameCount);
        PlayerPrefs.SetInt("gameStage", gameStage);
    }
}
