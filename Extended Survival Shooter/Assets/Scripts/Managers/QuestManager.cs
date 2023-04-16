using System;
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

    public Text shopkeeperInfoText;
    public Button SaveProgressButton;
    public Button ContinueWithoutSaving;
    public Button RestartButton;
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

    bool isMoneyPaid = false;

    // Shopkeeper phase timer
    float shopkeeperDelay = 10f;
    float shopkeeperTimer = 0;

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
        TimerQ3 = 50f;
        

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
            // Give player money
            if (!isMoneyPaid)
            {
                if (!MainManager.Instance.infMoney)
                {
                    MainManager.Instance.currentMoney += 500;
                }
                isMoneyPaid = true;
            }

            if (shopkeeperTimer < shopkeeperDelay){
                shopkeeperInfoText.gameObject.SetActive(true);
            }
            shopkeeperTimer += Time.deltaTime;
            shopkeeperInfoText.text = "You have "+ (shopkeeperDelay-shopkeeperTimer).ToString("0") + " seconds to go to the shop before proceeding to the next quest";
            if (shopkeeperTimer >= shopkeeperDelay)
            {
                shopkeeperInfoText.gameObject.SetActive(false);
                MainManager.Instance.currentQuest = 2;
                MainManager.Instance.nextScene = "TransitionQuest1";
                questVerdictText.text = "Reward: +500 Money";
                Transition("Quest 1 Completed", "QuestCompleted", "TransitionQuest1ToQuest2");
            }
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

            // Give player money
            if (!isMoneyPaid)
            {
                if (!MainManager.Instance.infMoney)
                {
                    MainManager.Instance.currentMoney += 1000;
                }
                isMoneyPaid = true;
            }

            if (shopkeeperTimer < shopkeeperDelay){
                shopkeeperInfoText.gameObject.SetActive(true);
            }
            shopkeeperTimer += Time.deltaTime;
            shopkeeperInfoText.text = "You have "+ (shopkeeperDelay-shopkeeperTimer).ToString("0") + " seconds to go to the shop before proceeding to the next quest";
            if (shopkeeperTimer >= shopkeeperDelay)
            {
                shopkeeperInfoText.gameObject.SetActive(false);
                MainManager.Instance.isQuestOnGoing = false;
                MainManager.Instance.currentQuest = 3;
                MainManager.Instance.nextScene = "MainScene";
                questVerdictText.text = "Reward: +1000 Money";
                Transition("Quest 2 Completed", "QuestCompleted", "Quest3");
            } 
        }
    }

    void Quest3()
    {
        TimerTxt.gameObject.SetActive(true);
        timeCount = Time.deltaTime;
        TimerQ3 -= timeCount;
        TimerTxt.text = "Zombie Membuatmu sesak! \n Waktu tersisa : " + TimerQ3.ToString("0.00");
        int totalKill = ZombunnyV2Killed + ZombearV2Killed + HellephantV2Killed;
        questText.text = "Quest 3: Bunuh Zombies yang ada (" + totalKill.ToString() + "/9)";
        moneyText.text = "Money: " + MainManager.Instance.currentMoney.ToString();

        if (totalKill == 9)
        {
            MainManager.Instance.isQuestOnGoing = false;

            if (!isMoneyPaid)
            {
                if (!MainManager.Instance.infMoney)
                {
                    MainManager.Instance.currentMoney += 1500;
                }
                isMoneyPaid = true;
            }

            if (shopkeeperTimer < shopkeeperDelay){
                shopkeeperInfoText.gameObject.SetActive(true);
            }
            shopkeeperTimer += Time.deltaTime;
            shopkeeperInfoText.text = "You have "+ (shopkeeperDelay-shopkeeperTimer).ToString("0") + " seconds to go to the shop before proceeding to the next quest";
            if (shopkeeperTimer >= shopkeeperDelay)
            {
                MainManager.Instance.isQuestOnGoing = false;
                MainManager.Instance.currentQuest = 4;
                MainManager.Instance.nextScene = "MainScene";
                questVerdictText.text = "Reward: +1500 Money";
                TimerTxt.gameObject.SetActive(false);
                Transition("Quest 3 Completed", "QuestCompleted", "TransitionQuest3ToQuest4");
            } 
        }
    }

    void Quest4()
    {
        TimerTxt.gameObject.SetActive(false);
        int totalKill = ClownKilled;
        questText.text = "Quest 4: Bunuh Clown (" + totalKill.ToString() + "/1)";
        moneyText.text = "Money: " + MainManager.Instance.currentMoney.ToString();
        if (totalKill == 1)
        {
            MainManager.Instance.isQuestOnGoing = false;
            MainManager.Instance.nextScene = "EndingScene";
            ScoreUtility.AddScore(new Score(PlayerPrefs.GetString("PlayerName"), MainManager.Instance.currentPlayDuration));
            Reset();
            Destroy(MainManager.Instance.gameObject);
            SceneManager.LoadScene("EndingScene");
        }
    }


    void Transition(string verdictText, string trigger, string continueToScene)
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
                Reset();
                Destroy(MainManager.Instance.gameObject);
                SceneManager.LoadScene("MainMenuScene");
                
            }
        } else if(trigger == "QuestCompleted")
        {
            StartCoroutine(UpdatePhase());
        }
    }

    IEnumerator UpdatePhase()
    {
        // Update money
        yield return new WaitForSeconds(1);
        Reset();
        Time.timeScale = 0f;
        shopkeeperInfoText.gameObject.SetActive(false);
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
        string latestSavedData = "";
        foreach (string filePathName in filePaths)
        {
            if (Path.GetFileName(filePathName).Contains("data_"))
            {
                latestSavedData = filePathName;
            }
        }

        if (latestSavedData != "")
        {
            string json = File.ReadAllText(latestSavedData);
            MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);
            MainManager.Instance.LoadQuestProgress(data.slotNumber);
            Reset();
            MainManager.Instance.LoadGameByQuest();
        } else
        {
            RestartButton.interactable = false;
        }
        
        Reset();

    }
}
