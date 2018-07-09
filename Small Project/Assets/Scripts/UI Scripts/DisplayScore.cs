using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour {

    public static DisplayScore Instance;
    public Text scoreDisplay;
    int totalAmount = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateScore(int amount)
    {
        totalAmount += amount;
        scoreDisplay.text = "Score: " + totalAmount;
    }
}
