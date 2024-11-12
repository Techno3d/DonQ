using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] Text text;
    void Start()
    {
        text.text = Settings.score + "/10 TREASURES WERE FOUND";
    }
}
