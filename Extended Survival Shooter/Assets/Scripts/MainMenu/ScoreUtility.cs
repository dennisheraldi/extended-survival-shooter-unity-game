using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUtility : MonoBehaviour
{
    private ScoreData _scoreData;

    private void Awake()
    {
        var scoreJson = PlayerPrefs.GetString("scores", "{}");
        _scoreData = JsonUtility.FromJson<ScoreData>(scoreJson);
    }

    private void OnDestroy()
    {
        SaveScore();
    }

    public IEnumerable<Score> GetSortedScores()
    {
        return _scoreData.scores.OrderBy(x => x.score);
    }

    public void AddScore(Score score)
    {
        _scoreData.scores.Add(score);
    }

    private void SaveScore()
    {
        var scoreJson = JsonUtility.ToJson(_scoreData);
        PlayerPrefs.SetString("scores", scoreJson);
    }
}
