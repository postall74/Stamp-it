using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static void LoadGameScene()
    {
        SceneManager.LoadScene(Constants.GameSceneName);
    }

    public static void LoadMenuScene()
    {
        SceneManager.LoadScene(Constants.MenuSceneName);
    }

    public static void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}