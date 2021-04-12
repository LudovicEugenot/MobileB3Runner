using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool gamePaused = true;

    public void PlayGame()
    {
        //SceneManager.UnloadSceneAsync("TitleMenuUI");
        Time.timeScale = 1f;
        gamePaused = true;
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
        //SceneManager.LoadScene("LTN_TestS.cene");
        //SceneManager.LoadScene("TitleMenuUI", LoadSceneMode.Additive);
        Time.timeScale = 0f;
        gamePaused = true;
    }
}
