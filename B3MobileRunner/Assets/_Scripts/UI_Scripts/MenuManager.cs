﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool gamePaused = true;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
    }

    public void PlayGame()
    {
        SceneManager.UnloadSceneAsync(ObjectsData.MainMenu);
        SceneManager.LoadScene(ObjectsData.MainGameScene);
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
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.UnloadSceneAsync("Milestone19_04");
        SceneManager.LoadScene("TitleMenuUI");
        Time.timeScale = 0f;
        gamePaused = true;
    }
}
