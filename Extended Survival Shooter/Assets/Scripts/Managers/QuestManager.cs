using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{

    public Text questText;
    public Text questVerdict;
    public Text details;
    public Button SaveProgressButton;
    public Button ContinueWithoutSaving;
    public static int ZombunnyKilled = 0;
    public static int ZombearKilled = 0;
    public static int HellephantKilled = 0;
    public string nextScene;

    public float restartDelay = 3f;
    public Animator anim;
    float restartTimer;

    int currentQuest;

    // Start is called before the first frame update
    void Start()
    {
        if (MainManager.Instance != null)
        {
            currentQuest = MainManager.Instance.currentQuest;
            MainManager.Instance.isQuestOnGoing = true;
        }

        // Set max spawn for enemy
        switch (currentQuest)
        {
            case 1:
                EnemyManager.maxSpawn = 1;
                break;
            case 2:
                EnemyManager.maxSpawn = 2;
                break;
            case 3:
                EnemyManager.maxSpawn = 3;
                break;
            case 4:
                EnemyManager.maxSpawn = 4;
                break;
            default:
                break;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerHealth.isDead)
        {
            Transition("Quest Failed", "GameOver", "MainScene");
        }

        switch (currentQuest)
        {
            case 1:
                Quest1();
                break;
            case 2:
                Quest2();
                break;
            case 3:
                Quest3();
                break;
            case 4:
                Quest4();
                break;
            default:
                break;
        }
    }

    void Quest1()
    {
        int totalKill = ZombunnyKilled + ZombearKilled + HellephantKilled;
        questText.text = "Quest 1: Bunuh Zombunny, Zombear, dan Hellephant (" + totalKill.ToString() + "/3)";
        if (totalKill == 1)
        {
            MainManager.Instance.isQuestOnGoing = false;
            MainManager.Instance.currentQuest = 2;
            MainManager.Instance.nextScene = "TransitionQuest1";
            Transition("Quest 1 Completed", "QuestCompleted", "TransitionQuest1");
        }

    }

    void Quest2()
    {
        int totalKill = ZombunnyKilled + ZombearKilled + HellephantKilled;
        questText.text = "Quest 2: Bunuh Zombunny, Zombear, dan Hellephant (" + totalKill.ToString() + "/6)";
        if (totalKill == 1)
        {
            MainManager.Instance.isQuestOnGoing = false;
            MainManager.Instance.currentQuest = 3;
            MainManager.Instance.nextScene = "MainScene";
            Transition("Quest 2 Completed", "QuestCompleted", "MainScene");
        }
    }

    void Quest3()
    {
        int totalKill = ZombunnyKilled + ZombearKilled + HellephantKilled;
        questText.text = "Quest 3: Bunuh Zombunny, Zombear, dan Hellephant (" + totalKill.ToString() + "/9)";
        if (totalKill == 1)
        {
            MainManager.Instance.isQuestOnGoing = false;
            MainManager.Instance.currentQuest = 4;
            MainManager.Instance.nextScene = "MainScene";
            Transition("Quest 3 Completed", "QuestCompleted", "MainScene");
        }
    }

    void Quest4()
    {
        int totalKill = ZombunnyKilled + ZombearKilled + HellephantKilled;
        questText.text = "Quest 4: Bunuh Zombunny, Zombear, dan Hellephant (" + totalKill.ToString() + "/12)";
        if (totalKill == 1)
        {
            MainManager.Instance.isQuestOnGoing = false;
            questVerdict.text = "THE END";
            details.text = "Game Completion Time " + System.TimeSpan.FromSeconds((int)MainManager.Instance.currentPlayDuration).ToString("c");
            anim.SetTrigger("GameFinished");
            restartTimer += Time.deltaTime;
            if (restartTimer >= restartDelay)
            {
                Reset();
                Time.timeScale = 0f;
            }
        }
    }

    void Transition(string verdictText, string trigger, string continueToScene)
    {
        questVerdict.text = verdictText;
        anim.SetTrigger(trigger);
        restartTimer += Time.deltaTime;
        nextScene = continueToScene;

        if (restartTimer >= restartDelay)
        {
            Reset();
            Time.timeScale = 0f;
        }
    }

    private void Reset()
    {
        ZombearKilled = 0;
        ZombunnyKilled = 0;
        HellephantKilled = 0;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextScene);
    }

}
