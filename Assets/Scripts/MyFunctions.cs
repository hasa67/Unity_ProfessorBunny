using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyFunctions {

    public static void ShuffleTrainQuestionsList(List<TrainQuestionCard> list) {
        if (list.Count == 2) {
            var tmp1 = list[0];
            var tmp2 = list[1];
            var r = Random.Range(0, 2);
            if (r == 0) {
                return;
            } else {
                list[0] = tmp2;
                list[1] = tmp1;
            }
        } else {
            for (int i = list.Count - 1; i > 0; i--) {
                var r = Random.Range(0, i);
                var tmp = list[i];
                list[i] = list[r];
                list[r] = tmp;
            }
        }
    }

    public static void ShuffleDragDropList(List<DragDrop> list) {
        if (list.Count == 2) {
            var tmp1 = list[0];
            var tmp2 = list[1];
            var r = Random.Range(0, 2);
            if (r == 0) {
                return;
            } else {
                list[0] = tmp2;
                list[1] = tmp1;
            }
        } else {
            for (int i = list.Count - 1; i > 0; i--) {
                var r = Random.Range(0, i);
                var tmp = list[i];
                list[i] = list[r];
                list[r] = tmp;
            }
        }
    }

    public static void ShuffleClickableList(List<Clickable> list) {
        if (list.Count == 2) {
            var tmp1 = list[0];
            var tmp2 = list[1];
            var r = Random.Range(0, 2);
            if (r == 0) {
                return;
            } else {
                list[0] = tmp2;
                list[1] = tmp1;
            }
        } else {
            for (int i = list.Count - 1; i > 0; i--) {
                var r = Random.Range(0, i);
                var tmp = list[i];
                list[i] = list[r];
                list[r] = tmp;
            }
        }
    }

}
