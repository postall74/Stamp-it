using UnityEngine;
using UnityEngine.UI; // Добавлено для работы с Text
using System;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Texture2D[] _maps;
    [SerializeField] private ColorToPrefab[] _colorMappings;
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _floorCube;
    [SerializeField] private Color _floorColor;
    [SerializeField] private Text _levelText; // Требует UnityEngine.UI

    private int _currentLevelIndex;
    private GameObject _floor;

    private void Start()
    {
        _currentLevelIndex = PlayerPrefs.GetInt("Level", 0);
        _levelText.text = $"LEVEL: {_currentLevelIndex}";
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        CreateFloor();
        GenerateTiles();
    }

    private void CreateFloor()
    {
        _floor = new GameObject("Floor");
        var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.SetParent(_floor.transform);

        Texture2D currentMap = _maps[_currentLevelIndex];
        Vector3 floorPosition = new Vector3(
            currentMap.width / 2f - 0.5f,
            Constants.FloorHeight,
            currentMap.height / 2f - 0.5f
        );

        floor.transform.position = floorPosition;
        floor.transform.localScale = new Vector3(
            currentMap.width + 0.5f,
            0.5f,
            currentMap.height + 0.5f
        );

        floor.GetComponent<MeshRenderer>().material.color = _floorColor;
    }

    private void GenerateTiles()
    {
        Texture2D currentMap = _maps[_currentLevelIndex];

        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                GenerateTile(x, y, currentMap);
            }
        }
    }

    private void GenerateTile(int x, int y, Texture2D map)
    {
        Color pixelColor = map.GetPixel(x, y);
        if (pixelColor.a == 0) return;

        foreach (ColorToPrefab mapping in _colorMappings)
        {
            if (mapping.Color.Equals(pixelColor))
            {
                Vector3 position = new Vector3(x, 0, y);

                if (mapping.Prefab.CompareTag(Constants.EnemyTagName))
                {
                    _player.position = new Vector3(x, Constants.PlayerYPosition, y);
                }
                else
                {
                    Instantiate(mapping.Prefab, position, Quaternion.identity);
                }
            }
        }
    }

    public Vector2 GetLevelDimensions()
    {
        Texture2D currentMap = _maps[_currentLevelIndex];
        return new Vector2(currentMap.width, currentMap.height);
    }
}