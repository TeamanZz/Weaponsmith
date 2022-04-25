using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonBoss : MonoBehaviour
{
    public TextMeshProUGUI bossName;

    public void Initialize(string name)
    {
        bossName.text = name;
    }
}