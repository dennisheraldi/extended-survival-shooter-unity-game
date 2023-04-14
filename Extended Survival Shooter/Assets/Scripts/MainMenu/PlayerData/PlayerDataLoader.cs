using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataLoader : MonoBehaviour
{
    public static string playerName;
    public static float gameVolume;

    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerName = PlayerPrefs.GetString("PlayerName");
        } else
        {
            playerName = "Player";
        }

        if (PlayerPrefs.HasKey("GameVolume"))
        {
            gameVolume = PlayerPrefs.GetFloat("GameVolume");
        }
        else
        {
            gameVolume = 100;
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
