using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PiecesDungeon : MonoBehaviour
{
    public Transform lastPoint;

    public List<GameObject> propsInPieces = new List<GameObject>();

    public int number;

    [ContextMenu("Show props")]
    public void ShowInInspector()
    {
        if (number != null && number >= 0 && number < propsInPieces.Count)
            ShowProps(number);
        else
            Debug.Log("Enter the number of pieces");
    }
    public void ShowProps(int propsNumber)
    {
        if (propsNumber < 0 || propsNumber > propsInPieces.Count) 
            return;
        
        for(int i = 0; i < propsInPieces.Count; i++)
        {
            propsInPieces[i].SetActive(false);
        }

        propsInPieces[propsNumber].SetActive(true);
        number = propsNumber;
    }

    [ContextMenu("Clear props")]
    public void ClearProps()
    {
        for (int i = 0; i < propsInPieces.Count; i++)
        {
            propsInPieces[i].SetActive(false);
        }

        number = 0;
        propsInPieces[0].SetActive(true);
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(PiecesDungeon))]
class PiecesEditor : Editor
{
    public override void OnInspectorGUI()//Rect position, GUIContent label)
    {
        var pieces = (PiecesDungeon)target;

        if (pieces == null)
            return;

        DrawDefaultInspector();

        if (GUILayout.Button("Create Environment By Number"))
        {
            pieces.ShowInInspector();
        }

    }
}
#endif
