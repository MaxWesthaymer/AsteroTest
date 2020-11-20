using System;
using System.Collections.Generic;
using GameConstants;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameData : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] public GameConfig gameConfig;
    #endregion
    
    #region Propierties
    public static GameData Instance { get; private set; }
    public Data Data { get; private set; }
    public int CurrentLevel { get; private set; }
    #endregion
    
    #region UnityMethods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        LoadData();
    }
    #endregion

    #region PublicMethods
    public void Save() => SaveData(Data);
    public void SetCurrentLevel(int value)
    {
        CurrentLevel = value;
    }

    public void OpenNextLevel()
    {
        Data.levels[CurrentLevel].levelState = LevelState.COMPLETE;
        var nextLevel = CurrentLevel + 1;
        if (nextLevel < Data.levels.Count && Data.levels[nextLevel].levelState == LevelState.CLOSED)
        {
            Data.levels[nextLevel].levelState = LevelState.OPEN;
        }
        Save();
    }
    #endregion
    
    #region PrivateMethods
    private void SaveData(object obj)
    {
        var str = JsonUtility.ToJson(obj);
        PlayerPrefs.SetString("savingdata", str);
    }
    
    private void LoadData()
    {
        var str = PlayerPrefs.GetString("savingdata");
        if (str == String.Empty)
        {
            Data = new Data();
            Data.levels = new List<LevelData>();

            for (int i = 0; i < gameConfig.CountOfLevels; i++)
            {
                Data.levels.Add(CreateRandomLevel());
            }

            Data.levels[0].levelState = LevelState.OPEN;
        }
        else
        {
            Data = JsonUtility.FromJson<Data>(str);
        }
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private LevelData CreateRandomLevel()
    {
        var asteroidId = Random.Range(0, gameConfig.AsteroidsPrefabs.Length);
        var asterSpeed = Random.Range(gameConfig.AsteroidsSpeedRange.x, gameConfig.AsteroidsSpeedRange.y);
        var asteroidSpawnTime = Random.Range(gameConfig.AsteroidsSpawnTimeRange.x, gameConfig.AsteroidsSpawnTimeRange.y);
        var asteroidColorId = Random.Range(0, gameConfig.AsteroidsColors.Length);
        var scoreToWin = Random.Range(gameConfig.ScoreToWinRange.x, gameConfig.ScoreToWinRange.y + 1);
        var spaceColorId = Random.Range(0, gameConfig.SpaceColors.Length);
        return new LevelData(LevelState.CLOSED, asteroidId, asterSpeed, asteroidSpawnTime, asteroidColorId, scoreToWin, spaceColorId);
    }
    #endregion
}

[Serializable]
public class Data
{
    public List<LevelData> levels;
}

[Serializable]
public class LevelData
{
    public LevelData(LevelState levelState, int asteroidId, float asteroidSpeed, float asteroidSpawnTime, int asteroidColorId, int scoreToWin, int spaceColorId)
    {
        this.levelState = levelState;
        this.asteroidId = asteroidId;
        this.asteroidSpeed = asteroidSpeed;
        this.asteroidSpawnTime = asteroidSpawnTime;
        this.asteroidColorId = asteroidColorId;
        this.scoreToWin = scoreToWin;
        this.spaceColorId = spaceColorId;
    }

    public LevelState levelState;
    public int asteroidId;
    public float asteroidSpeed;
    public float asteroidSpawnTime;
    public int asteroidColorId;
    public int scoreToWin;
    public int spaceColorId;
}


