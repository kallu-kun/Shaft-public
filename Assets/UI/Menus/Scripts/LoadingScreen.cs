using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using TMPro;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	public RawImage loadingImage;
	private AsyncOperation loadMainLevel;

	void Start()
	{
		// StartCoroutine(Loading());
		StartCoroutine(LoadLevelAsync(3));
	}

	private void Update()
	{
		loadingImage.transform.Rotate(new Vector3(0, 0, -100) * Time.deltaTime);
	}

	/*
	IEnumerator Loading()
	{
		while (true)
		{		
			foreach (var txt in text)
			{
				yield return new WaitForSeconds(0.3f);
				txt.gameObject.SetActive(true);
			}

			yield return new WaitForSeconds(0.3f);

			foreach (var txt in text)
			{
				txt.gameObject.SetActive(false);
			}
		}
	}
	*/

	IEnumerator LoadLevelAsync(int sceneIndex)
	{
		loadMainLevel = SceneManager.LoadSceneAsync(sceneIndex);

		loadMainLevel.allowSceneActivation = true;

		yield return null;
	}

	/*
	public void NextTutorialImage()
	{
		if (currentIndex == tutorialImages.Length - 1)
		{
			tutorialImages[0].SetActive(true);
			tutorialImages[tutorialImages.Length - 1].SetActive(false);
			currentIndex = 0;
		}
		else
		{
			tutorialImages[currentIndex].SetActive(false);
			tutorialImages[currentIndex + 1].SetActive(true);
			currentIndex++;
		}
	}
	*/
}
