using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject[] tools = null;
    private GameObject[] nTools;

	[SerializeField]
	private Transform toolSpawnPosition = null;

    [SerializeField]
    private GameObject trackPickup = null;

	private float xOffset;
    private float zOffset;

	private void Start()
	{
		xOffset = 3;
        zOffset = 4;

        nTools = new GameObject[2];

        for (int i = 0; i < 2; i++)
		{
			nTools[i] = Instantiate(tools[i], toolSpawnPosition.position + new Vector3(xOffset, 0, zOffset), Quaternion.Euler(0, -90, -90), transform);
			xOffset += 5;
		}

        GameObject nTrackPickup = Instantiate(trackPickup, toolSpawnPosition.position + new Vector3(5, 0, 0), Quaternion.identity);
        Pickup pickup = nTrackPickup.GetComponent<Pickup>();
        pickup.Initialise();
        pickup.AddPickups(3);
	}
}
