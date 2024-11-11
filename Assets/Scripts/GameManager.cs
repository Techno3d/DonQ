using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameOverScreen GameOverScreen;
    public TMP_Text counter;
    public int score = 0;
    public void IncScore(int byVal) {
        score += byVal;
        counter.text = "Treasures: " + score + "/9";
    }

    public void GameOver()
    {
        GameOverScreen.Setup();
    }
}
