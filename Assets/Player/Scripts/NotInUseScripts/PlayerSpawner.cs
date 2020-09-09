using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
	private PlayerInputManager playerInputManager;
	private bool playerCanJoin;
	

	private void Start()
	{
		playerInputManager = GetComponent<PlayerInputManager>();
		playerInputManager.EnableJoining();
		playerCanJoin = true;
	}

	private void Update()
	{
		if (playerCanJoin && playerInputManager.playerCount == playerInputManager.maxPlayerCount)
		{
			playerInputManager.DisableJoining();
			playerCanJoin = false;
		}
	}
}
