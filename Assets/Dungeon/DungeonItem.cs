using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class DungeonItem
{
    public int dungeonNumber;
    public string dungeonName;
    public int piecesDungeonCount;
    public List<int> rowState = new List<int>();
}
