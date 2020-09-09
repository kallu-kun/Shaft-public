using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: 
// 1. Pickuppien switchaaminen edestakas maasta käteen, kädestä maahan
// 2. Bugi: Jos seisoo pickupin päällä ja laskee pickupit maahan, tapahtuu kummia
// 3. Bugi: Pickupit jäävät ilmaan välillä kun juoksee pickup stackin yli - other.gameObject.SetActive(false) tapahtuu randomisti
// OnTriggerEnterissä? Miten voisi poistaa ylimmät ensin?

[RequireComponent(typeof(CharacterController), typeof(PlayerToolController), typeof(TrackPlacer))]
public class PickupExampleNotes : MonoBehaviour
{

	[Header("Renderers")]

	public MeshRenderer[] fuelPickupRenderer;
	public MeshRenderer[] woodPickupRenderer;
	public MeshRenderer[] rockPickupRenderer;
	public MeshRenderer trackPickupRenderer;
	public MeshRenderer axeRenderer;
	public MeshRenderer pickaxeRenderer;

	[Header("Prefabs")]

	[SerializeField]
	private GameObject toolPrefab = null;

	public GameObject fuelPickup;
	public GameObject woodPickup;
	public GameObject rockPickup;
	public GameObject axe;
	public GameObject pickaxe;

	private GameObject toolCheckbox;
	private PlayerToolController toolController;

	private int pickupAmount;

	private bool isHoldingItem;
	private bool isHoldingAxe;
	private bool isHoldingPickaxe;
	private bool isHoldingWoodPickup;
	private bool isHoldingRockPickup;
	private bool isHoldingFuelPickup;

	private bool playerInPickupArea = false;
	private bool playerWantstoPickupItem = false;
	private bool isHoldingPickUp = false;
	private bool isHoldingTrack = false;
	private GameObject heldPickUp;
	private Quaternion pickupOriginalRotation;
	private Quaternion toolOriginalRotation;
	private Vector2 movementInput;

	private void Start()
	{
		toolCheckbox = Instantiate(toolPrefab, transform.position, Quaternion.identity);
		FindComponents();

		pickupOriginalRotation = Quaternion.Euler(0, 0, 0);
		toolOriginalRotation = Quaternion.Euler(0, -90, -90);

		SetRenderers();
	}

	private void FindComponents()
	{
		toolController = toolCheckbox.GetComponent<PlayerToolController>();
		toolController.Initialise(transform);
	}

	private void SetRenderers()
	{
		foreach (MeshRenderer woodRenderer in woodPickupRenderer)
		{
			woodRenderer.enabled = false;
		}

		foreach (MeshRenderer rockRenderer in rockPickupRenderer)
		{
			rockRenderer.enabled = false;
		}

		foreach (MeshRenderer fuelRenderer in fuelPickupRenderer)
		{
			fuelRenderer.enabled = false;
		}

		trackPickupRenderer.enabled = false;
		pickaxeRenderer.enabled = false;
		axeRenderer.enabled = false;
	}

	public void Interact(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{

			if (playerInPickupArea && !isHoldingItem)
			{
				playerWantstoPickupItem = true;
			}
			else if (isHoldingItem && (!isHoldingPickUp || (!toolController.collidesWithResourceCart &&
				!toolController.collidesWithTrain && !toolController.collidesWithRavine)) && !isHoldingTrack)
			{
				playerWantstoPickupItem = false;
				PlaceItemOnGround();
			}
			else if (isHoldingPickUp && toolController.collidesWithResourceCart)
			{
				AddResources();
			}
			else if (isHoldingPickUp && toolController.collidesWithTrain) 
			{
				AddFuel();
			}
			else if (!isHoldingItem && toolController.collidesWithTrackCart && toolController.trackCrafter.CanPickUp())
			{
				playerWantstoPickupItem = true;
			}
		}
	}

	private void AddResources()
	{
		if (isHoldingWoodPickup)
		{
			toolController.trackCrafter.AddResources(pickupAmount, 0);
			Debug.Log("Wood resource added");
			PickUpsUsed();

			foreach (MeshRenderer woodRenderer in woodPickupRenderer)
			{
				woodRenderer.enabled = false;
			}
		}
		else if (isHoldingRockPickup)
		{
			toolController.trackCrafter.AddResources(0, pickupAmount);
			Debug.Log("Rock resources added");
			PickUpsUsed();

			foreach (MeshRenderer rockRenderer in rockPickupRenderer)
			{
				rockRenderer.enabled = false;
			}
		}
	}

