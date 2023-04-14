using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score
{
    public string username;
    public int score;

    public Score(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
}
