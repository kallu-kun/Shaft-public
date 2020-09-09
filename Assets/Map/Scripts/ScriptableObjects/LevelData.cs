using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    public int levelWidth = 20;
    public int levelLength = 35;

    public float cameraspeed = 0.1f;

    public bool generateMoreLevel;

    public bool isInBounds(int x, int z)
    {
        return x >= 0 && z >= 0 && z < levelWidth;
    }
}