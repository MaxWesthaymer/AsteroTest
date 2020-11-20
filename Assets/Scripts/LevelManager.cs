using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SpawnController spawnController;
    [SerializeField] private LevelUI levelUi;
    [SerializeField] private Material asteroidMaterial;
    [SerializeField] private MeshRenderer back;
    public int scoreToWin = 10;

    private IntReactiveProperty score;
    private IDisposable _dLives;
    private IDisposable _dIsDead;
    private IDisposable _dScore;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        var config = GameData.Instance.gameConfig;
        var levelData = GameData.Instance.Data.levels[GameData.Instance.CurrentLevel];
        
        playerController.SetShipParameters(config.ShipSpeed, config.ShipFireRate, config.ShipLivesCount);
        spawnController.SetSpawner(levelData.asteroidSpawnTime, config.AsteroidsPrefabs[levelData.asteroidId]);
        
        back.material.SetColor("_ColorMid", config.SpaceColors[levelData.spaceColorId].TopColor);
        back.material.SetColor("_ColorTop", config.SpaceColors[levelData.spaceColorId].BottomColor);
        asteroidMaterial.color = config.AsteroidsColors[levelData.asteroidColorId];
        
        score = new IntReactiveProperty(0);
        _dLives = playerController.LivesCount.Subscribe(SetLivesInfo);
        _dIsDead = playerController.IsDead.Subscribe(SetGameOver);
        _dScore = score.Subscribe(SetScoreInfo);
    }

    public void AddScore()
    {
        score.Value++;

        if (score.Value >= 10)
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
        levelUi.UpdateScoreInfo(value, scoreToWin);
    }
    
    private void SetGameOver(bool value)
    {
        if (value)
        {
            levelUi.ShowEndGamePanel(false);
        }
    }
    
    private void SetWin()
    {
        levelUi.ShowEndGamePanel(true);
    }

    private void OnDestroy()
    {
        _dLives?.Dispose();
        _dIsDead?.Dispose();
        _dScore?.Dispose();
    }
}
