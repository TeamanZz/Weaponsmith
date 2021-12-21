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

    public void SpawnNewItem(int itemIndex, Vector3 position, Quaternion rotation)
    {
        if (!isSpawned)
        {
            var newItem = Instantiate(wholeItems[itemIndex], position, rotation);
            // var spawnParticles = Instantiate(spawnParticlesPrefab, position + new Vector3(0, 0.5f, 0), rotation);
            isSpawned = true;
            StartCoroutine(SpawnDelayCoroutine());
        }
    }

    public void UnlockCraftWeapon(int index)
    {
        craftPanelBodies[index].gameObject.SetActive(true);
        craftPanelTips[index].gameObject.SetActive(true);
    }
}