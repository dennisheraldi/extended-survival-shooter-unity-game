using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("OpeningScene");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("LoadGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
