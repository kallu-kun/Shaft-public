using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerActivator : MonoBehaviour
{
    [SerializeField]
    private float duration = 5.0f;

    [SerializeField]
    private List<GameObject> spawners;

    private Timer spawnTimer;

    private void Start()
    {
        spawnTimer = gameObject.AddComponent<Timer>();
        spawnTimer.duration = duration;
        spawnTimer.StartTimer();
    }

    private void Update()
    {
        if (spawnTimer.isFinished)
        {
            ActivateSpawners();
        }
    }

    private void ActivateSpawners()
    {
        foreach (GameObject obj in spawners)
        {
            obj.SetActive(true);
        }
    }
}
