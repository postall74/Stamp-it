using UnityEngine;

[System.Serializable]
public class ColorToPrefab
{
    [SerializeField] private Color _color;
    [SerializeField] private GameObject _prefab;

    public Color Color => _color;
    public GameObject Prefab => _prefab;
}