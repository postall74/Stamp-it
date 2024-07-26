using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GamePolygon;
public class MenuController : MonoBehaviour
{
    int CurrentLevel;
    public Text LevelText;
    public Text Soundtext;

    [Header("ABOUT")]
    public string MoreGamesURL;
    public string RateUsURL;
    
  
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
         CurrentLevel = PlayerPrefs.GetInt("Level", 0);
         LevelText.text = "LEVEL : " + (CurrentLevel);
        SoundManager.Instance.PlayMusic(SoundManager.Instance.Menu);



    }

    public void StartGame() {
        PlayButton();
        SceneManager.LoadScene("Game");
    }

    public void StartShop() {
        PlayButton();
        SceneManager.LoadScene("Shop");
    }

    public void MuteSound() {
        if (SoundManager.Instance.IsMusicOff())
        {
            Soundtext.text = "MUSIC ON";
        }
        else {
            Soundtext.text = "MUSIC OFF";

        }
        PlayButton();
        SoundManager.Instance.ToggleMusic();
        
    }

    public void OpenMoreGamePage() {
        PlayButton();
        Application.OpenURL(MoreGamesURL);
    }

    public void OpenRateUs() {
        PlayButton();
        Application.OpenURL(RateUsURL);
    }

    void PlayButton() {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

    }
}
