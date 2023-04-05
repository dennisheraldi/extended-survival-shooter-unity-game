using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour
{

	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;
	public Text QuestText;

	public Canvas canvas;

	void Start()
	{
		canvas = GetComponent<Canvas>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			canvas.enabled = !canvas.enabled;
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
			QuestText.CrossFadeAlpha(0, 0, true);
		}

		else

		{
			unpaused.TransitionTo(.01f);
			QuestText.CrossFadeAlpha(1, 0, true);
		}
	}

	public void Quit()
	{
		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
