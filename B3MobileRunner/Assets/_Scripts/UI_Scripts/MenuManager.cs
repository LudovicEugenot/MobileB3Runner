﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool gamePaused = true;

    public void PlayGame()
    {
        SceneManager.UnloadSceneAsync("TitleMenuUI");
        SceneManager.LoadScene("LTN_TestScene");
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        //SceneManager.LoadScene("PauseMenuScene", LoadSceneMode.Additive);
        gamePaused = true;
    }

    public void Resume()
    {
        //SceneManager.UnloadSceneAsync("PauseMenuScene");
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void ExitGame()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        //SceneManager.UnloadSceneAsync("LTN_TestScene");
        SceneManager.LoadScene("TitleMenuUI");
        Time.timeScale = 0f;
        gamePaused = true;
    }
}
