using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _soundText;
    [SerializeField] private string _moreGamesUrl;
    [SerializeField] private string _rateUsUrl;

    private void Start()
    {
        Application.targetFrameRate = 60;
        int currentLevel = PlayerPrefs.GetInt("Level", 0);
        _levelText.text = $"LEVEL: {currentLevel}";
        SoundManager.Instance.PlayMusic(SoundManager.Instance.Menu);
        UpdateSoundButtonText();
    }

    public void StartGame()
    {
        PlayButtonSound();
        LevelManager.LoadGameScene();
    }

    public void ToggleSound()
    {
        PlayButtonSound();
        SoundManager.Instance.ToggleMusic();
        UpdateSoundButtonText();
    }

    private void UpdateSoundButtonText()
    {
        _soundText.text = SoundManager.Instance.IsMusicOff ? "MUSIC OFF" : "MUSIC ON";
    }

    public void OpenMoreGames()
    {
        PlayButtonSound();
        Application.OpenURL(_moreGamesUrl);
    }

    public void RateGame()
    {
        PlayButtonSound();
        Application.OpenURL(_rateUsUrl);
    }

    private void PlayButtonSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.Button);
    }
}