using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelController : MonoBehaviour {

    private TestLevelObjectData prefabData;

    public GenerationType generationType;
    public int density = 1;
    public TestLevelObjectMap map;

    private ObjectPool groundPool;
    private ObjectPool rockPool;
    private ObjectPool woodPool;

    public int groundWidth = 30;
    public int groundLength = 15;

    public void Initialise() {
        prefabData = GetComponent<TestLevelObjectData>();
        groundPool = prefabData.groundParent.GetComponent<ObjectPool>();
        rockPool = prefabData.rockParent.GetComponent<ObjectPool>();
        woodPool = prefabData.woodParent.GetComponent<ObjectPool>();

        map = new TestLevelObjectMap(this);
    }

    public void CreatePools() {
        int groundAmount = groundWidth * groundLength;
        CreatePoolInEditMode(groundPool, groundAmount);

        CreatePoolInEditMode(woodPool, 200);
        CreatePoolInEditMode(rockPool, 200);
    }

    private void CreatePoolInEditMode(ObjectPool pool, int objectAmount) {
        if (objectAmount != pool.objectPool.Length) {
            pool.DestroyAll();
            pool.PoolObjects(objectAmount);
        } else {
            pool.PoolObjectsFromParent();
        }
    }

    public void CreateLevel() {
        CreatePools();
        SetObjectsToArea(groundPool, 0, 0, groundWidth, groundLength, 2);

        SpawnObjects();
    }

    private void SpawnObjects() {
        for (int z = 0; z < groundLength; z++) {
            for (int x = 0; x < groundWidth; x++) {
                MapObjectEnum mapObject = map.mapObjects[z, x];

                ObjectPool pool = null;
                switch (mapObject) {
                    case MapObjectEnum.Rock: pool = rockPool; break;
                    case MapObjectEnum.Wood: pool = woodPool; break;
                }
                
                if (mapObject != MapObjectEnum.Empty) {
                    Vector3 loc = new Vector3(x + 0.5f, 0.0f, z + 0.5f);
                    GameObject obj = pool.GetInactiveObject(true);
                    obj.transform.position = loc;
                }
            }
        }
    }

    private void SetObjectsToArea(ObjectPool pool, int startX, int startZ, int width, int length, int interval) {
        for (int z = startZ; z < startZ + length; z += interval) {
            for (int x = startX; x < startX + width; x += interval) {
                Vector3 loc = new Vector3(x + interval/2.0f, 0.0f, z + interval/2.0f);
                GameObject obj = pool.GetInactiveObject(true);
                obj.transform.position = loc;
            }
        }
    }

    private void DestroyAllChildren(GameObject obj) {
        for (int i = 0; i < obj.transform.childCount; i++) {
            DestroyImmediate(obj.transform.GetChild(1).gameObject);
        }
    }

    private void AddObjectToMap(GameObject obj, Vector3 loc) {
        Instantiate(obj, loc, Quaternion.identity);
    }
}