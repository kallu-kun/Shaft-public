using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// TODO: 
// 1. Pickuppien switchaaminen edestakas maasta käteen, kädestä maahan
// 2. Bugi: Jos seisoo pickupin päällä ja laskee pickupit maahan, tapahtuu kummia
// 3. Bugi: Pickupit jäävät ilmaan välillä kun juoksee pickup stackin yli - other.gameObject.SetActive(false) tapahtuu randomisti
// OnTriggerEnterissä? Miten voisi poistaa ylimmät ensin?

[RequireComponent(typeof(CharacterController), typeof(PlayerToolController), typeof(TrackPlacer)), RequireComponent(typeof(PlayerRendererAssist))]
public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private LevelData levelData;

	private GameObject pauseMenu;

	[Header("Speed")]
	[SerializeField]
	private float movementSpeed = 0;
	[SerializeField]
	private float turnSpeed = 0;
	[SerializeField]
	private float gravity = 5f;

	[Header("ToolCheckbox")]
	[SerializeField]
	private GameObject bridgeBlock = null;
	[SerializeField]
	private GameObject toolPrefab = null;

	[SerializeField]
	private GameObject placePositionChecker;

	[Header("Audio")]
	public AudioSource audioSource;
	public AudioClip[] audioClips;

	private CharacterController characterController;
	private Animator animator;

	private ItemData itemData;
	private PlayerRendererAssist rendererAssist;

	private GameObject toolCheckbox;
	private PlayerToolController toolController;

	private TrackPlacer trackPlacer;

	private int pickupAmount;
	private int maxPickupAmount = 3;
	private GameObject heldItem;
	private GameObject pickableItem;

	private Timer resourceBreakingTimer;
	private float resourceBreakingDuration = 0.3f;

	private Quaternion toolOriginalRotation;
	private Vector2 movementInput;
	private Vector3 movement;

	private void Start()
	{
		toolCheckbox = Instantiate(toolPrefab, transform.position, Quaternion.identity);

		FindComponents();

		resourceBreakingTimer = gameObject.AddComponent<Timer>();
		resourceBreakingTimer.duration = resourceBreakingDuration;

		toolOriginalRotation = Quaternion.Euler(0, -90, -90);
	}

	private void FindComponents()
	{
		pauseMenu = GameObject.Find("Canvas").transform.Find("Pause Menu").gameObject;

		itemData = GetComponent<ItemData>();
		rendererAssist = GetComponent<PlayerRendererAssist>();
		rendererAssist.Initialise(itemData);

		characterController = GetComponent<CharacterController>();

		toolController = toolCheckbox.GetComponent<PlayerToolController>();
		toolController.Initialise(transform);

		trackPlacer = GetComponent<TrackPlacer>();
		trackPlacer.Initialize();

		animator = GetComponentInChildren<Animator>();
	}

	private void Update()
	{
		PlayerMovement();

		TrackGhostActivity();
		BridgeGhostActivity();
		CartHighlightActivity();

		HitBreakable();
	}

	private void PlayerMovement()
	{
		if (movementInput != Vector2.zero && characterController.isGrounded)
		{
			movement = new Vector3(movementInput.x, 0, movementInput.y) * movementSpeed * Time.deltaTime;
			characterController.Move(movement);
			Quaternion targetRotation = Quaternion.LookRotation(movement);
			Quaternion limitedRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
			transform.rotation = limitedRotation;
		}

		movement.y = gravity * Time.deltaTime;
		characterController.Move(new Vector3(0, -movement.y, 0));

		animator.SetBool("walk", movementInput != Vector2.zero);
		animator.SetBool("holdingPickup", heldItem != null && heldItem.tag != "Axe" && heldItem.tag != "Pickaxe");
	}

	public void Move(InputAction.CallbackContext context)
	{
		movementInput = context.ReadValue<Vector2>();
	}

	public void Interact(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			if (pickableItem != null)
			{
				PickOrSwapItem();
			}
			else if ((itemData.IsWood(heldItem) || itemData.IsRock(heldItem)) && toolController.collidesWithResourceCart)
			{
				AddResourcesToResourceCart();
			}
			else if (itemData.IsFuel(heldItem) && toolController.collidesWithTrain)
			{
				AddFuel();
			}
			else if (toolController.collidesWithTrackCart && toolController.trackCrafter.tracksReady > 0 && (!IsHoldingItem() ||
				itemData.IsTrackpickup(heldItem)))
			{
				TracksFromCart();
			}
			else if (itemData.IsWood(heldItem) && toolController.collidesWithRavine &&
				levelData.isInBounds((int)toolController.transform.position.x, (int)toolController.transform.position.z))
			{
				PlaceBridge();
			}
			else if (trackPlacer != null && CanPlaceTrack() && itemData.IsTrackpickup(heldItem))
			{
				PlaceTrack();
			}
			else if (IsHoldingItem())
			{
				PlaceItemOnGround(heldItem);
			}
		}
	}

	public void OpenMenu(InputAction.CallbackContext context)
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
	}

	private void AddResourcesToResourceCart()
	{
		if (itemData.IsWood(heldItem))
		{
			toolController.trackCrafter.AddResources(pickupAmount, 0);
		}
		else if (itemData.IsRock(heldItem))
		{
			toolController.trackCrafter.AddResources(0, pickupAmount);
		}
		Debug.Log("Resources added");
		RemoveAllPickups();
		audioSource.PlayOneShot(PlayPlaceResourcesSound());

		PlaySparkParticleEffect();
	}

	private void AddFuel()
	{
		toolController.trainController.AddResources(pickupAmount);
		Debug.Log("Fuel added");
		RemoveAllPickups();
		audioSource.PlayOneShot(PlayPlaceResourcesSound());

		PlaySparkParticleEffect();
	}

	private void TracksFromCart()
	{

		GameObject trackObject = Instantiate(itemData.trackPickup, transform.position, Quaternion.identity);
		Pickup pickup = trackObject.GetComponent<Pickup>();
		pickup.Initialise();
		pickup.AddPickups(toolController.trackCrafter.tracksReady);
		toolController.trackCrafter.TakeTracks(toolController.trackCrafter.tracksReady);
		PickItem(trackObject);

	}

	private void PlaceBridge()
	{
		Vector3 bridgeLoc = new Vector3(toolController.transform.position.x, -1, toolController.transform.position.z);
		toolController.collidingRavine.SetActive(false);

		Instantiate(bridgeBlock, bridgeLoc, Quaternion.identity);

		RemoveOnePickUp();
	}

	private void PlaceTrack()
	{
		Vector3 toolPos = toolCheckbox.transform.position;
		trackPlacer.PlaceTrack(new Vector3(toolPos.x, 0, toolPos.z));

		RemoveOnePickUp();

		PlayTrackParticleEffect();
		audioSource.PlayOneShot(PlayPlaceTrackSound());

		Debug.Log("Track placed");
	}

	private void PickOrSwapItem()
	{
		if (IsHoldingItem() && itemData.IsSameItem(heldItem, pickableItem))
		{
			PlaceItemOnGround(heldItem);
		}
		else if (IsHoldingItem())
		{
			PlaceItemOnGround(heldItem);
			PickItem(pickableItem);
		}
		else
		{
			PickItem(pickableItem);
		}
	}

	private void PickItem(GameObject item)
	{
		int pickedItemAmount = 1;

		heldItem = Instantiate(itemData.GetItemFromTag(item.tag), new Vector3(-10, -10, -10), Quaternion.identity);

		if (itemData.IsPickup(item))
		{
			Pickup pickup = item.GetComponent<Pickup>();
			int pickupsInStack = pickup.stackedPickups.Count;
			pickedItemAmount = pickupsInStack + pickupAmount < maxPickupAmount ? pickupsInStack : maxPickupAmount - pickupAmount;

			pickup.RemovePickups(pickedItemAmount);
		}
		else
		{
			Destroy(item);
		}

		pickupAmount += pickedItemAmount;

		rendererAssist.SetActiveHeldItemRenderer(heldItem.tag);
		rendererAssist.ActivateHeldItemRenderers(pickedItemAmount);
	}

	private void PlaceItemOnGround(GameObject item)
	{
		item = itemData.GetItemFromTag(item.tag);
		Vector3 itemPlacePos = new Vector3(transform.position.x, 0.1f, transform.position.z);
		GameObject groundItem = null;

		if (itemData.IsPickup(item))
		{
			Pickup pickup;
			if (itemData.IsSameItem(heldItem, pickableItem))
			{
				groundItem = pickableItem;
				pickup = groundItem.GetComponent<Pickup>();
			}
			else
			{
				groundItem = Instantiate(item, itemPlacePos, Quaternion.identity);
				pickup = groundItem.GetComponent<Pickup>();
				pickup.Initialise();
			}
			pickup.AddPickups(pickupAmount);
		}
		else if (itemData.IsTool(item))
		{
			Instantiate(item, itemPlacePos, toolOriginalRotation);
		}

		RemoveAllPickups();
	}

	private Vector3 GetItemPlacePosition()
	{
		Vector3 itemPlacePos = new Vector3();

		PlacePositionChecker checker = placePositionChecker.GetComponent<PlacePositionChecker>();
		if (checker.positionIsBlocked)
		{
			for (int i = 0; i < 30; i++)
			{
				checker.MoveAround();
				Debug.Log(checker.transform.position);

				if (!checker.positionIsBlocked)
				{
					itemPlacePos = checker.transform.position;
				}
			}

			checker.ResetPosition();
		}
		else
		{
			int xPos = (int)Mathf.Round(transform.position.x);
			int zPos = (int)Mathf.Round(transform.position.z);

			itemPlacePos = new Vector3(xPos, 0.1f, zPos);
		}

		return itemPlacePos;
	}

	private void RemoveOnePickUp()
	{
		if (pickupAmount > 0)
		{
			pickupAmount--;

			if (pickupAmount == 0)
			{
				heldItem = null;
			}

			rendererAssist.DeactivateOneHeldItemRenderer();
		}
	}

	private void RemoveAllPickups()
	{
		pickupAmount = 0;
		heldItem = null;

		rendererAssist.DeActivateHeldItemRenderers();
	}

	private void HitBreakable()
	{
		GameObject breakable = toolController.collidingBreakable;
		animator.SetBool("mine", toolController.collidesWithBreakable && CanHitBreakable(breakable));
		if (resourceBreakingTimer.isFinished && CanHitBreakable(breakable) && breakable.transform.parent != null)
		{
			PlayBreakingParticleEffect(breakable.transform.parent.gameObject);
			breakable.transform.parent.GetComponent<BreakableObject>().TakeDamage();
			resourceBreakingTimer.StopTimer();
		} else if (toolController.collidesWithBreakable && !resourceBreakingTimer.isRunning && CanHitBreakable(breakable))
		{
			resourceBreakingTimer.StartTimer();
		} else if (!toolController.collidesWithBreakable)
		{
			resourceBreakingTimer.StopTimer();
		}
	}

	private void PlayBreakingParticleEffect(GameObject breakable)
	{
		GameObject particles = null;
		if (breakable.name == "Rock(Clone)" || breakable.name == "Fuel(Clone)")
		{
			particles = toolController.rockParticles;
			audioSource.PlayOneShot(PlayHitRockSound());
		} else if (breakable.name == "Cactus(Clone)")
		{
			particles = toolController.cactusParticles;
			audioSource.PlayOneShot(PlayHitWoodSound());
		}
		if (particles != null)
		{
			GameObject nParticles = Instantiate(particles, toolCheckbox.transform.position, Quaternion.identity);
		}
	}

	private void PlaySparkParticleEffect()
	{
		GameObject nParticles = Instantiate(toolController.sparkParticles, toolCheckbox.transform.position, Quaternion.identity);
	}

	private void PlayTrackParticleEffect()
	{
		GameObject nParticles = Instantiate(toolController.trackParticles, toolCheckbox.transform.position, Quaternion.identity);
	}

	private void TrackGhostActivity()
	{
		GameObject trackGhost = toolController.trackGhost;

		if (trackGhost.activeInHierarchy)
		{
			if (CanPlaceTrack())
			{
				trackPlacer.SetTrackOrientations(trackPlacer.placedTracks.Count, trackGhost);
			}
			else
			{
				trackPlacer.SetTrackOrientations(trackPlacer.placedTracks.Count - 1);
			}

		}
		trackGhost.SetActive(CanPlaceTrack());
	}

	private void BridgeGhostActivity()
	{
		toolController.bridgeGhost.SetActive(CanPlaceBridge());
	}

	private void CartHighlightActivity()
	{
		if (toolController.cartMaterialChanger != null)
		{
			if ((itemData.IsWood(heldItem) || itemData.IsRock(heldItem)) && toolController.collidesWithResourceCart)
			{
				toolController.cartMaterialChanger.ChangeMaterial(toolController.cartMaterialChanger.highlightMaterial);
			}
			else if (itemData.IsFuel(heldItem) && toolController.collidesWithTrain)
			{
				toolController.cartMaterialChanger.ChangeMaterial(toolController.cartMaterialChanger.highlightMaterial);
			}
			else if (toolController.collidesWithTrackCart && toolController.trackCrafter.tracksReady > 0 && !IsHoldingItem())
			{
				toolController.trackCrafter.TrackCartMaterial(toolController.trackCrafter.trackCartAssist.highlightMaterial);
			}
			else
			{
				if (toolController.cartMaterialChanger != null)
				{
					toolController.cartMaterialChanger.ChangeMaterial(toolController.cartMaterialChanger.defaultMaterial);
				}
				if (toolController.trackCrafter != null)
				{
					toolController.trackCrafter.TrackCartMaterial(toolController.trackCrafter.trackCartAssist.defaultMaterial);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

		if (itemData.IsPickableItem(other.gameObject))
		{
			pickableItem = other.gameObject;

			if (pickableItem != null && IsHoldingItem() && itemData.IsPickup(pickableItem) && 
				heldItem.tag.Equals(pickableItem.tag) && pickupAmount < maxPickupAmount)
			{
				PickItem(pickableItem);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

		if (itemData.IsPickableItem(other.gameObject))
		{
			pickableItem = null;
		}
	}

	private bool CanPlaceTrack()
	{
		return toolController.CanPlaceTrack() && trackPlacer.IsNextToTrack(toolController.transform.position) && itemData.IsTrackpickup(heldItem);
	}

	private bool CanPlaceBridge()
	{
		Vector3 toolPos = toolCheckbox.transform.position;
		return toolController.collidesWithRavine && itemData.IsWood(heldItem) && levelData.isInBounds((int)toolPos.x, (int)toolPos.z);
	}

	private bool CanHitBreakable(GameObject breakable)
	{
		if (breakable != null)
		{
			string name = breakable.transform.parent.name;
			return (((name == "Rock(Clone)" || name == "Fuel(Clone)") && itemData.IsPickaxe(heldItem)) ||
					(name == "Cactus(Clone)" && itemData.IsAxe(heldItem)));
		}
		else
		{
			return false;
		}
	}

	private bool IsHoldingItem()
	{
		return heldItem != null;
	}

	private bool IsPickableTrack(GameObject other)
	{
		return (other != null && itemData.IsTrack(other) && (!trackPlacer.placedTracks.Contains(other) || (trackPlacer.LastTrack() && !trackPlacer.TrainOnTrack())));
	}

	private AudioClip PlayHitRockSound()
	{
		return audioClips[UnityEngine.Random.Range(2, 4)];
	}

	private AudioClip PlayHitWoodSound()
	{
		return audioClips[UnityEngine.Random.Range(0, 2)];
	}

	private AudioClip PlayPlaceTrackSound()
	{
		return audioClips[4];
	}

	private AudioClip PlayPlaceResourcesSound()
	{
		return audioClips[5];
	}

	private AudioClip PlayStepSound()
	{
		return audioClips[6];
	}
}
