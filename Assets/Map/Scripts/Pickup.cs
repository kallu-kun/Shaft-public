using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private GameObject pickupPrefab = null;

    public List<GameObject> stackedPickups;

    public float pickupSize = 1.0f;
    private float pickupHeight = 0.2f;

    public void Initialise()
    {
        stackedPickups = new List<GameObject>();
    }

    public void AddPickups(int amount)
    {
        Debug.Log("Pickups in stack: " + stackedPickups.Count + ", Added pickups: " + amount);
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = transform.position;
            pos.y = 0.1f + pickupHeight * stackedPickups.Count;

            GameObject nPickup = Instantiate(pickupPrefab, pos, Quaternion.identity);
            nPickup.transform.localScale.Scale(new Vector3(pickupSize, pickupSize, pickupSize));

            stackedPickups.Add(nPickup);
            nPickup.transform.parent = gameObject.transform;
        }
    }

    public void RemovePickups(int amount)
    {
        int pickupAmount = stackedPickups.Count;

        Debug.Log("Pickups in stack: " + pickupAmount + ", Removed pickup amount: " + amount);
        for (int i = 0; i < amount; i++)
        {
            GameObject stackedPickup = stackedPickups[stackedPickups.Count - 1];
            stackedPickups.Remove(stackedPickup);
            Destroy(stackedPickup);
        }

        if (stackedPickups.Count <= 0)
        {
            Destroy(gameObject);
        }

        Debug.Log("Pickups remaining: " + stackedPickups.Count);
    }
}