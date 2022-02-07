using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemy : MonoBehaviour
{
    private DungeonCharacter dungeonCharacter;
    private Animator animator;

    public int dropRate = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        dropRate = PanelsHandler.Instance.dropChanceImprovements;

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