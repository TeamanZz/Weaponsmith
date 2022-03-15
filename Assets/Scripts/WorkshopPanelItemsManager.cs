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

    [ContextMenu("Awake")]
    public void Awake()
    {
        Instance = this;

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

    public void UnlockObject(int index)
    {
        currentEraWorkshopObjects[index].SetActive(!currentEraWorkshopObjects[index].activeSelf);
        Instantiate(appearParticles, currentEraWorkshopObjects[index].transform.position, new Quaternion(0, 0, 0, 0));
        workshopPanelItems[index].ReplaceOldObjects();
    }

    public void UnlockObjectWithoutParticles(int index)
    {
        currentEraWorkshopObjects[index].SetActive(!currentEraWorkshopObjects[index].activeSelf);
        workshopPanelItems[index].ReplaceOldObjects();
    }
}