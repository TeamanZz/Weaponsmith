using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetsHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        DungeonObjectData obj = EditorUtility.InstanceIDToObject(instanceId) as DungeonObjectData;

        if(obj!= null)
        {
            DungeonEditorWindow.Open(obj);
            return true;
        }
        
        return false;
    }
}

[CustomEditor(typeof(DungeonObjectData))]
public class DungeonCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        if (GUILayout.Button("Open Editor"))
        {
            DungeonEditorWindow.Open((DungeonObjectData)target);
        }

    }


}
