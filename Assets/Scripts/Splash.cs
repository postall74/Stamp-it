using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private float _delay = 2f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(Constants.MenuSceneName);
    }
}