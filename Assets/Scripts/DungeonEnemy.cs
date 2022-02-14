using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemy : MonoBehaviour
{

    [SerializeField] private List<Animator> animators = new List<Animator>();
    public List<GameObject> currentEnemySkin = new List<GameObject>();
    public int skinCount;

    private DungeonCharacter dungeonCharacter;
    [HideInInspector] public int dropRate = 0;

    public GameObject blueprintPrefab;
    private void Awake()
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
            Debug.Log("heh1");

            foreach (GameObject obj in currentEnemySkin)
            {
                obj.SetActive(false);
            }
            currentEnemySkin[random].SetActive(true);
            Debug.Log("heh2");
            if (random == 0)
            {

                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                GetComponent<BoxCollider>().center += new Vector3(0, 0, 2.5f);
            }
        }
        else
        {
            Debug.Log("heh3");

            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            GetComponent<BoxCollider>().center += new Vector3(0, 0, 2.5f);
            currentEnemySkin[0].SetActive(true);
        }
        Debug.Log("heh4");

        PanelsHandler.Instance.dropChanceImprovements -= 1;
        if (PanelsHandler.Instance.dropChanceImprovements <= 0)
            PanelsHandler.Instance.dropChanceImprovements = 7;
    }

    public void InvokeDeathAnimation()
    {
        // Instantiate(blueprintPrefab, transform.position + new Vector3(0, 0.325f, 0), Quaternion.identity, transform.parent);

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