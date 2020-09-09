using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartsManager : MonoBehaviour
{
    public float cartsSpeed;

    public List<GameObject> TrackPath;

    public GameObject[] carts;
    
    public CartNavigation[] CN;

    private TrackCrafter TC;
    private TrainController trainController;
    [SerializeField]
    private ScoreCalculator scoreCal;

    public void Initialize(List<GameObject>tracks)
    {
        TrackPath = tracks;
        for(int i = 0; i < carts.Length; i++)
        {
            CN[i] = carts[i].gameObject.GetComponent<CartNavigation>();
            CN[i].firstCart = carts[0];
            CN[i].MoveSpeed = cartsSpeed;
            CN[i].Initialize();
            CN[i].CurrentTrack = (carts.Length - 1) - i;
            CN[i].CheckNode();
        }
        TC = carts[1].GetComponent<TrackCrafter>();
        TC.Initialize(carts[2]);
        trainController = carts[0].GetComponent<TrainController>();

        scoreCal.startX = carts[0].transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (CN[0].CanMove())
        {
            for (int i = 0; i < carts.Length; i++)
            {
                CN[i].MoveCart();
                cartsSpeed = trainController.speed;
                CN[i].MoveSpeed = cartsSpeed;
            }
            trainController.isMoving = true;
        }
        else
        {
            trainController.isMoving = false;
        }

        StatsToCalc();
    }

    private void StatsToCalc()
    {
        if (scoreCal != null)
        {
            scoreCal.currentX = carts[0].transform.position.x;
            scoreCal.tracks = TrackPath.Count;
            scoreCal.CalculateScore();
        }
    }
}
