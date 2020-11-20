using System.Collections;
using System.Collections.Generic;
using GameConstants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private LevelButton levelButtonPrefab;
    [SerializeField] private Transform buttonsContainer;
    #endregion
    
    #region UnityMethods
    private void Start()
    {
        InstantiateLevels();
    }
    #endregion

    #region PrivateMethods
    private void LoadBuildingLevel(int levelId)
    {
        GameData.Instance.SetCurrentLevel(levelId);
        SceneManager.LoadScene(1);
    }

    private void InstantiateLevels()
    {
        for (var levelId = 0; levelId < GameData.Instance.Data.levels.Count; levelId++)
        {
            var levelBtn = Instantiate(levelButtonPrefab);
            levelBtn.transform.parent = buttonsContainer;
            levelBtn.transform.localScale = Vector3.one;
            var isLocked = GameData.Instance.Data.levels[levelId].levelState == LevelState.CLOSED;
            levelBtn.SetupLevelButton(levelId, isLocked, LoadBuildingLevel);
        }
    }
    #endregion
}