	private void AddFuel()
	{
		if (isHoldingFuelPickup)
		{
			toolController.trainController.AddResources(pickupAmount);
			Debug.Log("Fuel added");
			PickUpsUsed();
			foreach (MeshRenderer fuelRenderer in fuelPickupRenderer)
			{
				fuelRenderer.enabled = false;
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "WoodPickup")
		{
			playerInPickupArea = true;

			if (playerWantstoPickupItem && pickupAmount <= 3 && !isHoldingRockPickup && !isHoldingFuelPickup && !isHoldingAxe && !isHoldingPickaxe)
			{
				isHoldingItem = true;
				isHoldingPickUp = true;
				heldPickUp = other.gameObject;
				isHoldingWoodPickup = true;

				for (int i = 0; i < pickupAmount; i++)
				{
					woodPickupRenderer[i].enabled = true;
				}

				other.gameObject.SetActive(false);
			}
		}
		else if (other.tag == "RockPickup")
		{
			playerInPickupArea = true;

			if (playerWantstoPickupItem && pickupAmount <= 3 && !isHoldingWoodPickup && !isHoldingFuelPickup && !isHoldingAxe && !isHoldingPickaxe)
			{
				isHoldingItem = true;
				isHoldingPickUp = true;
				heldPickUp = other.gameObject;
				isHoldingRockPickup = true;

				for (int i = 0; i < pickupAmount; i++)
				{
					rockPickupRenderer[i].enabled = true;
				}

				other.gameObject.SetActive(false);
			}
		}
		else if (other.tag == "FuelPickup")
		{
			playerInPickupArea = true;

			if (playerWantstoPickupItem && pickupAmount <= 3 && !isHoldingWoodPickup && !isHoldingRockPickup && !isHoldingAxe && !isHoldingPickaxe)
			{
				isHoldingItem = true;
				isHoldingPickUp = true;
				heldPickUp = other.gameObject;
				isHoldingFuelPickup = true;

				for (int i = 0; i < pickupAmount; i++)
				{
					fuelPickupRenderer[i].enabled = true;
				}

				other.gameObject.SetActive(false);
			}
		}
		else if (other.tag == "Axe")
		{
			playerInPickupArea = true;

			if (playerWantstoPickupItem && !isHoldingItem)
			{
				isHoldingItem = true;
				isHoldingAxe = true;

				axeRenderer.enabled = true;
				other.gameObject.SetActive(false);
			}
		}
		else if (other.tag == "Pickaxe")
		{
			playerInPickupArea = true;

			if (playerWantstoPickupItem && !isHoldingItem)
			{
				isHoldingItem = true;
				isHoldingPickaxe = true;

				pickaxeRenderer.enabled = true;
				other.gameObject.SetActive(false);
			} 
		}
		else if (other.tag == "TrackCart")
		{
			if (playerWantstoPickupItem && !isHoldingItem)
			{
				isHoldingItem = true;
				isHoldingTrack = true;
				playerWantstoPickupItem = false;

				trackPickupRenderer.enabled = true;
				toolController.trackCrafter.GetTrack();
			}
		}
	}

    private void PickItem(GameObject pickedObject, Renderer[] renderers)
    {
        playerInPickupArea = true;

        if (playerWantstoPickupItem && pickupAmount <= 3 && !isHoldingRockPickup && !isHoldingFuelPickup && !isHoldingAxe && !isHoldingPickaxe)
        {
            isHoldingItem = true;
            isHoldingPickUp = true;
            heldPickUp = pickedObject;

            for (int i = 0; i < pickupAmount; i++)
            {
                renderers[i].enabled = true;
            }

            pickedObject.SetActive(false);
        }
    }

    private bool IsHoldingAxe()
    {
        return heldPickUp.tag == "Axe";
    }

    private bool AreHandsFull()
    {
        return pickupAmount <= 3 || heldPickUp.tag == "Axe" || heldPickUp.tag == "Pickaxe";
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "WoodPickup" && !isHoldingWoodPickup && !isHoldingRockPickup && !isHoldingFuelPickup && pickupAmount <= 3)
		{
			pickupAmount++;
		}
		else if (other.tag == "RockPickup" && !isHoldingWoodPickup && !isHoldingRockPickup && !isHoldingFuelPickup && pickupAmount <= 3)
		{
			pickupAmount++;
		}
		else if (other.tag == "FuelPickup" && !isHoldingWoodPickup && !isHoldingRockPickup && !isHoldingFuelPickup && pickupAmount <= 3)
		{
			pickupAmount++;
		}
		
		if (other.tag == "WoodPickup" && isHoldingWoodPickup && !isHoldingRockPickup && !isHoldingFuelPickup && pickupAmount <= 3)
		{
			pickupAmount++;

			if (pickupAmount == 2)
			{
				woodPickupRenderer[1].enabled = true;
				other.gameObject.SetActive(false);
			}
			else if (pickupAmount == 3)
			{
				woodPickupRenderer[2].enabled = true;
				other.gameObject.SetActive(false);
			}
		}
		else if (other.tag == "RockPickup" && isHoldingRockPickup && !isHoldingWoodPickup && !isHoldingFuelPickup && pickupAmount <= 3)
		{
			pickupAmount++;

			if (pickupAmount == 2)
			{
				rockPickupRenderer[1].enabled = true;
				other.gameObject.SetActive(false);
			}
			else if (pickupAmount == 3)
			{
				rockPickupRenderer[2].enabled = true;
				other.gameObject.SetActive(false);
			}
		}
		else if (other.tag == "FuelPickup" && isHoldingFuelPickup && !isHoldingWoodPickup && !isHoldingRockPickup && pickupAmount <= 3)
		{
			pickupAmount++;

			if (pickupAmount == 2)
			{
				fuelPickupRenderer[1].enabled = true;
				other.gameObject.SetActive(false);
			}
			else if (pickupAmount == 3)
			{
				fuelPickupRenderer[2].enabled = true;
				other.gameObject.SetActive(false);
			}
		}
		
	}

