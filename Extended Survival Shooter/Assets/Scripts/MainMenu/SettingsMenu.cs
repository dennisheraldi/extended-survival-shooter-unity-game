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
    public Slider VolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        NamePlaceholder.text = PlayerDataLoader.playerName;
        InputtedName.text = PlayerDataLoader.playerName;
        VolumeSlider.value = PlayerDataLoader.gameVolume;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void UpdateName(string name)
    {
        PlayerDataLoader.playerName = name;
        PlayerPrefs.SetString("PlayerName", PlayerDataLoader.playerName);
    }

    public void UpdateVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerDataLoader.gameVolume = volume;
        VolumeSlider.value = PlayerDataLoader.gameVolume;
        PlayerPrefs.SetFloat("GameVolume", PlayerDataLoader.gameVolume);
    }
}
