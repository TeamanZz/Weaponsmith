using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    public static SkinsManager Instance;

    public int currentSkinIndex;
    [HideInInspector] public int dungeonEnemySkinCount = 0;

    public List<ParticleSystem> particles = new List<ParticleSystem>();
    public List<GameObject> skins = new List<GameObject>();
    public List<GameObject> dungeonSkins = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        currentSkinIndex = PlayerPrefs.GetInt("skinIndex");
        dungeonEnemySkinCount = PlayerPrefs.GetInt("EnemySkinCount");
    }

}