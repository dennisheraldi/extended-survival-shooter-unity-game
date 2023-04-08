using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveProgress : MonoBehaviour
{

    public Dropdown SlotChoice;
    public InputField CustomizeSlotName;
    public GameObject SaveProgressPanel;


    // Start is called before the first frame update
    void Start() 
    {
        SlotChoice.options[0].text = "Slot 1";
        SlotChoice.options[1].text = "Slot 2";
        SlotChoice.options[2].text = "Slot 3";
        CustomizeSlotName.text = SlotChoice.options[SlotChoice.value].text;

        // Read data from external files
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
        foreach (string foundFilePath in filePaths)
        {
            if (Path.GetFileName(foundFilePath).Contains("data_0"))
            {
                string json = File.ReadAllText(foundFilePath);
                MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);
                SlotChoice.options[0].text = data.slotName;
            } else if (Path.GetFileName(foundFilePath).Contains("data_1"))
            {
                string json = File.ReadAllText(foundFilePath);
                MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);
                SlotChoice.options[1].text = data.slotName;
            } else if (Path.GetFileName(foundFilePath).Contains("data_2"))
            {
                string json = File.ReadAllText(foundFilePath);
                MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);
                SlotChoice.options[2].text = data.slotName;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropdownValueChanged()
    {
        CustomizeSlotName.text = SlotChoice.options[SlotChoice.value].text;
    }

    public void SaveButtonToSaveProgressPanel()
    {
        SaveProgressPanel.SetActive(true);
    }

    public void BackButton()
    {
        SaveProgressPanel.SetActive(false);
    }

    public void SaveAndContinue()
    {
        MainManager.Instance.SaveQuestProgress(SlotChoice.value, CustomizeSlotName.text);
    }
}
