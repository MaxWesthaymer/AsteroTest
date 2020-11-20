using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanlel;
    [SerializeField] private Text endGameTitleTxt;
    [SerializeField] private Text levesTxt;
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Button retryLevel;
    [SerializeField] private Button leaveToMenu;


    public void ShowEndGamePanel(bool isWin)
    {
        endGamePanlel.SetActive(true);
        endGameTitleTxt.text = isWin ? $"Level Complete!" : "Game Over";
    }
    
    public void UpdateLevesInfo(int value)
    {
        levesTxt.text = $"Lives: <color=#00ff00ff>{value}</color>";
    }
    
    public void UpdateScoreInfo(int value, int scoreToWin)
    {
        scoreTxt.text = $"Score: <color=#00ff00ff>{value}/{scoreToWin}</color>";
    }
}
