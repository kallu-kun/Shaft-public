using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    public GameObject woodPickup = null;
    [SerializeField]
    public GameObject rockPickup = null;
    [SerializeField]
    public GameObject fuelPickup = null;
    [SerializeField]
    public GameObject axe = null;
    [SerializeField]
    public GameObject pickaxe = null;
    [SerializeField]
    public GameObject track = null;
    [SerializeField]
    public GameObject trackPickup = null;

    private List<GameObject> heldItems;

    public void Awake()
    {
        heldItems = new List<GameObject>();
        heldItems.Add(fuelPickup);
        heldItems.Add(rockPickup);
        heldItems.Add(woodPickup);
        heldItems.Add(axe);
        heldItems.Add(pickaxe);
        heldItems.Add(track);
        heldItems.Add(trackPickup);
    }

    public GameObject GetItemFromTag(string tag)
    {
        GameObject result = null;

        if (tag != null)
        {
            foreach (GameObject obj in heldItems)
            {
                if (obj != null && tag.Equals(obj.tag))
                {
                    result = obj;
                    break;
                }
            }
        }

        return result;
    }

    public GameObject GetPickupObject(string tag, int pickupAmount)
    {
        GameObject result = null;

        if (tag != null)
        {
            foreach (GameObject obj in heldItems)
            {
                if (obj != null && tag.Equals(obj.tag) && obj.GetComponent<Pickup>() != null)
                {
                    result = obj;
                    break;
                }
            }
        }

        if (result != null && result.GetComponent<Pickup>() != null)
        {
            Pickup pickup = result.GetComponent<Pickup>();
            pickup.Initialise();
            pickup.AddPickups(pickupAmount);
        }

        return result;
    }

    public bool IsSameItem(GameObject obj1, GameObject obj2)
    {
        return obj1 != null && obj2 != null && obj1.tag == obj2.tag;
    }

    public bool IsPickup(GameObject obj)
    {
        return obj != null && (IsRock(obj) || IsWood(obj) || IsFuel(obj) || IsTrackpickup(obj));
    }

    public bool IsTool(GameObject obj)
    {
        return obj != null && (IsAxe(obj) || IsPickaxe(obj));
    }

    public bool IsRock(GameObject obj)
    {
        return obj != null && obj.tag == rockPickup.tag;
    }

    public bool IsWood(GameObject obj)
    {
        return obj != null && obj.tag == woodPickup.tag;
    }

    public bool IsFuel(GameObject obj)
    {
        return obj != null && obj.tag == fuelPickup.tag;
    }

    public bool IsAxe(GameObject obj)
    {
        return obj != null && obj.tag == axe.tag;
    }

    public bool IsPickaxe(GameObject obj)
    {
        return obj != null && obj.tag == pickaxe.tag;
    }

    public bool IsTrack(GameObject obj)
    {
        return obj != null && obj.tag == track.tag;
    }

    public bool IsTrackpickup(GameObject obj)
    {
        return obj != null && obj.tag == trackPickup.tag;
    }

    public bool IsPickableItem(GameObject obj)
    {
        return obj != null && (obj.tag == woodPickup.tag || obj.tag == rockPickup.tag || obj.tag == fuelPickup.tag ||
            obj.tag == axe.tag || obj.tag == pickaxe.tag || obj.tag == trackPickup.tag);
    }
}
