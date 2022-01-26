using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DungeonEditorWindow : ExtendedEditorWindow
{
    public static void Open(DungeonObjectData dungeonData)
    {
        DungeonEditorWindow window = GetWindow<DungeonEditorWindow>("Dungeon Data Editor");
        window.serializedObject = new SerializedObject(dungeonData);
    }

    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("DungeonObjectData");
        DrawProperties(currentProperty, true);
    }
}
