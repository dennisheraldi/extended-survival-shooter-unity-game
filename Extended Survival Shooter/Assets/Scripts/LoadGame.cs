using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    public Dropdown m_Dropdown;
    int m_Index;
    public Text DropdownLabel;
    public Button LoadGameButton;

    //Use these for adding options to the Dropdown List
    Dropdown.OptionData m_NewData1, m_NewData2, m_NewData3;
    //The list of messages for the Dropdown
    List<Dropdown.OptionData> m_Messages = new List<Dropdown.OptionData>();

    // Start is called before the first frame update
    void Start()
    {
        m_Dropdown.ClearOptions();
        // Read data from external files
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
        foreach (string foundFilePath in filePaths)
        {
            if (Path.GetFileName(foundFilePath).Contains("data_0"))
            {
                string json = File.ReadAllText(foundFilePath);
                MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);
                m_NewData1 = new Dropdown.OptionData();
                m_NewData1.text = data.slotName;
                m_Messages.Add(m_NewData1);
            }
            else if (Path.GetFileName(foundFilePath).Contains("data_1"))
            {
                string json = File.ReadAllText(foundFilePath);
                MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);
                m_NewData2 = new Dropdown.OptionData();
                m_NewData2.text = data.slotName;
                m_Messages.Add(m_NewData2);
            }
            else if (Path.GetFileName(foundFilePath).Contains("data_2"))
            {
                string json = File.ReadAllText(foundFilePath);
                MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);
                m_NewData3 = new Dropdown.OptionData();
                m_NewData3.text = data.slotName;
                m_Messages.Add(m_NewData3);
            }
        }

        if (filePaths.Length == 0)
        {
            DropdownLabel.text = "None";
            m_Dropdown.interactable = false;
            LoadGameButton.interactable = false;
        }
        else
        {
            //Take each entry in the message List
            foreach (Dropdown.OptionData message in m_Messages)
            {
                //Add each entry to the Dropdown
                m_Dropdown.options.Add(message);
                //Make the index equal to the total number of entries
                m_Index = m_Messages.Count - 1;
            }
            DropdownLabel.text = m_Dropdown.options[m_Dropdown.value].text;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLoadGame()
    {
        MainManager.Instance.LoadQuestProgress(m_Dropdown.value);
        MainManager.Instance.LoadGameByQuest();
    }

}
