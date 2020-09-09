using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        string otherTag = other.tag;

        if (otherTag == "Player" || otherTag == "Train" || otherTag == "ResourceCart" || otherTag == "TrackCart")
        {
            HitEdge();
        }
        else
        {
            other.gameObject.SetActive(false);
        }
    }

    public void HitEdge()
    {
        StartCoroutine(EndGame(2));
    }

    private IEnumerator EndGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Main Menu");
    }
}
