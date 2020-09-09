using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
	[SerializeField]
	public GameObject pickupPrefab;
    [SerializeField]
    public int pickupsDropped = 1;

	[SerializeField]
	public Material highlightMaterial;
	public Material defaultMaterial;

	private MeshRenderer[] renderers;

	public int health;
	public int maxHealth;

	public void Start()
	{
		renderers = GetComponentsInChildren<MeshRenderer>();
		defaultMaterial = renderers[0].material;

		Initialise();
		RotateMeshes();
	}

	private void Initialise()
	{
		foreach (MeshRenderer renderer in renderers)
		{
			renderer.enabled = true;
		}

		health = 4;
		maxHealth = health;
	}

	public void TakeDamage()
	{
		if (renderers.Length == maxHealth)
		{
			renderers[maxHealth - health].enabled = false;
		}
		health -= 1;

		if (health == 0)
		{
			Destroy();
		}
	}

	private void Destroy()
	{
		GameObject nPickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);

		Pickup pickup = nPickup.GetComponent<Pickup>();
		pickup.Initialise();
		pickup.AddPickups(pickupsDropped);

		Initialise();
		gameObject.transform.position = new Vector3(0, 0, 0);
		gameObject.SetActive(false);
	}

	public void ChangeMaterial(Material material)
	{
		foreach (MeshRenderer renderer in renderers)
		{
			renderer.material = material;
		}
	}

	public void RotateMeshes()
	{
		Quaternion currentRot = renderers[0].transform.rotation;
		Quaternion nRot = Quaternion.Euler(new Vector3(currentRot.eulerAngles.x, Random.Range(0, 359), currentRot.eulerAngles.z));

		foreach (MeshRenderer renderer in renderers)
		{
			renderer.gameObject.transform.rotation = nRot;
		}
	}
}