    private void AddItems(GameObject addedObject, Renderer[] renderers)
    {
        pickupAmount++;

        if (pickupAmount == 2)
        {
            rockPickupRenderer[1].enabled = true;
            addedObject.SetActive(false);
        } else if (pickupAmount == 3)
        {
            rockPickupRenderer[2].enabled = true;
            addedObject.SetActive(false);
        }
    }

	private void PlaceItemOnGround()
	{
		if (CanPlaceItem())
		{
			float pickupPosY = 0.1f;

			if (heldPickUp.tag == "WoodPickup")
			{
				for (int i = 0; i < pickupAmount; i++)
				{
					Instantiate(woodPickup, new Vector3(Mathf.Round(toolController.transform.position.x),
						pickupPosY, Mathf.Round(toolController.transform.position.z)), pickupOriginalRotation);

					pickupPosY += 0.2f;
				}

				foreach (MeshRenderer woodRenderer in woodPickupRenderer)
				{
					woodRenderer.enabled = false;
				}
			}
			else if (isHoldingRockPickup)
			{
				for (int i = 0; i < pickupAmount; i++)
				{
					Instantiate(rockPickup, new Vector3(Mathf.Round(toolController.transform.position.x),
						pickupPosY, Mathf.Round(toolController.transform.position.z)), pickupOriginalRotation);

					pickupPosY += 0.2f;
				}

				foreach (MeshRenderer rockRenderer in rockPickupRenderer)
				{
					rockRenderer.enabled = false;
				}
			}
			else if (isHoldingFuelPickup)
			{
				for (int i = 0; i < pickupAmount; i++)
				{
					Instantiate(fuelPickup, new Vector3(Mathf.Round(toolController.transform.position.x),
						pickupPosY, Mathf.Round(toolController.transform.position.z)), pickupOriginalRotation);

					pickupPosY += 0.2f;
				}

				foreach (MeshRenderer fuelRenderer in fuelPickupRenderer)
				{
					fuelRenderer.enabled = false;
				}
			}
			else if (isHoldingAxe)
			{
				Instantiate(axe, new Vector3(Mathf.Round(toolController.transform.position.x),
					pickupPosY, Mathf.Round(toolController.transform.position.z)), toolOriginalRotation);

				axeRenderer.enabled = false;
			}
			else if (isHoldingPickaxe)
			{
				Instantiate(pickaxe, new Vector3(Mathf.Round(toolController.transform.position.x),
					pickupPosY, Mathf.Round(toolController.transform.position.z)), toolOriginalRotation);

				pickaxeRenderer.enabled = false;
			}

			PickUpsUsed();

			/*
			pickupAmount = 0;
			isHoldingPickUp = false;
			isHoldingWoodPickup = false;
			isHoldingRockPickup = false;
			isHoldingFuelPickup = false;
			isHoldingItem = false;
			isHoldingAxe = false;
			isHoldingPickaxe = false;
			*/
		}
	}

    private void PlaceItem(GameObject placedItem, Renderer[] renderers, float pickupPosY)
    {
        for (int i = 0; i < pickupAmount; i++)
        {
            Instantiate(placedItem, new Vector3(Mathf.Round(toolController.transform.position.x),
                pickupPosY, Mathf.Round(toolController.transform.position.z)), pickupOriginalRotation);

            pickupPosY += 0.2f;
        }

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }

