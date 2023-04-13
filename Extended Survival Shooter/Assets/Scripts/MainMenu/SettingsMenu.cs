using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public InputField nameField;
    
    private string _name;
    private float _volume;
    // Start is called before the first frame update
    void Start()
    {
        _volume = AudioListener.volume;
        if (_name != "")
        {
            nameField.text = _name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = _volume;
        if (_name != "")
        {
            nameField.text = _name;
        }
    }

    public void UpdateName(string username)
    {
        _name = username;
    }

    public void UpdateVolume(float volume)
    {
        _volume = volume;
    }
}
