using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            levelData.generateMoreLevel = true;
            levelData.cameraspeed *= 1.2f;
        }

        gameObject.SetActive(false);
    }
}
