using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score
{
    public string username;
    public float score;

    public Score(string username, float score)
    {
        this.username = username;
        this.score = score;
    }
}
