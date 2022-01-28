using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DungeonEditorWindow : ExtendedEditorWindow
{
    public static DungeonObjectData dungeonData; 
    public Vector2 scrollPosition = Vector2.zero;
    public Vector2 piececesScroll = Vector2.zero; 
    
    bool groupEnabled;
    public void CopyDungeonInfo()
    {
        
    }
    public static void Open(DungeonObjectData newDungeonData)
    {
        Debug.Log(newDungeonData.currentName);
        DungeonEditorWindow window = GetWindow<DungeonEditorWindow>("Dungeon Enviroments Editor");
        window.serializedObject = new SerializedObject(newDungeonData);

        dungeonData = newDungeonData;
    }

    private void OnGUI()
    {
        if (dungeonData == null)
            return;

        if(dungeonData.inRealTimeEditor == true)
            GUILayout.Label("Current Dungeon " + "In Real Time " + "Not Saved", EditorStyles.boldLabel);
        else
            GUILayout.Label("Current Dungeon " + dungeonData.currentName + " In DATA", EditorStyles.boldLabel) ;

        GUILayout.Space(7.5f);
        //scrollPosition = GUI.BeginScrollView(new Rect(1, 25, 300, 300), scrollPosition, new Rect(1, 0, 0, 25 * dungeonData.currentPieces.Count));


        groupEnabled = EditorGUILayout.BeginToggleGroup("In Real Time", dungeonData.inRealTimeEditor);
        
        EditorGUILayout.BeginHorizontal();

        dungeonData.currentName = EditorGUILayout.TextField("Enter the title", dungeonData.currentName); 
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("Name of the dungeon - " + dungeonData.currentName);
        

        if (GUILayout.Button("Save New Dungeon"))
        {
            dungeonData.SaveDungeon();

        }
        EditorGUILayout.EndToggleGroup();

        groupEnabled = EditorGUILayout.BeginToggleGroup("In Saves", !dungeonData.inRealTimeEditor);
        
        GUILayout.Label("Saved number - " + PlayerPrefs.GetInt("numberOfRows" + dungeonData.currentNumber) + " | Name - " + PlayerPrefs.GetString("Dungeon" + dungeonData.currentNumber) + " | Pieces count - " + PlayerPrefs.GetInt("piecesDungeonCount" + dungeonData.currentNumber));


        EditorGUILayout.EndToggleGroup();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            dungeonData.SaveUpdatedDungeon();
        }

        if (GUILayout.Button("Clear"))
        {
            dungeonData.RemoveChangesRedoSave();
        }

        GUILayout.EndHorizontal();

        piececesScroll = EditorGUILayout.BeginScrollView(piececesScroll, GUILayout.Height(200));
        for (int i = 0; i < dungeonData.currentPieces.Count; i++)
        {
            if (dungeonData.currentPieces[i] == null)
                break;

            PiecesDungeon pieces = dungeonData.currentPieces[i];
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button((i + 1) + " element"))
            {
                pieces = dungeonData.currentPieces[i];
                pieces.ShowInInspector();
            }
            pieces.number = EditorGUILayout.IntField(pieces.number);
            EditorGUILayout.EndHorizontal();
        }
        GUI.EndScrollView();

    }
}
