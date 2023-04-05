using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    public Animator transition;
    public PlayableDirector director;

    public float transitionTime = 6f;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    LoadNextLevel();
        //}
        
        if (director.duration - director.time < 4)
        {
            LoadNextLevel();
        }

    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel("MainScene"));
    }

    IEnumerator LoadLevel(string SceneName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneName);
    }
}
