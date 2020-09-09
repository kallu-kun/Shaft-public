using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float fuel;
    public float speed;
    public bool isMoving;

    [SerializeField]
    private float maxSpeed = 1;
    [SerializeField]
    private float maxFuel = 5;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                ChangeSpeed();
                if (fuel > 0)
                {
                    fuel -= 0.04f;
                }
                timer = 0;
            }
        }
        if (fuel > maxFuel)
        {
            fuel = maxFuel;
        }
        else if (fuel < 0)
        {
            fuel = 0;
        }
    }

    private void ChangeSpeed()
    {
        speed = maxSpeed * fuelInStorage();
    }

    private float fuelInStorage()
    {
        return (fuel / maxFuel);
    }

    private float currentSpeed()
    {
        return (speed / maxSpeed);
    }

    public void AddResources(int added)
    {
        fuel += (float)added + 0.5f;
    }
}
