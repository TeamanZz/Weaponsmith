using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{

    public List<GameObject> skins = new List<GameObject>();
    public List<GameObject> dungeonSkins = new List<GameObject>();
    public static SkinsManager Instance;
    public TrackingCamera trackingCamera;

    public int currentSkinIndex;
    public int skinCount = 0;
    private void Awake()
    {
        Instance = this;
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
        if (currentSkinIndex >= skins.Count - 1)
            return;

        skins[currentSkinIndex].SetActive(false);
        dungeonSkins[currentSkinIndex].SetActive(false);
        trackingCamera.target = dungeonSkins[currentSkinIndex + 1].transform;
        dungeonSkins[currentSkinIndex + 1].transform.position = dungeonSkins[currentSkinIndex].transform.position;

        currentSkinIndex++;
        skins[currentSkinIndex].SetActive(true);
        dungeonSkins[currentSkinIndex].SetActive(true);

        PlayerPrefs.SetInt("skinIndex", currentSkinIndex);
        PlayerPrefs.SetInt("EnemySkinCount", skinCount);
    }
}