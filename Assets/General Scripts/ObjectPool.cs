using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public GameObject obj { get; set; }
    public GameObject[] objectPool;

    public GameObject parent;
    
    public void PoolObjects(int amountToPool)
    {
        objectPool = new GameObject[amountToPool];

        for (int i = 0; i < amountToPool; i++)
        {
            objectPool[i] = (GameObject) Instantiate(obj);
            objectPool[i].SetActive(false);

            if (parent != null)
            {
                objectPool[i].transform.parent = parent.transform;
            } 
            else
            {
                objectPool[i].transform.parent = gameObject.transform;
            }
        }
    }

    public void PoolObjectsFromParent()
    {
        objectPool = new GameObject[parent.transform.childCount];

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            objectPool[i] = parent.transform.GetChild(i).gameObject;
        }
        DeActivateAll();
    }

    public GameObject GetInactiveObject(bool activate)
    {
        GameObject result = null;

        for (int i = 0; i < objectPool.Length; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                result = objectPool[i];

                if (activate)
                {
                    result.SetActive(true);
                }
                break;
            }
        }

        return result;
    }

    public void DeActivateAll()
    {
        for (int i = 0; i < objectPool.Length; i++)
        {
            if (objectPool[i].activeInHierarchy)
            {
                objectPool[i].SetActive(false);
            }
        }
    }

    public void DestroyAll()
    {
        for (int i = 0; i < objectPool.Length; i++)
        {
            DestroyImmediate(objectPool[i]);
        }
    }
}
