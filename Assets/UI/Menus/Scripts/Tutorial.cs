using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tutorialImages;

    private int currentIndex;

    public void NextImage()
    {
        if (currentIndex > 0)
        {
            tutorialImages[currentIndex - 1].SetActive(false);
        }

        if (currentIndex == tutorialImages.Length)
        {
            currentIndex = 0;
            gameObject.SetActive(false);
        }
        else
        {
            tutorialImages[currentIndex].SetActive(true);

            currentIndex++;
        }
    }
}
