using System.Collections;
using System.Collections.Generic;
using Knossos.Character;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    public static bool gameIsPaused = false;

    SelectionMenu selectionMenu;

    void Awake()
    {
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1f;
        selectionMenu = this.GetComponent<SelectionMenu>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        selectionMenu.enabled = true;
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        selectionMenu.enabled = false;
        Time.timeScale = 1f;
        InputManager.EmptyBuffer();
        gameIsPaused = false;
    }

    // public void MainMenu()
    // {
    //     pauseMenuUI.SetActive(false);
    //     Time.timeScale = 1f;
    //     gameIsPaused = false;
    // }
}
