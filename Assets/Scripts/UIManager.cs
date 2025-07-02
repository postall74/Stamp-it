using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera _gameCamera;
    [SerializeField] private CinemachineVirtualCamera _endCamera;

    [Header("UI Elements")]
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _levelCompleteUI;
    [SerializeField] private Text _movesText;
    [SerializeField] private Text _winMovesText;

    private int _moveCount;

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

    public void SwitchToEndCamera()
    {
        _endCamera.Priority = 10;
        _gameCamera.Priority = 0;
    }

    public void SwitchToGameCamera()
    {
        _gameCamera.Priority = 10;
        _endCamera.Priority = 0;
    }

    public void IncrementMoveCount()
    {
        _moveCount++;
        _movesText.text = $"MOVES: {_moveCount}";
    }

    public void ShowLevelComplete()
    {
        _winMovesText.text = $"MOVES: {_moveCount}";
        _levelCompleteUI.SetActive(true);
        SoundManager.Instance.PlaySound(SoundManager.Instance.Finish);
        SoundManager.Instance.PlayMusic(SoundManager.Instance.GameWin);
    }

    public void ShowGameOver()
    {
        _gameOverUI.SetActive(true);
    }

    public void HideGameUI()
    {
        _gameUI.SetActive(false);
    }
}