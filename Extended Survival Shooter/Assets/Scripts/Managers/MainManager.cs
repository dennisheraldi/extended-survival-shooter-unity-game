using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    [System.Serializable]
    public class SaveData
    {
        public int slotNumber;
        public string slotName;
        public int currentPlayerHealth;
        public int currentQuest;
        public int currentMoney;
        public float currentPlayDuration;
        public bool isQuestOnGoing;
        public string timeSaved;
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
        data.isQuestOnGoing = isQuestOnGoing;
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

        string fileName = "data_" + slotNumber.ToString() + "_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json";
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

            }
        }

        
    }


}

