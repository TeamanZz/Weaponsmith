using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomObjectsHandler : MonoBehaviour
{
    public static RoomObjectsHandler Instance;

    public List<GameObject> roomObjects = new List<GameObject>();
    public List<WorkshopItem> workshopPanelItems = new List<WorkshopItem>();
    public GameObject appearParticles;

    [HideInInspector] public GameObject transitionPanel;
    [HideInInspector] public Button transitionButton;
    [HideInInspector] public int numberOfOpenPanels = 0;
    [HideInInspector] public bool endOfEra = false;

    public EraController eraController;
    [ContextMenu("Awake")]
    public void Awake()
    {
        Instance = this;

        if (transitionPanel == null)
        {
            Debug.Log("Transition panel in stortage is null");
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
        roomObjects = objects;
        endOfEra = isEnd;

        //if(transitionPanel != null)
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
        roomObjects[index].SetActive(true);
        Instantiate(appearParticles, roomObjects[index].transform.position, new Quaternion(0, 0, 0, 0));
        workshopPanelItems[index].ReplaceOldObjects();

        // if (index == 3)
        // {
        //     CustomerController.Instance.ChangeAnimation();
        //     MoneyHandler.Instance.StopCurrencyCoroutine();
        // }
    }

    public void UnlockObjectWithoutParticles(int index)
    {
        roomObjects[index].SetActive(true);
        workshopPanelItems[index].ReplaceOldObjects();
    }
}