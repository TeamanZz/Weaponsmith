using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public static CraftManager Instance;

    public GameObject newItemPrefab;
    //public GameObject spawnParticlesPrefab;

    public bool isSpawned;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnNewItem(Vector3 position, Quaternion rotation)
    {
        if (!isSpawned)
        {
            var newItem = Instantiate(newItemPrefab, position, rotation);
            // var spawnParticles = Instantiate(spawnParticlesPrefab, position + new Vector3(0, 0.5f, 0), rotation);

            isSpawned = true;
        }
    }
}