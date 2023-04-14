using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // private string _name;
    // private float _volume;
    public Text NamePlaceholder;
    public Text InputtedName;

    // Start is called before the first frame update
    void Start()
    {
        // _volume = AudioListener.volume;
        // if (_name != "")
        // {
        //     nameField.text = _name;
        // }
        MainManager.Instance.LoadSettingsPreferences();
        NamePlaceholder.text = MainManager.Instance.playerName;
        InputtedName.text = MainManager.Instance.playerName;
    }

    // Update is called once per frame
    void Update()
    {
        // AudioListener.volume = _volume;
        // if (_name != "")
        // {
        //     nameField.text = _name;
        // }
    }

    public void UpdateName(string username)
    {
        // _name = username;
        MainManager.Instance.playerName = username;
        MainManager.Instance.SaveSettingsPreferences();
    }

    public void UpdateVolume(float volume)
    {
        AudioListener.volume = volume;
        MainManager.Instance.gameVolume = volume;
        MainManager.Instance.SaveSettingsPreferences();
    }
}
