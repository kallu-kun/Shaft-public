using UnityEngine;

public class TestLevelObjectMap {
    private TestLevelController controller;

    public MapObjectEnum[,] mapObjects;

    private int width;
    private int length;

    public TestLevelObjectMap(TestLevelController controller) {
        this.controller = controller;

        width = controller.groundWidth;
        length = controller.groundLength;
        mapObjects = new MapObjectEnum[length, width];

        GenerateLevel(controller.generationType);
    }

    private void GenerateLevel(GenerationType generationType) {

        switch (generationType) {
            case GenerationType.Default: DefaultTestGeneration(); break;
            case GenerationType.Scattered: ScatteredGeneration(); break;
        }
    }

    public void DefaultTestGeneration() {
        int areaWidth = width / 6 + controller.density;
        int areaLength = length / 6 + controller.density;
        SetObjectsToArea(MapObjectEnum.Rock, width / 6, 0, areaWidth, areaLength);
        SetObjectsToArea(MapObjectEnum.Wood, width / 6 * 3, 0, areaWidth, areaLength);
    }

    private void SetObjectsToArea(MapObjectEnum obj, int startX, int startZ, int width, int length) {
        for (int z = startZ; z < startZ + length; z ++) {
            for (int x = startX; x < startX + width; x++) {
                mapObjects[z, x] = obj;
            }
        }
    }

    private void ScatteredGeneration() {
        for (int z = 0; z < length; z++) {
            for (int x = 3; x < width; x++) {
                int probCap = 35 / controller.density;
                int objectIndex = Random.Range(0, probCap);
                mapObjects[z, x] = MapObjectFromInt(objectIndex, probCap);
            }
        }
    }

    private MapObjectEnum MapObjectFromInt(int i, int cap) {
        MapObjectEnum result = MapObjectEnum.Empty;
        if (i == cap - 1) {
            result = MapObjectEnum.Rock;
        } else if (i == cap - 2) {
            result = MapObjectEnum.Wood;
        }

        return result;
    }
}
