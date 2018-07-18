using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedDecision {

    int weight;
    int decisionNum;
    int rangeStart = 0;

    public WeightedDecision(int _decisionNum, int _weight) {
        weight = _weight;
        decisionNum = _decisionNum;
    }

    static public int MakeDecision(List<WeightedDecision> decisionList) {
        int totalWeight = 0;
        for (int i = 0; i < decisionList.Count; i++) {
            decisionList[i].rangeStart = totalWeight;
            totalWeight += decisionList[i].weight;
        }

        int rndNum = Random.Range(0, totalWeight);
        for (int i = 0; i < decisionList.Count; i++) {
            if (rndNum >= decisionList[i].rangeStart && rndNum <= (decisionList[i].rangeStart + decisionList[i].weight)) {
                return i;
            }
        }
        Debug.Log("Oops, that didn't work");
        return 0;
    }

    void DebugPrint() {
        Debug.Log("Num: " + decisionNum + " weight: " + weight + " range: " + rangeStart + "/" + (rangeStart + weight));
    }
}
