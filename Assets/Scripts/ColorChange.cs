using UnityEngine;

public class ColorChange : MonoBehaviour
{
    [SerializeField] private Color[] _colors;

    private void Start()
    {
        SetRandomColor();
    }

    private void SetRandomColor()
    {
        int randomColorIndex = Random.Range(0, _colors.Length);
        GetComponent<MeshRenderer>().material.color = _colors[randomColorIndex];
    }
}