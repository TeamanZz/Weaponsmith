using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopPanelItemsManager : MonoBehaviour
{
    public static WorkshopPanelItemsManager Instance;

    public GameObject appearParticles;
 
    public List<GameObject> workshopObjects = new List<GameObject>();
    public List<WorkshopItem> workshopPanelItems = new List<WorkshopItem>();
 
    [Header("Camera")]
    public Coroutine currentFocusCorutine;
    public float unlockObjectTime = 1f;

    public CinemachineVirtualCamera dungeonCinemaCamera;
    public Transform calibrationPosition;

    private float startCameraField = 0;
    // [SerializeField] private float lastCameraField = 80f;

    public float timeToReturnFocus = 0.5f;

    public CameraWidth cameraWidth;
    public Coroutine invokeCoroutine;

    [ContextMenu("Awake")]
    public void Awake()
    {
        Instance = this;
    }

    public void PreUnlock(int index, float focus)
    {
        CameraFocus(workshopObjects[index].transform, focus);
        invokeCoroutine = StartCoroutine(Unlock(index));
    }
    public IEnumerator Unlock(int index)
    {
        yield return new WaitForSeconds(unlockObjectTime);
        UnlockObject(index);
    }

    public void UnlockObject(int index)
    {
        Debug.Log("Unlock");
        workshopObjects[index].SetActive(!workshopObjects[index].activeSelf);
        Instantiate(appearParticles, workshopObjects[index].transform.position, new Quaternion(0, 0, 0, 0));
        workshopPanelItems[index].ReplaceOldObjects();
    }

    public void UnlockObjectWithoutParticles(int index)
    {
        workshopObjects[index].SetActive(!workshopObjects[index].activeSelf);
        workshopPanelItems[index].ReplaceOldObjects();
    }

    public void CameraFocus(Transform target, float focus)
    {
        Debug.Log("Camera Focus");
        if (currentFocusCorutine != null)
            StopCoroutine(currentFocusCorutine);

        currentFocusCorutine = StartCoroutine(EnterFocus(target, focus));
    }

    public IEnumerator EnterFocus(Transform target, float focusField)
    {
        Debug.Log("Enter Focus");
        cameraWidth.enabled = false;
        if (startCameraField == 0)
            startCameraField = dungeonCinemaCamera.m_Lens.FieldOfView;
        dungeonCinemaCamera.LookAt = target;
        dungeonCinemaCamera.m_Lens.FieldOfView = startCameraField;

        DOTween.To(() => dungeonCinemaCamera.m_Lens.FieldOfView, x => dungeonCinemaCamera.m_Lens.FieldOfView = x, focusField, timeToReturnFocus);
        yield return new WaitForSeconds(timeToReturnFocus);

        currentFocusCorutine = StartCoroutine(ReturnFocus());
    }
    public IEnumerator ReturnFocus()
    {
        Debug.Log("Return Focus");
        DOTween.To(() => dungeonCinemaCamera.m_Lens.FieldOfView, x => dungeonCinemaCamera.m_Lens.FieldOfView = x, startCameraField, timeToReturnFocus);
        yield return new WaitForSeconds(timeToReturnFocus);
        cameraWidth.enabled = true;

        dungeonCinemaCamera.LookAt = calibrationPosition;
        // dungeonCinemaCamera.m_Lens.FieldOfView = lastCameraField;
        currentFocusCorutine = null;
        //dungeonCinemaCamera.m_Lens.FieldOfView = startCameraField;
    }

}