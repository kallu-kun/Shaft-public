using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelPooler : MonoBehaviour
{
    private GameObject[] voxels;
    public ObjectPool[] voxelPools;

    public void Initialise()
    {
        voxels = GetComponentInParent<LevelController>().voxelData.GetAllVoxels();
        voxelPools = new ObjectPool[voxels.Length];
        CreatePools();
    }

    public void CreatePools()
    {
        for (int i = 0; i < voxelPools.Length; i++)
        {
            if (voxels[i] != null)
            {
                voxelPools[i] = gameObject.AddComponent<ObjectPool>();
                voxelPools[i].obj = voxels[i];
                voxelPools[i].PoolObjects(9000);
            }
        }
    }

    public void DeactivateAll()
    {
        foreach (ObjectPool pool in voxelPools)
        {
            if (pool != null)
            {
                pool.DeActivateAll();
            }
        }
    }

    public ObjectPool GetPool(string name)
    {
        ObjectPool result = null;
        for (int i = 0; i < voxels.Length; i++)
        {
            if (name.Equals(voxels[i].name))
            {
                result = voxelPools[i];
            }
        }

        return result;
    }
}
