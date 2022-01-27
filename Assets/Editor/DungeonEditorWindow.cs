using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DungeonEditorWindow : ExtendedEditorWindow
{
    public static void Open(DungeonObjectData dungeonData)
    {
        Debug.Log(dungeonData);
        DungeonEditorWindow window = GetWindow<DungeonEditorWindow>("Dungeon Enviroments Editor");
        window.serializedObject = new SerializedObject(dungeonData);
    }

    private void OnGUI()
    {

    }
}
