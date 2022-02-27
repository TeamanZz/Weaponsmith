using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEnemy : MonoBehaviour
{
    [SerializeField] private List<Animator> animators = new List<Animator>();
    public List<GameObject> currentEnemySkin = new List<GameObject>();
    public int skinCount;

    private DungeonCharacter dungeonCharacter;
    [HideInInspector] public int dropRate = 0;

    public GameObject blueprintPrefab;
    public float scale = 1.5f;
    public float distanceToCollider = 1.5f;

    public EnemyHealthBar enemyHealthBar;

    private void Start()
    {
        animators.Add(GetComponent<Animator>());
        animators.AddRange(GetComponentsInChildren<Animator>());

        dropRate = PanelsHandler.Instance.dropChanceImprovements;

        skinCount = SkinsManager.Instance.dungeonEnemySkinCount;

        if (skinCount > 0)
        {
            int random = Random.Range(0, SkinsManager.Instance.dungeonEnemySkinCount);
            random = Mathf.Clamp(random, 0, currentEnemySkin.Count);

            Debug.Log(random);

            foreach (GameObject obj in currentEnemySkin)
            {
                obj.SetActive(false);
            }
            currentEnemySkin[random].SetActive(true);
        }
        else
        {
            currentEnemySkin[0].SetActive(true);
        }

        PanelsHandler.Instance.dropChanceImprovements -= 1;
        if (PanelsHandler.Instance.dropChanceImprovements <= 0)
            PanelsHandler.Instance.dropChanceImprovements = 7;

        transform.localScale = Vector3.one * scale;

        BoxCollider collider;
        if (GetComponent<BoxCollider>())
        {
            collider = GetComponent<BoxCollider>();
            collider.center = new Vector3(collider.center.x, collider.center.y, distanceToCollider);
        }
    }

    public void TakeDamage()
    {
        for (int i = 0; i < animators.Count; i++)
            animators[i].Play("Take Damage", 0, 0);
    }

    public void InvokeDeathAnimation()
    {
        if (ItemsManager.Instance.currentWaitingPanel != null && ItemsManager.Instance.currentWaitingPanel.currentState == PanelItemState.WaitingForDrawing)
        {
            int val = Random.Range(0, dropRate);
            Debug.Log("Value - " + val + " | " + "Drop rate - " + dropRate);

            if (val == 0)
            {
                Instantiate(blueprintPrefab, transform.position + new Vector3(0, 0.325f, 0), Quaternion.identity, transform.parent);
                PanelsHandler.Instance.dropChanceImprovements = 7;
            }
        }

        for (int i = 0; i < animators.Count; i++)
            animators[i].SetTrigger("Death");

        Destroy(gameObject, 10f);
    }
}