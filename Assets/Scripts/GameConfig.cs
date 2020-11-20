using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [Space] [Header("Ship Config")]
    public float ShipSpeed;
    public float ShipFireRate;
    public int ShipLivesCount;
    public int ShipDamage;
    
    [Space] [Header("Levels Config")]
    public int CountOfLevels;
    public GameObject[] AsteroidsPrefabs;
    public Vector2 AsteroidsSpeedRange;
    public Vector2 AsteroidsSpawnTimeRange;
    public Color[] AsteroidsColors;
    public Vector2Int ScoreToWinRange;
    public SpaceColor[] SpaceColors;
}

[Serializable]
public class SpaceColor
{
    public SpaceColor(Color topColor, Color bottomColor)
    {
        TopColor = topColor;
        BottomColor = bottomColor;
    }

    public Color TopColor;
    public Color BottomColor;
}