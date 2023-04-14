using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour
{

	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;

	public GameObject Panel;

	void Start()
	{
		
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (Panel.activeSelf) {
				Panel.SetActive(false);
			} else {
				Panel.SetActive(true);
			} 
			Pause();
		}
	}

	public void Pause()
	{
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		Lowpass();
	}

	void Lowpass()
	{
		if (Time.timeScale == 0)
		{
			paused.TransitionTo(.01f);
		}

		else

		{
			unpaused.TransitionTo(.01f);
		}
	}

	public void BackToMainMenu()
    {
		Time.timeScale = 1;
		Destroy(MainManager.Instance.gameObject);
		SceneManager.LoadScene("MainMenuScene");
    }

	/*public void Quit()
	{
		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}*/
}
