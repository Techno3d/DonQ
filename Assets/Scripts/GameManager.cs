using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text counter;
    public int score = 0;
    public void IncScore(int byVal)
    {
        score += byVal;
        counter.text = "Treasures: " + score + "/10";
        Settings.score = score;
    }
}
