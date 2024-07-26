using UnityEngine.UI;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using GamePolygon;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("CineMachine Camera")]
    public CinemachineVirtualCamera vCam1;
    public CinemachineVirtualCamera vCam2;

    public GameObject GameUIPanel;
    public GameObject GameOverUI;
    public Text totlaMoves, GameWinTotalMoves;
    int CurrentLevel;
    int moves;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameStartCam();
        SoundManager.Instance.PlayMusic(SoundManager.Instance.Menu);
        CurrentLevel = PlayerPrefs.GetInt("Level", 0);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }

    public void GameEndCam()
    {

        vCam2.Priority = 1;
        vCam1.Priority = 0;

    }

    public void GameStartCam()
    {
        vCam2.Priority = 0;
        vCam1.Priority = 1;
    }

    public void LevelCompleted()
    {
        GameWinTotalMoves.text = "Total moves <RT>" + moves;
        PlayerPrefs.SetInt("Level", (CurrentLevel + 1));
        GameUIPanel.SetActive(true);
        SoundManager.Instance.PlaySound(SoundManager.Instance.Finish);
        SoundManager.Instance.PlayMusic(SoundManager.Instance.GameWin);
        //AdMobManager._AdMobInstance.showInterstitial();


    }

    public void ShowGameOver()
    {
        GameOverUI.SetActive(true);
        //AdMobManager._AdMobInstance.showInterstitial();

    }


    public void Continue()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
        SceneManager.LoadScene("Game");
        //AdMobManager._AdMobInstance.showInterstitial();
    }

    public void GoTOMenu()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
        SceneManager.LoadScene("Menu");
        //AdMobManager._AdMobInstance.showInterstitial();
    }

    public void TotalMove()
    {
        moves++;
        totlaMoves.text = "Total moves " + moves;
    }

    public void TryAgain()
    {
        CurrentLevel = PlayerPrefs.GetInt("Level", 0);
        PlayerPrefs.SetInt("Level", (CurrentLevel - 1));
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
        SceneManager.LoadScene("Game");
    }
}
