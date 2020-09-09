using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlacer : MonoBehaviour
{
    [SerializeField]
    private TrackData trackData = null;

    private GameObject trackStraight;
    private GameObject trackLeft;
    private GameObject trackRight;

    public List<GameObject> placedTracks;

    public TrainDetector trainDetector;
    public void Initialize()
    {
        trackStraight = trackData.trackStraight;
        trackLeft = trackData.trackLeft;
        trackRight = trackData.trackRight;

        placedTracks = trackData.trackList;
    }

    public void PlaceTrack(Vector3 pos)
    {
        GameObject track = Instantiate(trackStraight, pos, Quaternion.identity);
        placedTracks.Add(track);

        SetTrackOrientations(placedTracks.Count - 1);
    }

    public void SetTrackOrientations(int index, GameObject ghostTrack = null)
    {
        GameObject track = ghostTrack == null ? placedTracks[index] : ghostTrack;
        
        int newYRot = 0;

        if (index > 0)
        {
            GameObject previousTrack = placedTracks[index - 1];

            Vector3 prevPos = previousTrack.transform.position;
            Vector3 pos = track.transform.position;
            if (prevPos.x < pos.x)
            {
                newYRot = 90;
            }
            else if (prevPos.x > pos.x)
            {
                newYRot = 270;
            }
            else if (prevPos.z > pos.z)
            {
                newYRot = 180;
            }

            GameObject newPreviousTrack = trackStraight;

            int prevRot = (int) previousTrack.transform.rotation.eulerAngles.y;
            if ((prevRot == 0 && newYRot == 270) || (prevRot == 90 && newYRot == 0) || (prevRot == 180 && newYRot == 90) || (prevRot == 270 && newYRot == 180))
            {
                newPreviousTrack = trackLeft;
            }
            else if ((prevRot == 0 && newYRot == 90) || (prevRot == 90 && newYRot == 180) || (prevRot == 180 && newYRot == 270) || (prevRot == 270 && newYRot == 0))
            {
                newPreviousTrack = trackRight;
            }

            SwitchTrackPiece(index - 1, newPreviousTrack);
        }
        else
        {
            newYRot = 90;
        }

        track.transform.rotation = Quaternion.Euler(track.transform.rotation.eulerAngles.x, newYRot, 0);
    }

    private bool IsGreaterAngle(int rotA, int rotB)
    {
        return false;
    }

    public void SwitchTrackPiece(int index, GameObject prefab)
    {
        GameObject track = placedTracks[index];
        GameObject newTrack = Instantiate(prefab, track.transform.position, track.transform.rotation);
        placedTracks[index] = newTrack;
        Destroy(track);
    }

    public bool IsNextToTrack(Vector3 pos)
    {
        if (placedTracks.Count > 0)
        {
            Vector3 lastTrackPos = placedTracks[placedTracks.Count - 1].transform.position;
            float xDiff = Mathf.Abs(pos.x - lastTrackPos.x);
            float zDiff = Mathf.Abs(pos.z - lastTrackPos.z);
            return xDiff + zDiff <= 1 && xDiff + zDiff > 0.5f;
            //return xDiff <= 1 && zDiff <= 1 && !(xDiff == 1 && zDiff == 1) && !(xDiff == 0 && zDiff == 0);
        }
        else
        {
            return true;
        }
    }
    public GameObject LastTrack()
    {
        return placedTracks[placedTracks.Count - 1].gameObject;
    }
    
    public bool TrainOnTrack()
    {
        trainDetector = LastTrack().GetComponent<TrainDetector>();
        return (trainDetector.collidesWithTrain);
    }
}
