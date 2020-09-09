using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    protected int width;
    protected int length;
    protected int height = 2;

    public GeneratorAssist assist;

    public VoxelData voxelData;
    private LevelData levelData;
    public GameObject[,,] map { get; protected set; }
    public List<int> waterFallXLocations;

    public MapGenerator(VoxelData voxelData, LevelData levelData, int width, int length)
    {
        this.voxelData = voxelData;
        this.levelData = levelData;

        map = new GameObject[length, width, height];
        assist = new GeneratorAssist(this);
        waterFallXLocations = new List<int>();

        this.width = width;
        this.length = length;
    }

    public GameObject[,,] GenerateMap(bool start)
    {
        InitialiseMap();
        assist.FillGround();

        ScatterGeneration(start);

        return map;
    }

    private void ForestSplashTest()
    {
        assist.AddCactusRectangle(5, 5, 15, 10);
    }

    private void ScatterGeneration(bool start)
    {
        int chunkAmount = 3;
        int chunkLength = map.GetLength(0) / chunkAmount;

        for (int i = 0; i < chunkAmount; i++)
        {
            if (start && i == 0)
            {
                AddStartResources();
            }
            else
            {
                int xMin = chunkLength * i + 1;
                int xMax = xMin + chunkLength - 1;

                AddChunkContents(xMin, xMax);

                if (i == chunkAmount - 1)
                {
                    assist.AddCheckpoint(xMax - 5);
                }
            }
        }
    }

    private void AddStartResources()
    {
        assist.AddRockRectangle(7, 9, 6, 10);
        assist.AddCactusRectangle(1, 9, 6, 10);
        assist.AddFuelRectangle(11, 5, 2, 3);
    }

    private void AddChunkContents(int xMin, int xMax)
    {
        for (int j = 0; j < RandomRange(1, 3); j++)
            assist.AddRockRectangle(RandomRange(xMin, xMax), RandomRange(0, width - 2), RandomRange(6, 10), RandomRange(5, 10));

        for (int j = 0; j < RandomRange(1, 3); j++)
            assist.AddCactusRectangle(RandomRange(xMin, xMax), RandomRange(0, width - 2), RandomRange(6, 10), RandomRange(5, 10));

        assist.AddFuelRectangle(RandomRange(xMin, xMax), RandomRange(0, width - 2), RandomRange(2, 4), RandomRange(2, 4));

        for (int j = 0; j < RandomRange(2, 6); j++)
            assist.AddUnbreakable(RandomRange(xMin, xMax), RandomRange(0, width));

        int ravineX = RandomRange(xMin, xMax);
        bool isRiver = assist.AddRandomRavine(ravineX);

        if (isRiver)
        {
            waterFallXLocations.Add(ravineX + 1);
        }
    }

    private void InitialiseMap()
    {
        for (int x = 0; x < length; x++)
        {
            for (int z = 0; z < width; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, z, y] = null;
                }
            }
        }
    }

    private int RandomRange(int min, int max)
    {
        return Random.Range(min, max);
    }
}