	private bool CanPlaceItem()
	{
		return !toolController.collidesWithPickup && !toolController.collidesWithResourceCart
			&& !toolController.collidesWithTool && !toolController.collidesWithTrack
			&& !toolController.collidesWithTrain && !toolController.collidesWithUnbreakable
			&& !toolController.collidesWithBreakable;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "WoodPickup" && !isHoldingRockPickup && !isHoldingFuelPickup)
		{
			pickupAmount--;
		}
		else if (other.tag == "RockPickup" && !isHoldingWoodPickup && !isHoldingFuelPickup)
		{
			pickupAmount--;
		}
		else if (other.tag == "FuelPickup" && !isHoldingRockPickup && !isHoldingWoodPickup)
		{
			pickupAmount--;
		}

		playerInPickupArea = false;
	}

	private void RemoveOnePickUp()
	{
		if (pickupAmount > 0)
		{
			if (isHoldingRockPickup)
			{
				rockPickupRenderer[pickupAmount - 1].enabled = false;
			}
			else if (isHoldingWoodPickup)
			{
				woodPickupRenderer[pickupAmount - 1].enabled = false;
			}
			else if (isHoldingFuelPickup)
			{
				fuelPickupRenderer[pickupAmount - 1].enabled = false;
			}
			pickupAmount--;

			if (pickupAmount == 0)
			{
				isHoldingPickUp = false;
				isHoldingRockPickup = false;
				isHoldingWoodPickup = false;
			}
		}
	}

	private void PickUpsUsed()
	{
		pickupAmount = 0;
		Destroy(heldPickUp);
		playerInPickupArea = false;
		playerWantstoPickupItem = false;
		isHoldingItem = false;
		isHoldingPickUp = false;
		isHoldingWoodPickup = false;
		isHoldingRockPickup = false;
		isHoldingFuelPickup = false;
		isHoldingAxe = false;
		isHoldingPickaxe = false;
	}
}


// Aikasempi input tsydeemi
/*
controls.Player.Hit.performed += ctx => Hit();
controls.Player.Pickup.performed += ctx => PickUpItem();
controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
controls.Player.Move.canceled += ctx => move = Vector2.zero;
*/

/*private void OnEnable()
	{
		controls.Player.Enable();
	}

	private void OnDisable()
	{
		controls.Player.Disable();
	}

	private void Update()
	{
		Vector3 movement = new Vector3(move.x, 0, move.y) * movementSpeed * Time.deltaTime;
		characterController.Move(movement);
		Debug.Log(isHoldingItem);
	}

	public void Hit()
	{
		Debug.Log("Lyö!");
	}

	public void PickUpItem()
	{
		if (!isHoldingItem)
		{
			isHoldingItem = true;
		}
		else if (isHoldingItem)
		{
			isHoldingItem = false;
		}
	}
*/

// Aikasempi pickup tsydeemi
/*
private void OnTriggerStay(Collider other)
{
if (other.tag == "Pickup" || other.tag == "Tool")
{
	playerInPickupArea = true;

	if (playerWantstoPickupItem && !isHoldingItem)
	{
		isHoldingItem = true;

		if (other.tag == "Tool")
		{
			other.transform.parent = toolPosition;
			other.transform.position = toolPosition.position;
			other.transform.rotation = toolPosition.rotation;

			isHoldingAxe = other.name == "Axe(Clone)";
			isHoldingPickaxe = other.name == "Pickaxe(Clone)";

			Debug.Log(isHoldingAxe);
		}
		else if (other.tag == "Pickup")
		{
			isHoldingPickUp = true;

			other.transform.parent = pickupPosition;
			other.transform.position = pickupPosition.position;
			other.transform.rotation = pickupPosition.rotation;
		}		
	}
	else if (!playerWantstoPickupItem && isHoldingItem)
	{
		if (other.tag == "Tool")
		{
			other.transform.position = new Vector3(Mathf.Round(toolController.transform.position.x),
				other.transform.parent.position.y - pickupOffsetY,
				Mathf.Round(toolController.transform.position.z));

			other.transform.rotation = toolOriginalRotation;
			other.transform.parent = null;

			isHoldingAxe = false;
			isHoldingPickaxe = false;
			isHoldingItem = false;
		}
		else if (other.tag == "Pickup")
		{
			isHoldingPickUp = false;

			other.transform.position = new Vector3(Mathf.Round(toolController.transform.position.x),
				other.transform.parent.position.y - pickupOffsetY,
				Mathf.Round(toolController.transform.position.z));

			other.transform.rotation = pickupOriginalRotation;
			other.transform.parent = null;
			isHoldingItem = false;
		}
	}
} 
}
*/
