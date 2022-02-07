using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DungeonManager : MonoBehaviour
{
    [Header("Basic settings")]
    public static DungeonManager dungeonManager;
    //public NavMeshSurface navMesh;
    public PiecesDungeon piecesObject;
    public DungeonObjectData currentDungeon;

    public DungeonObjectData currentObjectData;

    public Transform firstPoint;
    public Transform lastPoint;

    public bool startGame = false;

    [Header("Instatiate settings")]
    [HideInInspector] public int number;
    public int deletedNumber = -1;
    public List<PiecesDungeon> pieces = new List<PiecesDungeon>();
    public List<DungeonItem> dungeonsItems = new List<DungeonItem>();


    [Header("Show settings")]
    public int numberOfSaves = 0;

    public string savedName = "";

    public void Awake()
    {
        numberOfSaves = PlayerPrefs.GetInt("allNumberOfSaves");

        dungeonManager = this;
        startGame = true;
    }

    public void ReadSaveData()
    {
        dungeonManager = this;
        dungeonsItems.Clear();
        int saveCount = PlayerPrefs.GetInt("allNumberOfSaves");
        Debug.Log(saveCount);

        if (currentObjectData != null)
            currentObjectData.dungeonManager = this;

        for (int i = 0; i < saveCount; i++)
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

        numberOfSaves = dungeonsItems.Count;
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
            if(startGame == false)
            DestroyImmediate(piec.gameObject);
            else
                Destroy(piec.gameObject);
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

        //Debug.Log("Show number - " + piecesNumber + " | " + "Props number - " + propsNumber);
        pieces[piecesNumber].ShowProps(propsNumber);
        NavMeshRebaking();
    }

    public void NavMeshRebaking()
    {
        //navMesh.BuildNavMesh();
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
#if UNITY_EDITOR
        currentObjectData.inRealTimeEditor = false;
        currentDungeon.UpdateCurrentData();
#endif
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
            //Debug.Log(y + " | " + PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + showSavedNumber) + y).ToString()));
        }

    }

    public void SaveAllDungeon()
    {
        List<int> badNumber = new List<int>();
        for(int i = 0; i < dungeonsItems.Count; i ++)
        {
            DungeonItem item = dungeonsItems[i];

            if (item.dungeonName == "" || item.dungeonName == null || item.rowState.Count <= 0 || item.rowState.Count > 500)
            {
                Debug.Log("Enter saved name");
                badNumber.Add(i);
                continue;
                
            }


        }
        //  clearing    
        for(int i = 0; i < badNumber.Count;i++  )
        { 
            int num = badNumber[i];
            Debug.Log(badNumber[i]);
            dungeonsItems.Remove(dungeonsItems[num]);
        }

        numberOfSaves = 0;
        for (int i = 0; i < dungeonsItems.Count; i ++)
        {
            PlayerPrefs.SetString("Dungeon" + i, dungeonsItems[i].dungeonName);

            PlayerPrefs.SetInt("numberOfRows" + i, i);

            PlayerPrefs.SetInt("piecesDungeonCount" + i, dungeonsItems[i].rowState.Count);

            Debug.Log(PlayerPrefs.GetInt("numberOfRows" + i) + "." + PlayerPrefs.GetString("Dungeon" + i) + "." + PlayerPrefs.GetInt("piecesDungeonCount" + i));

            for (int y = 0; y < dungeonsItems[i].rowState.Count; y++)
            {
                //Debug.Log(dungeonManager.pieces[y].number);
                PlayerPrefs.SetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + i) + y), dungeonsItems[i].rowState[y]);
                //Debug.Log(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + y).ToString() + " - " + PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + y).ToString()));
            }

            string code = "";
            for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + i); y++)
            {
                code += PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + i) + y).ToString());
            }
            Debug.Log("Saved number - " + PlayerPrefs.GetInt("numberOfRows" + i) + " | Name - " + PlayerPrefs.GetString("Dungeon" + i) + " | Pieces count - " + PlayerPrefs.GetInt("piecesDungeonCount" + i) + " | Code - " + code);

            numberOfSaves += 1;
            PlayerPrefs.SetInt("allNumberOfSaves", numberOfSaves);
        }
    }
    public void DeleteDungeonByNumber(int deletedNumber)
    {
        if (deletedNumber < 0 || deletedNumber >= dungeonsItems.Count)
        {
            Debug.Log("Enter correct deleted number");
            return;
        }
        Debug.Log("Delete start");
        Debug.Log(deletedNumber);

        //for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + deletedNumber); y++)
        //{
        //    PlayerPrefs.DeleteKey(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + deletedNumber) + y));
        //    Debug.Log(PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + deletedNumber) + y).ToString()));
        //}

        //string code = "N";
        //for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + deletedNumber); y++)
        //{
        //    PlayerPrefs.DeleteKey(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + deletedNumber) + y));
        //    code += PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + deletedNumber) + y).ToString());
        //}

        //PlayerPrefs.DeleteKey("numberOfRows" + deletedNumber);
        //PlayerPrefs.DeleteKey("Dungeon" + deletedNumber);
        //PlayerPrefs.DeleteKey("piecesDungeonCount" + deletedNumber);

        dungeonsItems[deletedNumber].dungeonName = "";
        dungeonsItems[deletedNumber].piecesDungeonCount = 0;
        dungeonsItems[deletedNumber].rowState.Clear();

        //Debug.Log("Saved number - " + PlayerPrefs.GetInt("numberOfRows" + deletedNumber) + " | Name - " + PlayerPrefs.GetString("Dungeon" + deletedNumber) + " | Pieces count - " + PlayerPrefs.GetInt("piecesDungeonCount" + deletedNumber) + " | Code - ");

        //dungeonsItems.Remove(dungeonsItems[deletedNumber]);
#if UNITY_EDITOR
        currentObjectData.inRealTimeEditor = true;
        currentDungeon.UpdateCurrentData();
#endif
        ListCleaning();
    }

    //  удаление с возможностью сохранения

    public void ListCleaning()
    {
        int difference = 0;

        Debug.Log("Cleaning start");
        for (int i = 0; i < dungeonsItems.Count; i++)
        {
            bool norm = false;
            Debug.Log(i);
            Debug.Log(dungeonsItems.Count);

            if (dungeonsItems[i].dungeonName == "" || dungeonsItems[i].rowState.Count == 0 || dungeonsItems[i].piecesDungeonCount == 0 || dungeonsItems[i] == null)
            {
                norm = true;
                difference += 1;
                Debug.Log(i + " = null");
                //break;
                dungeonsItems.Remove(dungeonsItems[i]);
            }

            if (difference > 0 && norm == false)
            {
                dungeonsItems[i - difference] = dungeonsItems[i];
                dungeonsItems[i] = null;
            }
        }
    }

    public void ResaveData()
    {
        int saveCount = PlayerPrefs.GetInt("allNumberOfSaves");
        Debug.Log(saveCount);

        int difference = 0;

        if (currentObjectData != null)
            currentObjectData.dungeonManager = this;

        for (int i = 0; i < dungeonsItems.Count; i++)
        {
            DungeonItem item = dungeonsItems[i];

            Debug.Log("Item = " + item);

            PlayerPrefs.SetInt("numberOfRows" + i, item.dungeonNumber);
            PlayerPrefs.SetString("Dungeon" + i, item.dungeonName);
            PlayerPrefs.SetInt("piecesDungeonCount" + i, item.piecesDungeonCount);

            string code = "";

            for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + i); y++)
            {
                PlayerPrefs.SetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + i) + y), item.rowState[y]);
                code += PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + i) + y).ToString());
            }

            Debug.Log("Saved number - " + PlayerPrefs.GetInt("numberOfRows" + i) + " | Name - " + PlayerPrefs.GetString("Dungeon" + i) + " | Pieces count - " + PlayerPrefs.GetInt("piecesDungeonCount" + i) + " | Code - " + code);

            }

            difference = 0;
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

        if (GUILayout.Button("Show Save Data"))
        {
            dungeonManager.OutputSave();
        }

        //  create/clear
        GUILayout.Space(7.5f);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Shov Dungeon To Number"))
        {
            dungeonManager.ShowDungeon();
            dungeonManager.currentObjectData.inRealTimeEditor = false;
            dungeonManager.currentDungeon.UpdateCurrentData();
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
            dungeonManager.currentObjectData.inRealTimeEditor = true;
            dungeonManager.currentDungeon.UpdateCurrentData();
        }


        //  create/clear
        GUILayout.Space(7.5f);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Create Dungeon"))
        {
            dungeonManager.CreateDungeon();
            dungeonManager.currentObjectData.inRealTimeEditor = true;
            dungeonManager.currentDungeon.UpdateCurrentData();
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
            dungeonManager.currentObjectData.inRealTimeEditor = true;
            dungeonManager.currentDungeon.UpdateCurrentData();
        }

        GUILayout.Space(7.5f);
        EditorGUILayout.BeginHorizontal();

        dungeonManager.savedName = EditorGUILayout.TextField("Enter the title", dungeonManager.savedName);
        GUILayout.Label("Name of the dungeon - " + dungeonManager.savedName);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save Current Dungeon"))
        {
            dungeonManager.SaveDungeon();
            //dungeonManager.currentObjectData.inRealTimeEditor = false;
            //dungeonManager.currentDungeon.UpdateCurrentData();

        }

        EditorGUILayout.Space(7.5f);
        EditorGUILayout.BeginHorizontal();
        int deletedNumber;
        string deletedLabelText = "";
        if (GUILayout.Button("Delete Dungeon By Number"))
        {
            dungeonManager.DeleteDungeonByNumber(dungeonManager.deletedNumber);
            dungeonManager.deletedNumber = -1;
            //dungeonManager.currentObjectData.inRealTimeEditor = true;
            //dungeonManager.currentDungeon.UpdateCurrentData();
        }

        dungeonManager.deletedNumber = EditorGUILayout.IntField(dungeonManager.deletedNumber);

        string deleteText = "Maximum value = " + dungeonManager.dungeonsItems.Count;
        GUILayout.Label(deleteText);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(7.5f);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply removal"))
        {
            dungeonManager.SaveAllDungeon();
            //dungeonManager.currentObjectData.inRealTimeEditor = true;
            //dungeonManager.currentDungeon.UpdateCurrentData();
        }

        if (GUILayout.Button("Cancel changes"))
        {
            dungeonManager.ReadSaveData();
            //dungeonManager.currentObjectData.inRealTimeEditor = true;
            //dungeonManager.currentDungeon.UpdateCurrentData();
        }
        EditorGUILayout.EndHorizontal();

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

    public void OpenWindow()
    {

    }
}
#endif