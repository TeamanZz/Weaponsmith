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
    public EraController eraController;

    [HideInInspector] public List<GameObject> currentEraWorkshopObjects = new List<GameObject>();
    [HideInInspector] public List<WorkshopItem> workshopPanelItems = new List<WorkshopItem>();
    [HideInInspector] public GameObject transitionPanel;
    [HideInInspector] public Button transitionButton;
    [HideInInspector] public int numberOfOpenPanels = 0;
    [HideInInspector] public bool endOfEra = false;

    [Header("Camera")]
    public Coroutine currentFocusCorutine;
    public float unlockObjectTime = 1f;

    public CinemachineVirtualCamera dungeonCinemaCamera;
    public Transform calibrationPosition;

    public float startCameraField = 80f;
    public float timeToReturnFocus = 0.5f;

    public CameraWidth cameraWidth;

    [ContextMenu("Awake")]
    public void Awake()
    {
        Instance = this;
        startCameraField = dungeonCinemaCamera.m_Lens.FieldOfView;
        if (transitionPanel == null)
        {
            Debug.Log("Transition panel in storage is null");
            return;
        }
        transitionPanel.SetActive(false);
        if (endOfEra == true)
            return;

        transitionButton = transitionPanel.GetComponent<Button>();
        transitionButton.onClick.AddListener(() => eraController.TransitionToNewEra());
    }

    public void Initialization(List<GameObject> objects, List<WorkshopItem> newWorkshopPanelItems, GameObject newTransitionButton, bool isEnd)
    {
        currentEraWorkshopObjects = objects;
        endOfEra = isEnd;

        transitionPanel = newTransitionButton;

        workshopPanelItems.Clear();
        workshopPanelItems.AddRange(newWorkshopPanelItems);
    }

    public void CheckBeforeTransition()
    {
        numberOfOpenPanels += 1;
        if (numberOfOpenPanels < workshopPanelItems.Count || transitionPanel == null)
            return;

        if (endOfEra == true)
        {
            Debug.Log("End era");
            return;
        }

        Debug.Log("Open transition");
        transitionPanel.SetActive(true);
    }
    public Coroutine invokeCoroutine;
    public void PreUnlock(int index, float focus)
    {
        CameraFocus(currentEraWorkshopObjects[index].transform, focus);
        //if (invokeCoroutine != null)
        //    StopCoroutine(invokeCoroutine);

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
        currentEraWorkshopObjects[index].SetActive(!currentEraWorkshopObjects[index].activeSelf);
        Instantiate(appearParticles, currentEraWorkshopObjects[index].transform.position, new Quaternion(0, 0, 0, 0));
        workshopPanelItems[index].ReplaceOldObjects();
    }

    public void UnlockObjectWithoutParticles(int index)
    {
        currentEraWorkshopObjects[index].SetActive(!currentEraWorkshopObjects[index].activeSelf);
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
        currentFocusCorutine = null;
        //dungeonCinemaCamera.m_Lens.FieldOfView = startCameraField;
    }

}