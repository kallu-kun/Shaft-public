using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	[SerializeField]
	private Tutorial tutorial;
    [SerializeField]
    private ScoreInput scoreInput;

	public void StartGame()
	{
		SceneManager.LoadScene("Loading Screen");
	}

	public void StartGameOnePlayer()
	{
		Time.timeScale = 1f;
		PlayerSelect.SetOnePlayerMode();
		SceneManager.LoadScene("Loading Screen");
	}

	public void StartGameTwoPlayer()
	{
		Time.timeScale = 1f;
		PlayerSelect.SetTwoPlayerMode();
		SceneManager.LoadScene("Loading Screen");
	}

	public void LoadScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}

    public void MenuScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

	public void ActivateTutorial()
	{
		tutorial.gameObject.SetActive(true);
		tutorial.NextImage();
	}

	public void ExitGame()
	{
		Application.Quit(0);
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
		gameObject.SetActive(false);
	}

    public void DisableScoreInput()
    {
        scoreInput.gameObject.SetActive(false);
    }
}