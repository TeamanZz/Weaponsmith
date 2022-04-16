using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    public bool isDungeonStarted;

    [SerializeField] private Transform dungeonCharacter;
    [SerializeField] private Transform dungeonCharacterPreviewPos;
    [SerializeField] private Transform dungeonCharacterStartPos;
    [SerializeField] private TrackingCamera dungeonCamera;
    [SerializeField] private Transform dungeonCameraPreviewPos;
    [SerializeField] private Transform dungeonCameraStartPos;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCharacterOnStart()
    {
        dungeonCharacter.transform.position = dungeonCharacterStartPos.position;
        dungeonCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);

        dungeonCamera.transform.position = dungeonCameraStartPos.position;
        dungeonCamera.transform.rotation = Quaternion.Euler(32, 0, 0);
        dungeonCamera.targetDistance = 17;

        isDungeonStarted = true;
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
    }

    public void ResetDungeon()
    {
        ShowCharacterPreview();
        DungeonCharacter.Instance.animator.SetBool("IsDungeonStarted", false);
        DungeonBuilder.Instance.ResetDungeon();
    }
}
