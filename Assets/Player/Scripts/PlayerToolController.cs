using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerToolController : MonoBehaviour
{
    [Header("Particle prefabs")]
    [SerializeField]
    public GameObject rockParticles;
    [SerializeField]
    public GameObject cactusParticles;
    [SerializeField]
    public GameObject sparkParticles;
    [SerializeField]
    public GameObject trackParticles;

    [Header("Other shit")]
    public Transform followedPlayer;

    public GameObject collidingBreakable;
    public GameObject collidingRavine;
    public TrackCrafter trackCrafter;
    public TrainController trainController;
    public CartMaterialChanger cartMaterialChanger;

    public bool isColliding;

    public bool collidesWithBreakable;
    public bool collidesWithUnbreakable;
    public bool collidesWithGround;
    public bool collidesWithRavine;

    public bool collidesWithTrack;
    public bool collidesWithPickup;
    public bool collidesWithResourceCart;
    public bool collidesWithTrain;
    public bool collidesWithTrackCart;
    public bool collidesWithTool;

    public GameObject trackGhost;
    public GameObject bridgeGhost;

    public void Initialise(Transform followedPlayer)
    {
        this.followedPlayer = followedPlayer;

        trackGhost = transform.Find("track_straight").gameObject;
        bridgeGhost = transform.Find("GreenBlock").gameObject;

        trackGhost.SetActive(false);
        bridgeGhost.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (followedPlayer.hasChanged)
        {
            ChangePosition();
        }
    }

    private void ChangePosition()
    {
        Vector3 playerPos = followedPlayer.position;
        float playerRot = followedPlayer.rotation.eulerAngles.y;

        float xPos = Mathf.Round(playerPos.x + Mathf.Sin(playerRot * Mathf.Deg2Rad));
        float zPos = Mathf.Round(playerPos.z + Mathf.Cos(playerRot * Mathf.Deg2Rad));

        Vector3 result = new Vector3(xPos, 0.2f, zPos);
        transform.position = result;
    }

    private void OnTriggerEnter(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        switch (other.tag)
        {
            case "Breakable":
                collidingBreakable = other.gameObject;
                collidesWithBreakable = true;

                BreakableObject breakableObject = other.transform.parent.GetComponent<BreakableObject>();
                breakableObject.ChangeMaterial(breakableObject.highlightMaterial);
                break;
            case "Unbreakable":
                collidesWithUnbreakable = true;
                break;
            case "Ground":
                collidesWithGround = true;
                break;
            case "Track":
                collidesWithTrack = true;
                break;
            case "WoodPickup":
                collidesWithPickup = true;
                break;
            case "RockPickup":
                collidesWithPickup = true;
                break;
            case "Ravine":
                collidesWithRavine = true;
                collidingRavine = other.transform.parent.gameObject;
                break;
            case "Axe":
                collidesWithTool = true;
                break;
            case "Pickaxe":
                collidesWithTool = true;
                break;
            case "ResourceCart":
                collidesWithResourceCart = true;
                trackCrafter = other.gameObject.GetComponent<TrackCrafter>();
                cartMaterialChanger = other.gameObject.GetComponent<CartMaterialChanger>();
                break;
            case "Train":
                collidesWithTrain = true;
                trainController = other.gameObject.GetComponent<TrainController>();
                cartMaterialChanger = other.gameObject.GetComponent<CartMaterialChanger>();
                break;
            case "TrackCart":
                collidesWithTrackCart = true;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        switch (other.tag)
        {
            case "Breakable":
                collidingBreakable = null;
                collidesWithBreakable = false;

                BreakableObject breakableObject = other.transform.parent.GetComponent<BreakableObject>();
                breakableObject.ChangeMaterial(breakableObject.defaultMaterial);
                break;
            case "Unbreakable":
                collidesWithUnbreakable = false;
                break;
            case "Ground":
                collidesWithGround = false;
                break;
            case "Track":
                collidesWithTrack = false;
                break;
            case "WoodPickup":
                collidesWithPickup = false;
                break;
            case "RockPickup":
                collidesWithPickup = false;
                break;
            case "Ravine":
                collidesWithRavine = false;
                collidingRavine = null;
                break;
            case "Axe":
                collidesWithTool = false;
                break;
            case "Pickaxe":
                collidesWithTool = false;
                break;
            case "ResourceCart":
                collidesWithResourceCart = false;
                break;
            case "Train":
                collidesWithTrain = false;
                break;
            case "TrackCart":
                collidesWithTrackCart = false;
                break;
        }
    }

    public bool CanPlacePickup()
    {
        return collidesWithGround && !collidesWithRavine
            && !collidesWithResourceCart && !collidesWithTrack && !collidesWithTrain
            && !collidesWithUnbreakable && !collidesWithBreakable;
    }

    public bool CanPlaceTrack()
    {
        return collidesWithGround && !collidesWithRavine
            && !collidesWithResourceCart && !collidesWithTrack
            && !collidesWithUnbreakable && !collidesWithBreakable;
    }

    public bool IsNextToTrack(List<GameObject> placedTracks)
    {
        Vector3 pos = transform.position;
        if (placedTracks.Count > 0)
        {
            Vector3 lastTrackPos = placedTracks[placedTracks.Count - 1].transform.position;
            float xDiff = Mathf.Abs(pos.x - lastTrackPos.x);
            float zDiff = Mathf.Abs(pos.z - lastTrackPos.z);
            return xDiff <= 1 && zDiff <= 1 && !(xDiff == 1 && zDiff == 1) && !(xDiff == 0 && zDiff == 0);
        } else
        {
            return true;
        }
    }

#if UNITY_EDITOR
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.color = new Color(100, 255, 0, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale / 2);
    }
#endif
}
