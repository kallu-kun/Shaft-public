using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TrackData : ScriptableObject
{
    public List<GameObject> trackList;

    [Header("Track prefabs")]
    public GameObject trackLeft;
    public GameObject trackRight;
    public GameObject trackStraight;

    public void Initialise()
    {
        trackList = new List<GameObject>();
    }
}
