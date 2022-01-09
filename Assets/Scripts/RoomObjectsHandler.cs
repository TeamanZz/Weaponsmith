using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectsHandler : MonoBehaviour
{
    public static RoomObjectsHandler Instance;

    public List<GameObject> roomObjects = new List<GameObject>();
    public List<WorkshopItem> workshopPanelItems = new List<WorkshopItem>();
    public GameObject appearParticles;

    private void Awake()
    {
        Instance = this;
    }

    public void UnlockObject(int index)
    {
        roomObjects[index].SetActive(true);
        Instantiate(appearParticles, roomObjects[index].transform.position, new Quaternion(0, 0, 0, 0));

        if (index == 3)
        {
            CustomerController.Instance.ChangeAnimation();
        }
    }

    public void UnlockObjectWithoutParticles(int index)
    {
        roomObjects[index].SetActive(true);
    }
}