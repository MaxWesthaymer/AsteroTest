using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SpawnController spawnController;
    [SerializeField] private LevelUI levelUi;
    [SerializeField] private Material asteroidMaterial;
    [SerializeField] private MeshRenderer back;
    #endregion

    #region PrivateFields
    private int _scoreToWin;
    private IntReactiveProperty _score;
    private IDisposable _dLives;
    private IDisposable _dIsDead;
    private IDisposable _dScore;
    #endregion
    
    #region UnityMethods
    void Start()
    {
        FillLevel();
        _score = new IntReactiveProperty(0);
        ReactiveSubscribe();
    }
    
    private void OnDestroy()
    {
        _dLives?.Dispose();
        _dIsDead?.Dispose();
        _dScore?.Dispose();
    }
    #endregion

    #region PrivateMethods
    private void FillLevel()
    {
        var config = GameData.Instance.gameConfig;
        var levelData = GameData.Instance.Data.levels[GameData.Instance.CurrentLevel];

        _scoreToWin = levelData.scoreToWin;
        
        playerController.SetShipParameters(config.ShipSpeed, config.ShipFireRate, config.ShipLivesCount);
        spawnController.SetSpawner(levelData.asteroidSpawnTime, levelData.asteroidSpeed, config.AsteroidsPrefabs[levelData.asteroidId], AddScore);
        
        back.material.SetColor("_ColorMid", config.SpaceColors[levelData.spaceColorId].TopColor);
        back.material.SetColor("_ColorTop", config.SpaceColors[levelData.spaceColorId].BottomColor);
        asteroidMaterial.color = config.AsteroidsColors[levelData.asteroidColorId];
    }
    private void ReactiveSubscribe()
    {
        _dLives = playerController.LivesCount.Subscribe(SetLivesInfo);
        _dIsDead = playerController.IsDead.Subscribe(SetGameOver);
        _dScore = _score.Subscribe(SetScoreInfo);
    }
    private void AddScore()
    {
        _score.Value++;

        if (_score.Value >= _scoreToWin)
        {
            SetWin();
        }
    }
    private void SetLivesInfo(int value)
    {
        levelUi.UpdateLevesInfo(value);
    }
    
    private void SetScoreInfo(int value)
    {
        levelUi.UpdateScoreInfo(value, _scoreToWin);
    }
    private void SetGameOver(bool value)
    {
        if (value)
        {
            levelUi.ShowEndGamePanel(false, LeaveToMenu, ReloadLevel);
        }
    }
    private void SetWin()
    {
        GameData.Instance.OpenNextLevel();
        playerController.SetEndGame();
        levelUi.ShowEndGamePanel(true, LeaveToMenu, ReloadLevel);
    }
    private void LeaveToMenu() => SceneManager.LoadScene(0);
    private void ReloadLevel() => SceneManager.LoadScene(1);
    #endregion
}
