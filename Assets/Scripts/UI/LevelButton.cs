using System;
using GameConstants;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Text levelNumTxt;
    #endregion

    #region PublicMethods
    public void SetupLevelButton(int levelId, LevelState state, Action<int> onClick)
    {
        var button = GetComponent<Button>();
        bool isLocked = state == LevelState.CLOSED;
        button.interactable = !isLocked;
        lockIcon.SetActive(isLocked);
        levelNumTxt.text = isLocked ? "" : $"{levelId + 1}";

        switch (state)
        {
            case LevelState.OPEN:
                levelNumTxt.text = $"{levelId + 1}";
                break;
            case LevelState.CLOSED:
                levelNumTxt.text = "";
                break;
            case LevelState.COMPLETE:
                levelNumTxt.text = "OK!";
                break;
            default:
                levelNumTxt.text = "";
                break;
                
        }
        
        button.onClick.AddListener(() =>
        {
            onClick?.Invoke(levelId);
        });
    }
    #endregion
}
