using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardMenu : MonoBehaviour
{
    public ScoreEntry scoreEntry;
    public ScoreUtility scoreUtility;

    private const int LeaderboardLimit = 8;
    private int _leaderboardSize;

    // Start is called before the first frame update
    void Start()
    {
        var scores = scoreUtility.GetSortedScores().ToArray();
        _leaderboardSize = LeaderboardLimit < scores.Length ? LeaderboardLimit : scores.Length;
        for (var i = 0; i < _leaderboardSize; i++)
        {
            var entry = Instantiate(scoreEntry, transform).GetComponent<ScoreEntry>();
            entry.rank.text = (i + 1).ToString();
            entry.username.text = scores[i].username;
            entry.score.text = scores[i].score.ToString();
        }
    }

    public void ShowMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
