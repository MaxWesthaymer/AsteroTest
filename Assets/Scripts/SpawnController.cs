using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private GameObject asteroidPrefab;
    private Vector2 screenHalfSizeInWorldUnits;
    public float spawnRate;
    private float time;
    void Start()
    {
        Camera mainCamera = Camera.main;
        screenHalfSizeInWorldUnits = new Vector2(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize);
    }

    public void SetSpawner(float spawnTime, GameObject asteroid)
    {
        spawnRate = spawnTime;
        asteroidPrefab = asteroid;
    }

    // Update is called once per frame
    void Update()
    {
        if (time <= 0)
        {
            Vector2 spawnPoint = new Vector2( Random.Range(-screenHalfSizeInWorldUnits.x, screenHalfSizeInWorldUnits.x), screenHalfSizeInWorldUnits.y + 1f);
            Instantiate(asteroidPrefab, spawnPoint, Quaternion.identity);
            time = spawnRate;
        }
        time -= Time.deltaTime;
    }
}
