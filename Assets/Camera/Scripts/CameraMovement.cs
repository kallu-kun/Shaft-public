using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;

	private bool cameraIsMoving;

	void Start()
	{
		cameraIsMoving = true;
	}

	
	void Update()
	{
		if (cameraIsMoving)
		{
			transform.position += new Vector3(levelData.cameraspeed, 0, 0) * Time.deltaTime;
		}
	}
}
