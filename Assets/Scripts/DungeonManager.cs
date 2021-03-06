using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    public bool isDungeonStarted;
    public int currentLevelEarnedMoneyCount;
    public int currentChestReward;

    public bool blueprintRecieved;

    [SerializeField] private Transform dungeonCharacter;
    [SerializeField] private Transform dungeonCharacterPreviewPos;
    [SerializeField] private Transform dungeonCharacterStartPos;
    [SerializeField] private TrackingCamera dungeonCamera;
    [SerializeField] private Transform dungeonCameraPreviewPos;
    [SerializeField] private Transform dungeonCameraStartPos;

    public Image progressBarFill;
    public float bossZPosition = 0;

    public GameObject dungeonFogUI;
    public GameObject dungeonProgressBar;

    public int currentDungeonLevelId;

    private void Awake()
    {
        Instance = this;
        LoadData();
    }
    
    private void Update()
    {
        UpdateProgressBarValue();
    }

    [ContextMenu("Save Data")]
    public void SaveData()
    {
        PlayerPrefs.SetInt($"DungeonManager{0}Level", currentDungeonLevelId);
        Debug.Log("Save Dungeon Data = " + " Level ID =" + currentDungeonLevelId);
    }

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        currentDungeonLevelId = PlayerPrefs.GetInt($"DungeonManager{0}Level");
    }

    [ContextMenu("Remove Data")]
    public void RemoveData()
    {
        PlayerPrefs.SetInt($"DungeonManager{0}Level", 0);
        currentDungeonLevelId = 0;
    }

    private void UpdateProgressBarValue()
    {
        if (bossZPosition == 0)
            return;
        var newBarValue = (DungeonCharacter.Instance.transform.position.z - -6.5f) / (bossZPosition - -6.5f);
        progressBarFill.fillAmount = newBarValue + 0.05f;
    }

    public void SetCharacterOnStart()
    {
        dungeonCharacter.transform.position = dungeonCharacterStartPos.position;
        dungeonCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);

        dungeonCamera.transform.position = dungeonCameraStartPos.position;
        dungeonCamera.transform.rotation = Quaternion.Euler(32, 0, 0);
        dungeonCamera.targetDistance = 17;

        isDungeonStarted = true;
        dungeonProgressBar.SetActive(true);
        dungeonFogUI.SetActive(true);
    }

    public void ShowCharacterPreview()
    {
        if (isDungeonStarted)
            return;
        dungeonCharacter.transform.position = dungeonCharacterPreviewPos.position;
        dungeonCharacter.transform.rotation = Quaternion.Euler(0, -90, 0);
        dungeonCamera.transform.position = dungeonCameraPreviewPos.position;
        dungeonCamera.transform.rotation = Quaternion.Euler(7, 90, 0);
        dungeonCamera.targetDistance = 0;
        dungeonProgressBar.SetActive(false);
        dungeonFogUI.SetActive(false);
    }
}