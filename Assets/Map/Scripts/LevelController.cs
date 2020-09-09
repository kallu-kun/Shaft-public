using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject deathWall;
    private GameObject nDeathWall;

    [SerializeField]
    private GameObject fogParticles;
    [SerializeField]
    private GameObject dustParticles;

    [Header("Data")]
    [SerializeField]
    public VoxelData voxelData = null;
    [SerializeField]
    public LevelData levelData = null;
    [SerializeField]
    public TrackData trackData = null;

    [Header("Generation")]
    private int levelWidth;
    public int levelLength;
    private int currentLength;

    [SerializeField]
    private bool collapseMap;
    [SerializeField]
    private float initialCollapseDelay = 40.0f;
    [SerializeField]
    private bool animateGeneration;

    private VoxelPooler voxelPooler;
    private MapGenerator generator;

    private GameObject[,,] mapObjects;
    private List<GameObject[,]> mapRows;
    private List<GameObject> walls;

    private float startingHeight;
    private float mapDescentSpeed = 1.5f;
    private float mapDescentWaitTime = 0.03f;

    private Timer collapseTimer;
    private int collapsingRow = 0;

    private void Awake()
    {
        levelWidth = levelData.levelWidth;
        levelLength = levelData.levelLength;

        levelData.cameraspeed = 0.1f;
    }

    private void Start() {
        voxelPooler = GetComponentInChildren<VoxelPooler>();
        voxelPooler.Initialise();

        collapseTimer = gameObject.AddComponent<Timer>();
        collapseTimer.duration = initialCollapseDelay;
        collapseTimer.StartTimer();

        mapRows = new List<GameObject[,]>();
        CreateLevel(true);

        nDeathWall = Instantiate(deathWall, new Vector3(-1, -1, 10), Quaternion.identity, gameObject.transform);
    }

    private void Update()
    {
        CollapseMap();

        if (levelData.generateMoreLevel)
        {
            CreateLevel(false);
            levelData.generateMoreLevel = false;
        }
    }

    private void CollapseMap()
    {
        if (collapseTimer.isFinished && collapseMap)
        {
            StartCoroutine(CollapseRow(collapsingRow));
            nDeathWall.transform.Translate(new Vector3(1, 0, 0));
            collapsingRow++;

            collapseTimer.duration = 1 / levelData.cameraspeed;
            collapseTimer.StartTimer();
        }
    }

    public void CreateLevel(bool start)
    {
        generator = new MapGenerator(voxelData, levelData, levelWidth, levelLength);
        GameObject[,,] voxels = generator.GenerateMap(start);

        startingHeight = animateGeneration ? 20.0f : 0.0f;

        RenderMap(voxels);
        RenderWalls();

        AddMapParticles();

        currentLength += levelLength;

        StartCoroutine(MoveMapDown(mapObjects));
    }

    private void RenderMap(GameObject[,,] map)
    {
        mapObjects = new GameObject[map.GetLength(0), map.GetLength(1), map.GetLength(2)];

        for (int x = 0; x < levelLength; x++)
        {
            GameObject[,] mapRow = new GameObject[levelWidth, 2];
            for (int z = -1; z <= levelWidth; z++)
            {
                for (int y = 0; y < map.GetLength(2); y++)
                {
                    GameObject voxel = null;
                    Vector3 loc = new Vector3();

                    if (IsLevelBorder(map, x, z))
                    {
                        voxel = voxelData.GetVoxel("Ravine");
                        loc = new Vector3(x + currentLength, y - 1, z);
                    }
                    else
                    {
                        voxel = map[x, z, y];
                        loc = new Vector3(x + currentLength, y + startingHeight - 1, z);
                    }

                    if (voxel != null)
                    {
                        ObjectPool pool = voxelPooler.GetPool(voxel.name);
                        GameObject obj = pool.GetInactiveObject(true);

                        obj.transform.position = loc;

                        if (!IsLevelBorder(map, x, z))
                        {
                            mapObjects[x, z, y] = obj;
                            mapRow[z, y] = obj;
                        }
                    }
                }
            }
            mapRows.Add(mapRow);
        }

        GameObject waterfall = voxelData.waterfall;
        for (int i = 0; i < generator.waterFallXLocations.Count; i++)
        {
            Vector3 waterfallLoc = new Vector3(generator.waterFallXLocations[i] + currentLength, waterfall.transform.position.y, map.GetLength(1) - 1);

            Instantiate(waterfall, waterfallLoc, Quaternion.identity, transform);
        }
    }

    private bool IsLevelBorder(GameObject[,,] map, int x, int z)
    {
        return z == -1 || z == map.GetLength(1);
    }

    private void RenderWalls()
    {
        walls = new List<GameObject>();
        for (int i = 0; i < levelLength; i += 10)
        {
            Vector3 loc = new Vector3(i + 5 + currentLength, 5 + startingHeight, levelWidth + 1);
            GameObject obj = Instantiate(voxelData.GetRandomWall(), loc, Quaternion.Euler(0, -90, 0));
            walls.Add(obj);
        }
    }
    
    private IEnumerator MoveMapDown(GameObject[,,] map)
    {
        for (int y = 0; y < map.GetLength(2); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int z = 0; z < map.GetLength(1); z++)
                {
                    GameObject obj = map[x, z, y];

                    if (obj != null)
                    {
                        StartCoroutine(MoveBlockDown(obj.transform, true));
                    }
                }

                if (x % 10 == 0 && y == 1)
                {
                    StartCoroutine(MoveBlockDown(walls[x / 10].transform, true));
                }

                yield return new WaitForSeconds(mapDescentWaitTime);
            }
        }
    }

    private IEnumerator MoveBlockDown(Transform transform, bool loopyDescent = false)
    {
        if (transform != null)
        {
            Vector3 currentLoc = transform.position;
            Vector3 newLoc = new Vector3(currentLoc.x, currentLoc.y - startingHeight, currentLoc.z);

            while (Vector3.Distance(transform.position, newLoc) > 0.1f)
            {
                Vector3 destination = newLoc;
                if (transform.position.y > (newLoc.y + 1) * 1.1f && loopyDescent)
                {
                    float zLocFromCenter = destination.z - levelWidth / 2;
                    destination.z += zLocFromCenter;
                    destination.y *= 1.2f;
                }

                transform.position = Vector3.Lerp(transform.position, destination, mapDescentSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = newLoc;
        }
    }

    private IEnumerator CollapseRow(int collapsingRow)
    {
        List<GameObject[]> rowBlocks = new List<GameObject[]>();
        for (int i = 0; i < levelWidth; i++)
        {
            GameObject[] objects = new GameObject[2];
            for (int j = 0; j < 2; j++)
            {
                objects[j] = mapRows[0][i, j] ?? null;
            }

            rowBlocks.Add(objects);
        }

        mapRows.RemoveAt(0);

        for (int i = 0; i < levelWidth; i++)
        {
            int collapsingBlockIndex = Random.Range(0, rowBlocks.Count);

            GameObject[] collapsingBlocks = rowBlocks[collapsingBlockIndex];
            StartCoroutine(CollapseBlocks(collapsingBlocks));
            rowBlocks.Remove(collapsingBlocks);

            yield return new WaitForSeconds(collapseTimer.duration / 50);
        }
    }

    private IEnumerator CollapseBlocks(GameObject[] collapsingBlocks)
    {
        Transform blockTransform = collapsingBlocks[0].transform;
        Vector3 newLoc = blockTransform.position;
        newLoc -= new Vector3(0, 20, 0);

        float descentSpeed = 0.3f;

        while (Vector3.Distance(blockTransform.position, newLoc) > 1.5f)
        {
            for (int i = 0; i < collapsingBlocks.Length; i++)
            {
                if (collapsingBlocks[i] != null)
                {
                    collapsingBlocks[i].transform.position = Vector3.Lerp(collapsingBlocks[i].transform.position, newLoc, descentSpeed * Time.deltaTime);
                }
            }
            descentSpeed *= 1.1f;

            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < collapsingBlocks.Length; i++)
        {
            if (collapsingBlocks[i] != null)
            {
                collapsingBlocks[i].SetActive(false);
            }
        }
    }

    private void AddMapParticles()
    {
        Vector3 fogPos = fogParticles.transform.position;
        fogPos.x += currentLength;

        Vector3 dustPos = dustParticles.transform.position;
        dustPos.x += currentLength;

        Instantiate(fogParticles, fogPos, Quaternion.identity);
        Instantiate(dustParticles, dustPos, Quaternion.identity);
    }

    private void DestroyWalls()
    {
        if (walls != null)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                Destroy(walls[i]);
            }
        }
    }
    
    private Vector3 LocationAdjustVector(Voxel voxel)
    {
        float offSet = voxel.size / 2;
        print(offSet);
        return new Vector3(offSet, 0, offSet);
    }
}
