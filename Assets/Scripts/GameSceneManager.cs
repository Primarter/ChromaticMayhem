using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Build Menu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Build Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void EndGame()
    {
        SceneManager.LoadScene("ThanksForPlaying");
    }
}