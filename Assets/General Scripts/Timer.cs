using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float currentTime;
    public float duration;

    public bool isRunning { get; protected set; }
    public bool isFinished { get; protected set; }

    public void StartTimer()
    {
        isRunning = true;
        isFinished = false;
        currentTime = 0.0f;
    }

    public void StopTimer()
    {
        isRunning = false;
        isFinished = false;
        currentTime = 0.0f;
    }

    private void Update()
    {
        if (isRunning)
        {
            if (currentTime < duration)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                isFinished = true;
                isRunning = false;
            }
        }
    }
}
