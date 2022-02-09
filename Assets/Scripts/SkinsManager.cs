using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    public static SkinsManager Instance;

    public List<ParticleSystem> particles = new List<ParticleSystem>();

    public List<GameObject> skins = new List<GameObject>();
    public List<GameObject> dungeonSkins = new List<GameObject>();

    public TrackingCamera trackingCamera;

    public int currentSkinIndex;

    public int dungeonEnemySkinCount = 0;
    private void Awake()
    {
        Instance = this;
        currentSkinIndex = 0;

        currentSkinIndex = PlayerPrefs.GetInt("skinIndex");
        dungeonEnemySkinCount = PlayerPrefs.GetInt("EnemySkinCount");

        ChangeSkin();
    }

    private void Start()
    {
        var tempSkinIndex = PlayerPrefs.GetInt("skinIndex");
        if (tempSkinIndex == 1)
            ChangeSkin();
    }

    [ContextMenu("Change Skin")]
    public void ChangeSkin()
    {
        Debug.Log("New skin count - " + currentSkinIndex);
       
        int currentNumber = Mathf.Clamp(currentSkinIndex, 0, dungeonSkins.Count - 1);

        Debug.Log("Current number - " + currentSkinIndex + " | " + " Entered value - " + currentSkinIndex);
        Debug.Log("Contue");

        foreach(GameObject dungeSkin in dungeonSkins)
        {
            dungeSkin.SetActive(false);
        }

        foreach (GameObject skin in skins)
        {
            skin.SetActive(false);
        }

        foreach(var part in particles)
        {
            part.Play();
        }

        skins[currentNumber].SetActive(true);
        dungeonSkins[currentNumber].SetActive(true);

        PlayerPrefs.SetInt("skinIndex", currentSkinIndex);
        PlayerPrefs.SetInt("EnemySkinCount", dungeonEnemySkinCount);
    }
}