using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public static CraftManager Instance;

    public List<GameObject> itemsBodies = new List<GameObject>();
    public List<GameObject> itemsTips = new List<GameObject>();
    public List<GameObject> wholeItems = new List<GameObject>();

    public List<CraftItem> craftPanelBodies = new List<CraftItem>();
    public List<CraftItem> craftPanelTips = new List<CraftItem>();
    public GameObject spawnParticles;

    public bool isSpawned;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator SpawnDelayCoroutine()
    {
        yield return new WaitForSeconds(1);
        isSpawned = false;
    }

    public GameObject GetCraftItemPiece(ItemType itemType, CraftItemPiece itemPiece)
    {
        List<GameObject> itemsList = new List<GameObject>();

        if (itemType == ItemType.Body)
            itemsList = itemsBodies;
        else
            itemsList = itemsTips;

        return itemsList[(int)itemPiece];
    }

    public void SpawnNewItem(int itemIndex, Vector3 position, Quaternion rotation)
    {
        if (!isSpawned)
        {
            var newItem = Instantiate(wholeItems[itemIndex], position, rotation);
            var newParticles = Instantiate(spawnParticles, newItem.transform.position, Quaternion.identity);
            SFX.Instance.PlayMade();
            isSpawned = true;
            StartCoroutine(SpawnDelayCoroutine());
        }
    }

    public void UnlockCraftWeapon(int index)
    {
        // if (index == 0)
        // {
        //     craftPanelBodies[0].gameObject.SetActive(false);
        //     craftPanelTips[0].gameObject.SetActive(false);
        //     // craftPanelBodies[1].gameObject.SetActive();
        //     craftPanelTips[1].gameObject.SetActive(false);
        //     craftPanelBodies[2].gameObject.SetActive(false);
        //     craftPanelTips[2].gameObject.SetActive(false);
        // }
        // if (index >= 3 && index < 6)
        // {
        //     craftPanelBodies[3].gameObject.SetActive(false);
        //     craftPanelTips[3].gameObject.SetActive(false);
        //     craftPanelBodies[4].gameObject.SetActive(false);
        //     craftPanelTips[4].gameObject.SetActive(false);
        //     craftPanelBodies[5].gameObject.SetActive(false);
        //     craftPanelTips[5].gameObject.SetActive(false);
        // }
        // if (index >= 6 && index < 8)
        // {
        //     craftPanelBodies[6].gameObject.SetActive(false);
        //     craftPanelTips[6].gameObject.SetActive(false);
        //     craftPanelBodies[7].gameObject.SetActive(false);
        //     craftPanelTips[7].gameObject.SetActive(false);
        // }
        Debug.Log(index + 1);
        craftPanelBodies[index + 1].gameObject.SetActive(true);
        craftPanelTips[index + 1].gameObject.SetActive(true);
    }
}