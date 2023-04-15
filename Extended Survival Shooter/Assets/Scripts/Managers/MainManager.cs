using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static MainManager Instance;

    public int currentPlayerHealth;
    public int currentQuest;
    public int currentMoney;
    public float currentPlayDuration;
    public float gameVolume;
    public bool isQuestOnGoing;
    public string nextScene;
    public bool immunity;
    public bool instantKill;
    public string playerName;

    public string currentPet;

    public int currentPetHealth;

    private void Awake()
    {
        currentPet = "";
        currentPetHealth = 100;
        currentPlayerHealth = 100;
        currentMoney = 1000;
        currentQuest = 1;
        currentPlayDuration = 0;
        immunity = false;
        isQuestOnGoing = false;
        playerName = PlayerPrefs.GetString("PlayerName");
        gameVolume = PlayerPrefs.GetFloat("GameVolume");
        instantKill = false;
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

    [System.Serializable]
    public class SaveData
    {
        public int slotNumber;
        public string slotName;
        public int currentPlayerHealth;
        public int currentQuest;
        public int currentMoney;
        public float currentPlayDuration;
        public float gameVolume;
        public bool isQuestOnGoing;
        public string playerName;
        public string timeSaved;
        public string currentPet;
        public int currentPetHealth;
    }

    public void SaveQuestProgress(int slotNumber, string slotName)
    {
        SaveData data = new SaveData();
        data.slotNumber = slotNumber;
        data.slotName = slotName;
        data.timeSaved = System.DateTime.Now.ToString("F");
        data.currentPlayerHealth = currentPlayerHealth;
        data.currentQuest = currentQuest;
        data.currentMoney = currentMoney;
        data.currentPlayDuration = currentPlayDuration;
        data.gameVolume = gameVolume;
        data.isQuestOnGoing = isQuestOnGoing;
        data.playerName = playerName;
        data.currentPet = currentPet;
        data.currentPetHealth = currentPetHealth;
        string json = JsonUtility.ToJson(data);

        // Mencari data terlebih dahulu kemudian dihapus ketika ditemukan
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
        foreach (string foundFilePath in filePaths)
        {
            if (Path.GetFileName(foundFilePath).Contains("data_"+slotNumber.ToString()))
            {
                File.Delete(foundFilePath);
            }
        }

        string fileName = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_data_" + slotNumber.ToString() + ".json";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(filePath, json);

    }

    public void LoadQuestProgress(int slotNumber)
    {
        // Read data from external files
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
        foreach (string foundFilePath in filePaths)
        {
            if (Path.GetFileName(foundFilePath).Contains("data_"+ slotNumber.ToString()))
            {
                string json = File.ReadAllText(foundFilePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                // Assign the save data to MainManager Instances
                currentPlayerHealth = data.currentPlayerHealth;
                currentQuest = data.currentQuest;
                currentMoney = data.currentMoney;
                currentPlayDuration = data.currentPlayDuration;
                gameVolume = data.gameVolume;
                isQuestOnGoing = data.isQuestOnGoing;
                playerName = data.playerName;
                currentPet = data.currentPet;
                currentPetHealth = data.currentPetHealth;
            }
        }
    }

    public void LoadGameByQuest()
    {
        if (MainManager.Instance.currentQuest == 1 || MainManager.Instance.currentQuest == 2)
        {
            SceneManager.LoadScene("MainScene");
        }
        else if (MainManager.Instance.currentQuest == 3)
        {
            SceneManager.LoadScene("Quest3");
        }
        else if (MainManager.Instance.currentQuest == 4)
        {
            SceneManager.LoadScene("Quest4");
        }
    }
}

