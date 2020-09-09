using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxelData : ScriptableObject
{
    public GameObject[] grounds;
    public GameObject[] objects;

    public GameObject[] walls;
    public GameObject waterfall;

    public GameObject GetVoxel(string name)
    {
        GameObject result = null;

        for (int i = 0; i < grounds.Length || i < objects.Length; i++)
        {
            if (i < grounds.Length && grounds[i].name.Equals(name))
            {
                result = grounds[i];
                break;
            }
            else if (i < objects.Length && objects[i].name.Equals(name)) {
                result = objects[i];
                break;
            }
        }

        return result;
    }

    public GameObject[] GetAllVoxels()
    {
        GameObject[] result = new GameObject[grounds.Length + objects.Length];

        for (int i = 0; i < grounds.Length + objects.Length; i++)
        {
            result[i] = i < grounds.Length ? grounds[i] : objects[i - grounds.Length];
        }

        return result;
    }

    public GameObject GetRandomWall()
    {
        return walls[Random.Range(0, walls.Length)];
    }
}
