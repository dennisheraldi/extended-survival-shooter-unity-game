using UnityEngine;
using UnityEngine.UI;
using System;


public class ScoreManager : MonoBehaviour
{
    public static int score;

    private float startTime;

    public PlayerHealth playerHealth;

    Text text;


    void Awake ()
    {
        text = GetComponent <Text> ();
        score = 0;
        startTime = playerHealth.startTime;
    }


    void Update ()
    {
        float currentTime = Time.time - startTime;

        if (playerHealth.isDead)
        {
            currentTime = playerHealth.deathTime - startTime;
        }
        
        text.text = "Time: " + TimeSpan.FromSeconds((int)currentTime).ToString("c");
    }
}
