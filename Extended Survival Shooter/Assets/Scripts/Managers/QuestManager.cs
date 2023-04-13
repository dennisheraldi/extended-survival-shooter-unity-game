using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    PlayerHealth playerHealth;
    public Text questText;
    public Text questVerdict;
    public Text details;
    public Text questVerdictText;
    public Text moneyText;
    public Button SaveProgressButton;
    public Button ContinueWithoutSaving;
    public static int ZombunnyKilled = 0;
    public static int ZombearKilled = 0;
    public static int HellephantKilled = 0;
    public static int ZombunnyV2Killed = 0;
    public static int ZombearV2Killed = 0;
    public static int HellephantV2Killed = 0;
    public static int ClownKilled = 0;
    public string nextScene;
    public Animator anim;
 
    float restartDelay;
    float restartTimer;
    int currentQuest;

    //Timer materials
    public Text TimerTxt;
    //bool isTimerOn = false;
    public static float TimerQ3;
    float timeCount;

    // Start is called before the first frame update
    void Start()
    {
        restartDelay = 5f;
        restartTimer = 0;
        PlayerHealth.isDead = false;
        TimerQ3 = 5f;
        

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
            Transition("Quest Failed", "GameOver", "MainScene", 0);
            TimerTxt.text = "";
        }
        else
        {
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
      
    }

    void Quest1()
    {
       
        int totalKill = ZombunnyKilled + ZombearKilled + HellephantKilled;
        questText.text = "Quest 1: Bunuh Zombunny, Zombear, dan Hellephant (" + totalKill.ToString() + "/3)";
        moneyText.text = "Money: " + MainManager.Instance.currentMoney.ToString();
        TimerTxt.text = "";
        if (totalKill == 3)
        {
            MainManager.Instance.isQuestOnGoing = false;
            MainManager.Instance.currentQuest = 2;
            MainManager.Instance.nextScene = "TransitionQuest1";
            questVerdictText.text = "Reward: +200 Money";
            Transition("Quest 1 Completed", "QuestCompleted", "TransitionQuest1", 200);
        }

    }

    void Quest2()
    {
        int totalKill = ZombunnyKilled + ZombearKilled + HellephantKilled;
        questText.text = "Quest 2: Bunuh Zombunny, Zombear, dan Hellephant (" + totalKill.ToString() + "/6)";
        moneyText.text = "Money: " + MainManager.Instance.currentMoney.ToString();
        TimerTxt.text = "";
        if (totalKill == 6)
        {
            MainManager.Instance.isQuestOnGoing = false;
            MainManager.Instance.currentQuest = 3;
            MainManager.Instance.nextScene = "MainScene";
            questVerdictText.text = "Reward: +500 Money";
            Transition("Quest 2 Completed", "QuestCompleted", "TransitionQuest2", 500);
        }
    }

    void Quest3()
    {
        timeCount = Time.deltaTime;
        TimerQ3 -= timeCount;
        TimerTxt.text = "Zombie Membuatmu sesak! \n Waktu tersisa : " + TimerQ3.ToString("0.00");
        int totalKill = ZombunnyV2Killed + ZombearV2Killed + HellephantV2Killed;
        questText.text = "Quest 3: Bunuh Zombies yang ada (" + totalKill.ToString() + "/9)";
        moneyText.text = "Money: " + MainManager.Instance.currentMoney.ToString();

        if (totalKill == 9)
        {
            MainManager.Instance.isQuestOnGoing = false;
            MainManager.Instance.currentQuest = 4;
            MainManager.Instance.nextScene = "MainScene";
            questVerdictText.text = "Reward: +1000 Money";
            Transition("Quest 3 Completed", "QuestCompleted", "MainScene", 1000);
        }
    }

    void Quest4()
    {
        int totalKill = ClownKilled;
        questText.text = "Quest 3: Bunuh Clown (" + totalKill.ToString() + "/1)";
        moneyText.text = "Money: " + MainManager.Instance.currentMoney.ToString();
        if (totalKill == 1)
        {
            MainManager.Instance.isQuestOnGoing = false;
            MainManager.Instance.currentQuest = 5;
            MainManager.Instance.nextScene = "MainScene";
            questVerdictText.text = "Reward: +1000 Money";
            Transition("Quest 4 Completed", "QuestCompleted", "MainScene", 1000);
        }
    }

    void Quest5()
    {
        int totalKill = ZombunnyKilled + ZombearKilled + HellephantKilled;
        questText.text = "Quest 4: Bunuh Zombunny, Zombear, dan Hellephant (" + totalKill.ToString() + "/12)";
        moneyText.text = "Money: " + MainManager.Instance.currentMoney.ToString();
        if (totalKill == 12)
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

    void Transition(string verdictText, string trigger, string continueToScene, int obtainedMoney)
    {
        questVerdict.text = verdictText;
        anim.SetTrigger(trigger);
        restartTimer += Time.deltaTime;
        nextScene = continueToScene;
        if (trigger == "GameOver" && (restartDelay - restartTimer) > 1)
        {
            questVerdictText.text = "Back to main menu in " + (restartDelay - restartTimer).ToString("0") + " seconds";
        }
        
        if (trigger == "GameOver")
        {
            if (restartTimer >= restartDelay)
            {
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #else
		            Application.Quit();
                #endif
                
            }
        } else if(trigger == "QuestCompleted")
        {
            StartCoroutine(UpdatePhase(obtainedMoney));
        }
    }

    IEnumerator UpdatePhase(int obtainedMoney)
    {
        // Update money
        yield return new WaitForSeconds(1);
        MainManager.Instance.currentMoney += obtainedMoney;
        Reset();
        Time.timeScale = 0f;
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

    public void LoadLatestSavedData()
    {
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
        string json = File.ReadAllText(filePaths[2]);
        MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);
        MainManager.Instance.LoadQuestProgress(data.slotNumber);
        Reset();
        SceneManager.LoadScene("MainScene");
    }
}
