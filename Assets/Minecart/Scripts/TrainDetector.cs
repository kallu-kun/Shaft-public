using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDetector : MonoBehaviour
{
    public bool collidesWithTrain;
    // Start is called before the first frame update
    void Awake()
    {
        collidesWithTrain = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TrackDetector")
        {
            collidesWithTrain = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TrackDetector")
        {
            collidesWithTrain = false;
        }
    }
}
