using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnerInstantiate : MonoBehaviour
{
	[SerializeField]
	private GameObject player1Prefab = null;

	[SerializeField]
	private GameObject player2Prefab = null;

	[SerializeField]
	private Transform spawnPosition = null;

	public InputActionAsset inputActions;

	private PlayerInput player1;
	private PlayerInput player2;

	void Awake()
	{
		// Instantiate(player1, player1SpawnPosition, Quaternion.identity, transform);
		// Instantiate(player2, player2SpawnPosition, Quaternion.identity, transform);

		// Liikkuminen näppäimistöllä
		player1 = PlayerInput.Instantiate(player1Prefab, -1, "Keyboard&Mouse", -1, Keyboard.current);
		player1.transform.parent = transform;
		// player1.transform.position = spawnPosition.position;
		

		// TÄMÄ TOIMII (jotenkin äksdee), molemmat pelaajat liikkuvat ohjaimella, muista vaihtaa scheme gamepadiin editorista
		// PlayerInput.Instantiate(player1, -1, "Gamepad", -1, Gamepad.all[0]);
        if (player2Prefab != null && !PlayerSelect.onePlayerMode)
        {
            player2 = PlayerInput.Instantiate(player2Prefab, 1, "Gamepad", -1, Gamepad.current);
            player2.transform.parent = transform;
        }
		// player2.transform.position = spawnPosition.position + new Vector3(0, 0, 2);

		/*
		if (player1Input.actions == null)
		{
			player1Input.actions = inputActions;
			player1Input.defaultControlScheme = "Keyboard&Mouse";
		}

		
		if (player2Input.actions == null)
		{
			player2Input.actions = inputActions;
			player2Input.defaultControlScheme = "Gamepad";
		}
		*/	
	}

	private void Start()
	{
		SetSpawnPosition();
	}

	private void SetSpawnPosition()
	{
		player1.transform.position = spawnPosition.position;

        if (player2 != null)
        {
            player2.transform.position = spawnPosition.position + new Vector3(0, 0, 2);
        }
	}

}
