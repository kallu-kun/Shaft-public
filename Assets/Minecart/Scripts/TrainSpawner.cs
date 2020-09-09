using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
	public TrackData trackData;
	public List<GameObject> Train;

	public GameObject FirstCart;
	public GameObject SecondCart;
	public GameObject ThirdCart;

	private TrackPlacer TP;
	private CartsManager CM;
	// Start is called before the first frame update
	void Awake()
	{

		TP = GetComponent<TrackPlacer>();

		Train.Add(FirstCart);
		Train.Add(SecondCart);
		Train.Add(ThirdCart);

		SpawnTrain();
		SpawnTracks();
		CM.Initialize(trackData.trackList);
	}

	private void SpawnTrain()
	{
		for (int i = 0; i < Train.Count; i++)
		{
			GameObject Cart = Instantiate(Train[i], new Vector3(Train.Count - i, 0.0001f, 2), transform.rotation * Quaternion.Euler(0f, 90f, 0f));
			if(i == 0)
			{
				CM = Cart.GetComponent<CartsManager>();
			}
			else if (i == 1)
			{
				Cart.transform.position = new Vector3(Cart.transform.position.x - 0.5f, Cart.transform.position.y, Cart.transform.position.z);
			}
			else if (i == 2)
			{
				Cart.transform.position = new Vector3(Cart.transform.position.x - 0.7f, Cart.transform.position.y, Cart.transform.position.z);
			}
			CM.carts[i] = Cart;
		}
	}

	private void SpawnTracks()
	{
		trackData.Initialise();
		TP.Initialize();

		for (int i = 0; i < 6; i++)
		{
			TP.PlaceTrack(new Vector3(i, 0, 2));
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
