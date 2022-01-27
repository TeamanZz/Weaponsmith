using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class DungeonManager : MonoBehaviour
{
    [Header("Basic settings")]
    public NavMeshSurface navMesh;
    public PiecesDungeon piecesObject;
    public DungeonObjectData currentDungeon;

    public Transform firstPoint;
    public Transform lastPoint;

    [Header("Instatiate settings")]
    [HideInInspector]public int number;
    public List<PiecesDungeon> pieces = new List<PiecesDungeon>();
    public List<DungeonItem> dungeonsItems = new List<DungeonItem>();

    [Header("Show settings")]
    public int numberOfSaves = 0;

    public string savedName = "";

    public void Awake()
    {
        numberOfSaves = PlayerPrefs.GetInt("allNumberOfSaves");
    }

    public void ReadSaveData()
    {
        int saveCount = PlayerPrefs.GetInt("allNumberOfSaves");
        Debug.Log(saveCount);
        dungeonsItems.Clear();

        for (int i = 0; i < numberOfSaves; i++)
        {
            DungeonItem item = new DungeonItem();

            item.dungeonNumber = PlayerPrefs.GetInt("numberOfRows" + i);
            item.dungeonName = PlayerPrefs.GetString("Dungeon" + i);
            item.piecesDungeonCount = PlayerPrefs.GetInt("piecesDungeonCount" + i);

            string code = "";

            for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + i); y++)
            {
                int propceNumber = PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + i) + y));
                item.rowState.Add(propceNumber);
                code += PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + i) + y).ToString());
            }

            dungeonsItems.Add(item);
            Debug.Log("Saved number - " + PlayerPrefs.GetInt("numberOfRows" + i) + " | Name - " + PlayerPrefs.GetString("Dungeon" + i) + " | Pieces count - " + PlayerPrefs.GetInt("piecesDungeonCount" + i) + " | Code - " + code);
        }
    }

    [ContextMenu("Create Dungeon")]
    public void CreateDungeon()
    {
        if (number != null && number > 1 && number < 200)
            CreatePieacesDungeon(number);
        else
            Debug.Log("Enter the number of pieces");
    }
    public void CreatePieacesDungeon(int numberOfPieces)
    {
        for (int i = 0; i < numberOfPieces; i++)
        {
            PiecesDungeon newPieces = Instantiate(piecesObject, lastPoint);
            lastPoint = newPieces.lastPoint;
            pieces.Add(newPieces);
            newPieces.transform.parent = firstPoint.transform;
        }

        number = 0;
        NavMeshRebaking();
    }

    [ContextMenu("Clear Dungeon")]
    public void ClearDungeon()
    {
        foreach (PiecesDungeon piec in pieces)
        {
            DestroyImmediate(piec.gameObject);
        }

        pieces.Clear();
        lastPoint = firstPoint;
        number = 0;

        NavMeshRebaking();
    }

    public void ShovPieceProps(int piecesNumber, int propsNumber)
    {
        if (pieces.Count < 0 || piecesNumber > pieces.Count || propsNumber > pieces[piecesNumber].propsInPieces.Count || propsNumber < 0)
        {
            Debug.Log("Pieces count < 0");
            return;
        }

        Debug.Log("Show number - " + piecesNumber + " | " + "Props number - " + propsNumber);
        pieces[piecesNumber].ShowProps(propsNumber);
        NavMeshRebaking();
    }

    public void NavMeshRebaking()
    {
        navMesh.BuildNavMesh();
    }

    [ContextMenu("Saved")]
    public void SaveDungeon()
    {
        if (savedName == "" || savedName == null || pieces.Count <= 0 || pieces.Count > 500)
        {
            Debug.Log("Enter saved name");
            return;
        }

        PlayerPrefs.SetString("Dungeon" + numberOfSaves, savedName);

        savedName = "";

        PlayerPrefs.SetInt("numberOfRows" + numberOfSaves, numberOfSaves);

        PlayerPrefs.SetInt("piecesDungeonCount" + numberOfSaves, pieces.Count);

        Debug.Log(PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + "." + PlayerPrefs.GetString("Dungeon" + numberOfSaves) + "." + PlayerPrefs.GetInt("piecesDungeonCount" + numberOfSaves));

        for (int y = 0; y < pieces.Count; y++)
        {
            //Debug.Log(dungeonManager.pieces[y].number);
            PlayerPrefs.SetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + y), pieces[y].number);
            //Debug.Log(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + y).ToString() + " - " + PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + y).ToString()));
        }

        string code = "";
        for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + numberOfSaves); y++)
        {
            code += PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + y).ToString());
        }
        Debug.Log("Saved number - " + PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + " | Name - " + PlayerPrefs.GetString("Dungeon" + numberOfSaves) + " | Pieces count - " + PlayerPrefs.GetInt("piecesDungeonCount" + numberOfSaves) + " | Code - " + code);

        numberOfSaves += 1;
        PlayerPrefs.SetInt("allNumberOfSaves", numberOfSaves);
    }

    [ContextMenu("Show all saved")]
    public void OutputSave()
    {
        int number = PlayerPrefs.GetInt("allNumberOfSaves");
        //Debug.Log(number);

        for (int i = 0; i < number; i++)
        {
            //Debug.Log("Saved number" + PlayerPrefs.GetInt("numberOfRows" + i) + " | Name - " + PlayerPrefs.GetString("Dungeon" + i) + " | Pieces count - " + PlayerPrefs.GetInt("piecesDungeonCount" + i));

            string code = "";
            for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + i); y++)
            {
                code += PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + i) + y));
            }

            Debug.Log(PlayerPrefs.GetInt("numberOfRows" + i) + "." + PlayerPrefs.GetString("Dungeon" + i) + "." + PlayerPrefs.GetInt("piecesDungeonCount" + i) + "." + code);
            //for (int y = 0; y < dungeonManager.pieces.Count; y++)
            //{
            //    Debug.Log(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + y).ToString() + " - " + PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + y).ToString()));
            //}
        }
    }

    [ContextMenu("Clear all saved")]
    public void ClearAllSaved()
    {
        int number = PlayerPrefs.GetInt("allNumberOfSaves");

        for (int i = 0; i < number; i++)
        {
            PlayerPrefs.DeleteKey("numberOfRows" + i);
            PlayerPrefs.DeleteKey("Dungeon" + i);

            for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + i); y++)
            {
                PlayerPrefs.DeleteKey(PlayerPrefs.GetInt("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + i) + y).ToString());
            }

            PlayerPrefs.DeleteKey("piecesDungeonCount" + i);
        }
        PlayerPrefs.DeleteKey("allNumberOfSaves");

        number = PlayerPrefs.GetInt("allNumberOfSaves");
        Debug.Log("Cleared - " + number);

        numberOfSaves = 0;
    }

    [Header("Show dungeon settings")]
    public int showSavedNumber;
    [ContextMenu("Show dungeon")]
    public void ShowDungeon()
    {
        int number = PlayerPrefs.GetInt("allNumberOfSaves");
        if (showSavedNumber > number || showSavedNumber < 0)
        {
            Debug.Log("Enter saved number");
            return;
        }

        Debug.Log(PlayerPrefs.GetInt("piecesDungeonCount" + showSavedNumber));

        ClearDungeon();
        CreatePieacesDungeon(PlayerPrefs.GetInt("piecesDungeonCount" + showSavedNumber));


        string code = "N";
        for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + showSavedNumber); y++)
        {
            code += PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + showSavedNumber) + y).ToString());
            //Debug.Log("Code - " + code);
        }
        Debug.Log("Saved number - " + PlayerPrefs.GetInt("numberOfRows" + showSavedNumber) + " | Name - " + PlayerPrefs.GetString("Dungeon" + showSavedNumber) + " | Pieces count - " + PlayerPrefs.GetInt("piecesDungeonCount" + showSavedNumber) + " | Code - " + code);


        //Debug.Log(PlayerPrefs.GetInt("piecesDungeonCount" + showSavedNumber));

        for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + showSavedNumber); y++)
        {
            ShovPieceProps(y, PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + showSavedNumber) + y).ToString()));
            Debug.Log(y + " | " + PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + showSavedNumber) + y).ToString()));
        }

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DungeonManager))]
class DungeonManagerEditor : Editor  
{
    public override void OnInspectorGUI()//Rect position, GUIContent label)
    {
        var dungeonManager = (DungeonManager)target;

        if (dungeonManager == null)
            return;

        DrawDefaultInspector();

        //  saved
        GUILayout.Space(10);
        if (GUILayout.Button("Read Save Data"))
        {
            dungeonManager.ReadSaveData();
        }

        if (GUILayout.Button("Shov Save Data"))
        {
            dungeonManager.OutputSave ();
        }

       
        //  create/clear
        GUILayout.Space(7.5f);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Shov Dungeon To Number"))
        {
            dungeonManager.ShowDungeon();
        }

        int shovLenght = EditorGUILayout.IntField(dungeonManager.showSavedNumber);
        string shovText = "Maximum value = " + dungeonManager.dungeonsItems.Count;
        if (shovLenght > -1 && shovLenght < dungeonManager.dungeonsItems.Count)
        {
            dungeonManager.showSavedNumber = shovLenght;
            //labelText = "Final Lenght = " + (dungeonManager.pieces.Count + dungeonManager.number).ToString();
        }
        else
            dungeonManager.showSavedNumber = 0;

        GUILayout.Label(shovText);

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clear Current Dungeon"))
        {
            dungeonManager.ClearDungeon();
        }


        //  create/clear
        GUILayout.Space(7.5f);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Create Dungeon"))
        {
            dungeonManager.CreateDungeon();
        }

        int newLenght = EditorGUILayout.IntField(dungeonManager.number);
        string labelText = "";
        if (newLenght > -1 && newLenght < 250 && dungeonManager.pieces.Count + newLenght < 500)
        {
            dungeonManager.number = newLenght;
            labelText = "Final Lenght = " + (dungeonManager.pieces.Count + dungeonManager.number).ToString();
        }
        else
            dungeonManager.number = 0;
            
        GUILayout.Label(labelText);

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clear Current Dungeon"))
        {
            dungeonManager.ClearDungeon();
        }

        GUILayout.Space(7.5f);
        EditorGUILayout.BeginHorizontal();

        dungeonManager.savedName = EditorGUILayout.TextField("Enter the title", dungeonManager.savedName);
        GUILayout.Label("Name of the dungeon - " + dungeonManager.savedName);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save Current Dungeon"))
        {
            dungeonManager.SaveDungeon();
        }


        GUILayout.Space(15);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(75);

        if (GUILayout.Button("Clear All Data"))
        {
            dungeonManager.ClearAllSaved();
        }

        GUILayout.Space(75);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(15);

        CreateEditor(target);
    }

}
#endif