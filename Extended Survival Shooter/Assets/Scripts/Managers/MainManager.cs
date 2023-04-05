using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static MainManager Instance;

    public int currentPlayerHealth;
    public int currentQuest;
    public int currentMoney;
    public float currentPlayDuration;
    public bool isQuestOnGoing;

    private void Awake()
    {
        currentPlayerHealth = 100;
        currentMoney = 0;
        currentQuest = 1;
        currentPlayDuration = 0;
        isQuestOnGoing = false;
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
