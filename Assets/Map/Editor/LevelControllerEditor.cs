using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelController))]
public class LevelControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelController controller = (LevelController)target;

        if (Application.isPlaying)
        {
            if (GUILayout.Button("Generate more level"))
            {
                controller.CreateLevel(false);
            }
        }
    }
}
