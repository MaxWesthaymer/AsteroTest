using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    #region PrivateFields
    private GameObject _asteroidPrefab;
    private Vector2 _screenHalfSizeInWorldUnits;
    private float _spawnRate;
    private float _cooldown;
    private Action _asteroidDeath;
    private float _asteroidSpeed;
    #endregion

    #region PublickMethods
    public void SetSpawner(float spawnTime,float asteroidSpeed, GameObject asteroid, Action asteroidDeath)
    {
        _spawnRate = spawnTime;
        _asteroidPrefab = asteroid;
        _asteroidDeath = asteroidDeath;
        _asteroidSpeed = asteroidSpeed;
    }
    #endregion
    
    #region UnityMethods
    private void Start()
    {
        CalculateScreenSize();
    }
    
    private void Update()
    {
        if (_cooldown <= 0)
        {
            InstantiateAsteroid();
            _cooldown = _spawnRate;
        }
        _cooldown -= Time.deltaTime;
    }
    #endregion

    #region PrivateMethods
    private void CalculateScreenSize()
    {
        Camera mainCamera = Camera.main;
        _screenHalfSizeInWorldUnits = new Vector2(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize);
    }
    private void InstantiateAsteroid()
    {
        Vector2 spawnPoint = new Vector2( Random.Range(-_screenHalfSizeInWorldUnits.x, _screenHalfSizeInWorldUnits.x), _screenHalfSizeInWorldUnits.y + 1f);
        var asteroid = Instantiate(_asteroidPrefab, spawnPoint, Quaternion.identity);
        asteroid.GetComponent<Asteroid>().SetAsteroid(_asteroidSpeed, _asteroidDeath);
    }
    #endregion
}
