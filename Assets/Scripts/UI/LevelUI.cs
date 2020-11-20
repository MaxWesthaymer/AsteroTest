using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private GameObject endGamePanlel;
    [SerializeField] private Text endGameTitleTxt;
    [SerializeField] private Text levesTxt;
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Button retryLevelBtn;
    [SerializeField] private Button leaveToMenuBtn;
    #endregion

    #region PublicMethods
    public void ShowEndGamePanel(bool isWin, Action leave, Action retry)
    {
        endGamePanlel.SetActive(true);
        endGameTitleTxt.text = isWin ? $"Level Complete!" : "Game Over";
        leaveToMenuBtn.onClick.AddListener(() =>
        {
            leave?.Invoke();
        });
        retryLevelBtn.onClick.AddListener(() =>
        {
            retry?.Invoke();
        });
    }
    
    public void UpdateLevesInfo(int value)
    {
        levesTxt.text = $"Lives: <color=#00ff00ff>{value}</color>";
    }
    
    public void UpdateScoreInfo(int value, int scoreToWin)
    {
        scoreTxt.text = $"Score: <color=#00ff00ff>{value}/{scoreToWin}</color>";
    }
    #endregion
}
