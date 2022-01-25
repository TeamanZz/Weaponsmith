using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSavedManager : MonoBehaviour
{
    public DungeonManager dungeonManager;
    public int numberOfSaves = 0;

    public string savedName = "";

    public void Awake()
    {
        dungeonManager = GetComponent<DungeonManager>();
        numberOfSaves = PlayerPrefs.GetInt("allNumberOfSaves");
    }

    [ContextMenu("Saved")]
    public void SavedDungeon()
    {
        if (savedName == "" || savedName == null || dungeonManager.pieces.Count <= 0)
        {
            Debug.Log("Enter saved name");
            return;
        }

        PlayerPrefs.SetString("Dungeon" + numberOfSaves, savedName);

        savedName = "";

        PlayerPrefs.SetInt("numberOfRows" + numberOfSaves, numberOfSaves);

        PlayerPrefs.SetInt("piecesDungeonCount" + numberOfSaves, dungeonManager.pieces.Count);

        Debug.Log(PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + "." + PlayerPrefs.GetString("Dungeon" + numberOfSaves) + "." + PlayerPrefs.GetInt("piecesDungeonCount" + numberOfSaves));

        for (int y = 0; y < dungeonManager.pieces.Count; y++)
        {
            //Debug.Log(dungeonManager.pieces[y].number);
            PlayerPrefs.SetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + y), dungeonManager.pieces[y].number); 
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

        dungeonManager.ClearDungeon();
        dungeonManager.CreatePieacesDungeon(PlayerPrefs.GetInt("piecesDungeonCount" + showSavedNumber));


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
            dungeonManager.ShovPieceProps(y, PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + showSavedNumber) + y).ToString()));
            Debug.Log(y + " | " + PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + showSavedNumber) + y).ToString()));
        }

        //Debug.Log(PlayerPrefs.GetInt(("dungeonProps" + PlayerPrefs.GetInt("numberOfRows" + numberOfSaves) + y).ToString()));

        //dungeonManager.ShovPieceProps();

    }
}
