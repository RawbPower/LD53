using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    private void Start()
    {
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene >= SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        SceneManager.LoadScene(nextScene);
    }

    public void SettingsMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void HowToPlay()
    {
        SceneManager.LoadScene(10);
    }

    public void DriveScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadingScene()
    {
        if (GameManager.instance != null && GameManager.instance.IsValid())
        {
            SceneManager.LoadScene(GameManager.instance.GetCurrentVanScene());
        }
        else
        {
            SceneManager.LoadScene(4);
        }
    }

    public void ResetGame()
    {
        if (GameManager.instance != null && GameManager.instance.IsValid())
        {
            GameManager.instance.RestProgress();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
