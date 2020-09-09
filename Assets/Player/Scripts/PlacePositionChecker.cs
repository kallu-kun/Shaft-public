using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePositionChecker : MonoBehaviour
{
    Collider collider;

    private int dirIndex = 0;

    public bool positionIsBlocked;

    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<Collider>();
    }

    public void MoveAround()
    {
        dirIndex++;
        Debug.Log(dirIndex);
        transform.localPosition = GetMoveDirection();
    }

    public void ResetPosition()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        dirIndex = 0;
    }

    private Vector3 GetMoveDirection()
    {
        Vector3 result = new Vector3(0, 0, 0);

        switch (dirIndex)
        {
            case 0:
                break;
            case 1:
                result.x += 1;
                break;
            case 2:
                result.x += 1;
                result.z += 1;
                break;
            case 3:
                result.z += 1;
                break;
            case 4:
                result.x -= 1;
                result.z += 1;
                break;
            case 5:
                result.x -= 1;
                break;
            case 6:
                result.x -= 1;
                result.z -= 1;
                break;
            case 7:
                result.z -= 1;
                break;
            case 8:
                result.x += 1;
                result.z -= 1;
                break;
        }

        int dirMulti = 1 + (int)Mathf.Floor(dirIndex / 8.0f);
        Debug.Log(dirMulti);
        result.x *= dirMulti;
        result.z *= dirMulti;

        return result;
    }

    private void OnTriggerEnter(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        if (other.tag != "Ground")
        {
            positionIsBlocked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        if (other.tag != "Ground")
        {
            positionIsBlocked = false;
        }
    }
}
