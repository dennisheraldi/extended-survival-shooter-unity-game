using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    public Dropdown SlotChoice;
    public Text DropdownLabel;
    public Button LoadGameButton;

    // Start is called before the first frame update
    void Start()
    {
        // Read data from external files
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
        foreach (string foundFilePath in filePaths)
        {
            if (Path.GetFileName(foundFilePath).Contains("data_0"))
            {
                string json = File.ReadAllText(foundFilePath);
                MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);
                SlotChoice.options[0].text = data.slotName;
            }
            else if (Path.GetFileName(foundFilePath).Contains("data_1"))
            {
                string json = File.ReadAllText(foundFilePath);
                MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);
                SlotChoice.options[1].text = data.slotName;
            }
            else if (Path.GetFileName(foundFilePath).Contains("data_2"))
            {
                string json = File.ReadAllText(foundFilePath);
                MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);
                SlotChoice.options[2].text = data.slotName;
            }
        }

        DropdownLabel.text = SlotChoice.options[SlotChoice.value].text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLoadGame()
    {
        MainManager.Instance.LoadQuestProgress(SlotChoice.value);
        SceneManager.LoadScene("MainScene");
    }

}
