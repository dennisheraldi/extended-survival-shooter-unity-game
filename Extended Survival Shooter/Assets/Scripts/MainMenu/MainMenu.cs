using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text PlayerLabel;

    public void Update()
    {
        PlayerLabel.text = "Welcome back, " + PlayerDataLoader.playerName + "!";
    }

    public void NewGame()
    {
        SceneManager.LoadScene("OpeningScene");
    }

    public void GoToScoreboard()
    {
        SceneManager.LoadScene("Scoreboard");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
