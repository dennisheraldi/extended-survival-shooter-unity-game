using UnityEngine;
using UnityEngine.UI;
using System;


public class ScoreManager : MonoBehaviour
{
    

    Text text;

    void Awake ()
    {
        text = GetComponent <Text> ();
        
    }


    void Update ()
    {
        if (MainManager.Instance.isQuestOnGoing)
        {
            MainManager.Instance.currentPlayDuration += Time.deltaTime;
        } 

        text.text = "Time: " + TimeSpan.FromSeconds((int)MainManager.Instance.currentPlayDuration).ToString("c");
    }
}
