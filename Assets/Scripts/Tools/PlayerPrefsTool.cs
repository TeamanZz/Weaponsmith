using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPrefsTool : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("PlayerPrefs/Clear Player Prefs")]
    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    // [MenuItem("PlayerPrefs/Add10000PP")]

    // static void Add10000PP()
    // {
    //     var oldValue = PlayerPrefs.GetInt("PopulationPoints");

    //     oldValue += 10000;
    //     PlayerPrefs.SetInt("PopulationPoints", oldValue);
    // }
#endif
}
