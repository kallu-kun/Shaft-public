using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestLevelController))]
public class TestLevelEditor : Editor {

    public override void OnInspectorGUI() {
        TestLevelController controller = (TestLevelController)target;
        controller.Initialise();

        EditorGUILayout.LabelField("Level Bounds: ");
        controller.groundLength = EditorGUILayout.IntSlider("Length", controller.groundLength, 15, 20);
        controller.groundWidth = EditorGUILayout.IntSlider("Width", controller.groundWidth, 30, 50);

        EditorGUILayout.LabelField("Generation: ");
        string[] generationOptions = { "default", "scattered" };
        controller.generationType = (GenerationType)EditorGUILayout.Popup("Generation Type", (int)controller.generationType, System.Enum.GetNames(typeof(GenerationType)));

        controller.density = EditorGUILayout.IntSlider("Density", controller.density, 1, 5);

        if (GUILayout.Button("Build Level")) {
            controller.CreateLevel();
        }
    }
}
