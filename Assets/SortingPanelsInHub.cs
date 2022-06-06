using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingPanelsInHub : MonoBehaviour
{
    public Transform weaponsGroup;
    public Transform armorsGroup;

    public List<int> indexLisr;

    [ContextMenu("Sorting")]
    public void Sorting()
    {
        List<PanelItemInHub> weaponsPanels = new List<PanelItemInHub>();
        weaponsPanels.AddRange(weaponsGroup.GetComponentsInChildren<PanelItemInHub>());

        indexLisr.Clear();
        foreach (var pan in weaponsPanels)
        {
            indexLisr.Add(pan.panelID);
        }

        for (int layer = 0; layer < indexLisr.Count; layer++)
        {
            for (int index = layer + 1; index < indexLisr.Count; index++)
            {
                if (indexLisr[layer] > indexLisr[index])
                {
                    int tempID = indexLisr[layer];
                    indexLisr[layer] = indexLisr[index];
                    indexLisr[index] = tempID;
                }
            }
        }

        for (int layer = 0; layer < weaponsPanels.Count; layer++)
        {
            for (int index = layer + 1; index < weaponsPanels.Count; index++)
            {
                Debug.Log("Start Layer " + weaponsPanels[layer].transform.GetSiblingIndex() + " | id" + weaponsPanels[index].transform.GetSiblingIndex());

                if (weaponsPanels[layer].panelID > weaponsPanels[index].panelID)
                {
                    PanelItemInHub hubTemp = weaponsPanels[index];
                    int tempID = weaponsPanels[layer].transform.GetSiblingIndex();
                    weaponsPanels[layer].transform.SetSiblingIndex(weaponsPanels[index].transform.GetSiblingIndex());
                    hubTemp.transform.SetSiblingIndex(tempID);
                    Debug.Log("Layer Zam");
                }
                Debug.Log("Past Layer " + weaponsPanels[layer].transform.GetSiblingIndex() + " | id" + weaponsPanels[index].transform.GetSiblingIndex());

            }
        }

    }
}
