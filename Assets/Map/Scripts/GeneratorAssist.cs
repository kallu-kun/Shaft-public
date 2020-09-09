using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorAssist
{
    private MapGenerator generator;

    private VoxelData voxelData;

    private GameObject greenGround;
    private GameObject lightGreenGround;
    private GameObject purpleGround;
    private GameObject deepPurpleGround;
    private GameObject redGround;
    private GameObject pinkGround;
    private GameObject lightPinkGround;

    private GameObject ravine;
    private GameObject river1;
    private GameObject river2;

    private GameObject cactus;
    private GameObject rock;
    private GameObject fuel;

    private GameObject unBreakable;

    private GameObject[,,] map;

    public GeneratorAssist(MapGenerator generator)
    {
        this.generator = generator;
        voxelData = generator.voxelData;
        map = generator.map;

        AddVoxelReferences();
    }

    private void AddVoxelReferences()
    {
        greenGround = voxelData.GetVoxel("Green_ground");
        lightGreenGround = voxelData.GetVoxel("Light_Green_ground");
        deepPurpleGround = voxelData.GetVoxel("Deep_purple_ground");
        purpleGround = voxelData.GetVoxel("Purple_ground");
        redGround = voxelData.GetVoxel("Red_ground");
        pinkGround = voxelData.GetVoxel("Pink_ground");
        lightPinkGround = voxelData.GetVoxel("Light_Pink_ground");

        ravine = voxelData.GetVoxel("Ravine");
        river1 = voxelData.GetVoxel("Water_block_lighter");
        river2 = voxelData.GetVoxel("Water_block_darker");

        cactus = voxelData.GetVoxel("Cactus");
        rock = voxelData.GetVoxel("Rock");
        fuel = voxelData.GetVoxel("Fuel");

        unBreakable = voxelData.GetVoxel("Unbreakable_1x1");
    }

    public void ClearObject(int x, int z)
    {
        map[x, z, 1] = null;
    }

    public void SetCactus(int x, int z)
    {
        map[x, z, 1] = cactus;
        map[x, z, 0] = greenGround;
    }

    public void SetRock(int x, int z)
    {
        map[x, z, 1] = rock;
        map[x, z, 0] = purpleGround;
    }

    public void AddUnbreakable(int x, int z)
    {
        map[x, z, 1] = unBreakable;
        map[x, z, 0] = deepPurpleGround;
    }

    public void AddCheckpoint(int placeX)
    {
        Vector3 flagLoc = new Vector3(placeX, 0, map.GetLength(1) / 2);

        for (int x = (int)flagLoc.x - 3; x <= flagLoc.x + 3; x++)
        {
            for (int z = (int)flagLoc.z - 2; z <= flagLoc.z + 2; z++)
            {
                map[x, z, 1] = null;
                map[x, z, 0] = pinkGround;
            }
        }

        map[(int)flagLoc.x, (int)flagLoc.z, 1] = voxelData.GetVoxel("Checkpoint_flag");
    }

    public void AddRockRectangle(int startX, int startZ, int length, int width)
    {
        AddVoxelRectangle(rock, purpleGround, startX, startZ, length, width);
    }

    public void AddCactusRectangle(int startX, int startZ, int length, int width)
    {
        AddVoxelRectangle(cactus, greenGround, startX, startZ, length, width);
    }

    public void AddFuelRectangle(int startX, int startZ, int length, int width)
    {
        AddVoxelRectangle(fuel, deepPurpleGround, startX, startZ, length, width);
    }

    public void AddRiver(int startX, int width, int length)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int z = map.GetLength(1); z > map.GetLength(1) - length - 1; z--)
            {
                if (IsInBounds(x, z))
                {
                    GameObject riverBlock = Random.Range(0.0f, 1.0f) < 0.5f ? river1 : river2;
                    map[x, z, 0] = riverBlock;
                    map[x, z, 1] = null;
                }
            }
        }
    }

    public bool AddRandomRavine(int startX)
    {
        int length = Random.Range(10, map.GetLength(1));
        int width = Random.Range(2, 5);

        bool hasWaterFall = false;

        if (Random.Range(0, 9) <= 5)
        {
            width = 3;
            AddRiver(startX, width, length);
            hasWaterFall = true;
        }
        else
        {
            AddCurvedRavine(startX, width, length, Random.Range(1, 4), Random.Range(0.0f, 1.0f) < 0.5f);
        }

        return hasWaterFall;
    }

    public void AddRavine(int startX, int width, int length)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int z = map.GetLength(1); z > map.GetLength(1) - length - 1; z--)
            {
                SetRavine(x, z);
            }
        }
    }

    private void SetRavine(int x, int z)
    {
        if (IsInBounds(x, z))
        {
            map[x, z, 0] = ravine;
            map[x, z, 1] = null;
        }
    }

    public void AddCurvedRavine(int startX, int width, int length, int offset, bool reverse = false)
    {
        bool curvingForward = !reverse;
        int currentOffset = offset - 1;
        for (int z = map.GetLength(1); z > map.GetLength(1) - length - 1; z--)
        {
            for (int x = startX; x < startX + width; x++)
            {
                int placeX = x + currentOffset;
                SetRavine(placeX, z);
            }

            currentOffset += curvingForward ? 1 : -1;
            if (Mathf.Abs(currentOffset) == offset)
            {
                curvingForward = !curvingForward;
            }
        }
    }

    public void AddVoxelRectangle(GameObject voxel, GameObject groundVoxel, int startX, int startZ, int length, int width, GameObject borderGround = null)
    {
        int borderWidth = borderGround == null ? 0 : 1;

        for (int x = startX - borderWidth; x < length + startX + borderWidth; x++)
        {
            for (int z = startZ - borderWidth; z < width + startZ + borderWidth; z++)
            {
                if (IsInBounds(x, z) && !IsRavine(x, z))
                {
                    if (x >= startX && x < length + startX && z >= startZ && z < width + startZ)
                    {
                        SetVoxel(x, z, 1, voxel);
                        SetVoxel(x, z, 0, groundVoxel);
                    }
                    else
                    {
                        SetVoxel(x, z, 0, groundVoxel, 0.5f, borderGround);
                    }
                }
            }
        }
    }

    private void SetVoxel(int x, int z, int y, GameObject voxel, float setChance = 1, GameObject otherVoxel = null)
    {
        if (setChance == 1)
        {
            map[x, z, y] = voxel;
        }
        else
        {
            float f = Random.Range(0.0f, 1.0f);

            if (f <= setChance)
            {
                map[x, z, y] = voxel;
            }
            else if (otherVoxel != null)
            {
                map[x, z, y] = otherVoxel;
            }
        }
    }

    public void FillGround()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int z = 0; z < map.GetLength(1); z++)
            {
                SetVoxel(x, z, 0, pinkGround, 0.5f, lightPinkGround);
            }
        }
    }

    public bool IsRavine(int x, int z)
    {
        return map[x, z, 0] != null && map[x, z, 0].Equals(ravine);
    }

    public bool IsInBounds(int x, int z)
    {
        return x >= 0 && z >= 0 && x < map.GetLength(0) && z < map.GetLength(1);
    }
}
