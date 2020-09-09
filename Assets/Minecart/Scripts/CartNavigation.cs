using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartNavigation : MonoBehaviour
{
	public GameObject Cart;
	public float MoveSpeed;
	//float Timer;
	private Vector3 nextTrackPosition;
	private Vector3 trainTurnPosition;
	public int CurrentTrack;
	private Vector3 startPosition;
	private bool atLastTrack;

	public GameObject firstCart;
	public CartsManager CM;

	public AudioClip cartSound;
	public AudioSource audioSource;
	private bool playedCartSound;
	private bool rigidbodyAdded;

	// Start is called before the first frame update
	public void Initialize()
	{
		CM = firstCart.GetComponent<CartsManager>();
		atLastTrack = false;
	}

	public void CheckNode()
	{
		//Timer = 0;
		startPosition = CM.TrackPath[CurrentTrack].transform.position;
		CurrentTrack++;
		nextTrackPosition = CM.TrackPath[CurrentTrack].transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		//Timer += Time.deltaTime * MoveSpeed;
		//if (Timer > 1)
		//{
			//Timer = 1;
		//}

		if ((Cart.transform.position == nextTrackPosition) & CurrentTrack != CM.TrackPath.Count - 1)
		{
			atLastTrack = false;
			CheckNode();
		}
		else if ((Cart.transform.position == nextTrackPosition) & CurrentTrack == CM.TrackPath.Count - 1)
		{
			atLastTrack = true;
		}

		if (CanMove() && !playedCartSound)
		{
			audioSource.PlayOneShot(cartSound);
			playedCartSound = true;
		}

		RaycastHit hit;
		Ray ray = new Ray(transform.position, Vector3.down);

		if (Physics.Raycast(ray, out hit, 10))
		{
			if (hit.collider.CompareTag("DeathWall") && !rigidbodyAdded)
			{
				rigidbodyAdded = true;
				Rigidbody rb = gameObject.AddComponent<Rigidbody>();
			}
		}
	}
	public void MoveCart()
	{
		//Cart.transform.position = Vector3.Lerp(startPosition, nextTrackPosition, Timer);

		Cart.transform.position = Vector3.MoveTowards(transform.position, nextTrackPosition + new Vector3(0, 0.0001f, 0), MoveSpeed * Time.deltaTime);

		if (Vector3.Distance(transform.position, nextTrackPosition) < 0.001f)
		{
			Cart.transform.position = nextTrackPosition;
		}

		trainTurnPosition = TrainTurnCoordinates();

		transform.forward = Vector3.RotateTowards(transform.forward,
			trainTurnPosition - transform.position, MoveSpeed * Time.deltaTime, 0.0f);
	}

	private Vector3 TrainTurnCoordinates()
	{
		if ((CurrentTrack + 1) < (CM.TrackPath.Count))
		{
			return CM.TrackPath[CurrentTrack + 1].transform.position;
		}
		else
		{
			return nextTrackPosition;
		}
	}

	public bool CanMove()
	{
		if (!atLastTrack)
		{
			return (CurrentTrack <= CM.TrackPath.Count - 1);
		}
		else
		{
			playedCartSound = false;
			return false;
		}
	}
}
