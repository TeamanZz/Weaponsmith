using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonObjectData: MonoBehaviour
{
    public DungeonManager dungeonManager;

    public bool inRealTimeEditor = true;
    public bool notSave = false;

    public int currentNumber;
    public string currentName;
    public int currentPiecessCount;
    public List<PiecesDungeon> currentPieces = new List<PiecesDungeon>();

    public void UpdateCurrentData()
    {
        Debug.Log(inRealTimeEditor);

        if (inRealTimeEditor == true)
        {
            currentNumber = dungeonManager.numberOfSaves;
            currentName = "";
            currentPiecessCount = dungeonManager.pieces.Count;
            currentPieces = dungeonManager.pieces;
            //Debug.Log(dungeonManager.pieces);
        }
        else
        {
            int number = PlayerPrefs.GetInt("allNumberOfSaves");
            int setNumber = dungeonManager.showSavedNumber;

            if (setNumber > number || setNumber < 0)
            {
                Debug.Log("Incorrect number");
                return;
            }

            Debug.Log(PlayerPrefs.GetInt("piecesDungeonCount" + setNumber));

            currentNumber = setNumber;
            currentName = PlayerPrefs.GetString("Dungeon" + setNumber);
            PlayerPrefs.GetInt("piecesDungeonCount" + setNumber);


            string code = "N";
            for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + setNumber); y++)
            {
                code += PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + setNumber) + y).ToString());
                //Debug.Log("Code - " + code);
            }
            Debug.Log("Saved number - " + PlayerPrefs.GetInt("numberOfRows" + setNumber) + " | Name - " + PlayerPrefs.GetString("Dungeon" + setNumber) + " | Pieces count - " + PlayerPrefs.GetInt("piecesDungeonCount" + setNumber) + " | Code - " + code);

            currentPieces = dungeonManager.pieces;
        }
    }

    public void SaveUpdatedDungeon()
    {
        PlayerPrefs.SetString("Dungeon" + currentNumber, currentName);

        PlayerPrefs.SetInt("numberOfRows" + currentNumber, currentNumber);

        PlayerPrefs.SetInt("piecesDungeonCount" + currentNumber, currentPieces.Count);

        Debug.Log(PlayerPrefs.GetInt("numberOfRows" + currentNumber) + "." + PlayerPrefs.GetString("Dungeon" + currentNumber) + "." + PlayerPrefs.GetInt("piecesDungeonCount" + currentNumber));

        string code = "";
        for (int y = 0; y < currentPieces.Count; y++)
        {
            PlayerPrefs.SetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + currentPieces) + y), currentPieces[y].number);
            code += currentPieces[y].number.ToString();
        }

        //for (int y = 0; y < PlayerPrefs.GetInt("piecesDungeonCount" + currentNumber); y++)
        //{
        //    code += PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + currentNumber) + y).ToString());
            
        //}
        Debug.Log("Saved number - " + PlayerPrefs.GetInt("numberOfRows" + currentNumber) + " | Name - " + PlayerPrefs.GetString("Dungeon" + currentNumber) + " | Pieces count - " + PlayerPrefs.GetInt("piecesDungeonCount" + currentNumber) + " | Code - " + code);

        //dungeonManager.ResaveData();
    }

    public void RemoveChangesRedoSave()
    {
        dungeonManager.showSavedNumber = currentNumber;
        dungeonManager.ShowDungeon();
    }
    public void SaveDungeon()
    {
        inRealTimeEditor = false;
        dungeonManager.savedName = currentName;
        dungeonManager.SaveDungeon();
    }
}
