using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
	private bool gameIsPaused;
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			gameIsPaused = !gameIsPaused;
		}

		if (gameIsPaused)
		{
			PauseGame();
		}
		else
		{
			ResumeGame();
		}
	}

	public void LoadScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}

	public void PauseGame()
	{
		Time.timeScale = 0;
	}

	public void ResumeGame()
	{
		Time.timeScale = 1;
		gameObject.SetActive(false);
	}

	public void Disable()
	{
		gameIsPaused = false;
		gameObject.SetActive(false);
	}
}
