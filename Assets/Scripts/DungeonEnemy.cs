using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemy : MonoBehaviour
{
    private DungeonCharacter dungeonCharacter;
    private Animator animator;

    public List<GameObject> currentEnemySkin = new List<GameObject>();
    

    public int dropRate = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        dropRate = PanelsHandler.Instance.dropChanceImprovements;

        if (SkinsManager.Instance.skinCount > 0)
        {
            int random = Random.Range(0, SkinsManager.Instance.skinCount);
            random = Mathf.Clamp(random, 0, currentEnemySkin.Count - 1);

            currentEnemySkin[random].SetActive(true);
        }
        else
            currentEnemySkin[0].SetActive(true);

        PanelsHandler.Instance.dropChanceImprovements -= 1;
        if (PanelsHandler.Instance.dropChanceImprovements <= 0)
            PanelsHandler.Instance.dropChanceImprovements = 7;
    }

    public void InvokeDeathAnimation()
    {
        if (ItemsManager.Instance.currentWaitingPanel != null && ItemsManager.Instance.currentWaitingPanel.currentState == PanelItemState.WaitingForDrawing)
        {
            int val = Random.Range(0, dropRate);
            Debug.Log("Value - " + val + " | " + "Drop rate - " + dropRate);

            if (val == 0)
            {
                ItemsManager.Instance.OpenWaitingPanel();
                PanelsHandler.Instance.dropChanceImprovements = 7;
            }
        }
        animator.SetTrigger("Death");
    }
}