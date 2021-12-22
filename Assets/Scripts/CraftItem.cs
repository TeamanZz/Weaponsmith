using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftItem : MonoBehaviour
{
    public int index;
    public ItemType itemType;

    public void SpawnItem()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-2.4f, 1.6f), 4.3f, Random.Range(-0.021f, -2.279f));
        if (itemType == ItemType.Body)
        {
            var newItem = Instantiate(CraftManager.Instance.itemsBodies[index], spawnPosition, Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Random.Range(0, 360)));
        }
        else
        {
            var newItem = Instantiate(CraftManager.Instance.itemsTips[index], spawnPosition, Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Random.Range(0, 360)));
        }
    }

}
public enum ItemType
{
    Body,
    Tip
}