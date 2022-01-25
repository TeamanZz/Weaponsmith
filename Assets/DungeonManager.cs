using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DungeonManager : MonoBehaviour
{
    [Header("Basic settings")]
    public PiecesDungeon piecesObject;

    public Transform firstPoint;
    public Transform lastPoint;

    [Header("Instatiate settings")]
    public int number;
    public List<PiecesDungeon> pieces = new List<PiecesDungeon>();

    [ContextMenu("Create Dungeon")]
    public void CreateDungeon()
    {
        if (number != null || number > 1)
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
    }

    [ContextMenu("Clear Dungeon")]
    public void ClearDungeon()
    {
        //for (int i = 0; i < firstPoint.childCount; i++)
        //{
        //    GameObject firstObject = firstPoint.GetChild(i).gameObject;
        //    DestroyImmediate(firstObject);
        //}
        foreach (PiecesDungeon piec in pieces)
        {
            DestroyImmediate(piec.gameObject);
        }

        pieces.Clear();
        lastPoint = firstPoint;
        number = 0;
    }

    public void ShovPieceProps(int piecesNumber, int propsNumber)
    {
        if (pieces.Count < 0 || piecesNumber > pieces.Count || propsNumber > pieces[piecesNumber].propsInPieces.Count || propsNumber <  0)
        {
            Debug.Log("Pieces count < 0");
            return;
        }

        Debug.Log("Show number - " + piecesNumber + " | " + "Props number - " + propsNumber);
        pieces[piecesNumber].ShowProps(propsNumber);
    }
}
