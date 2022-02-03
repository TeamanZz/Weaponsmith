using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemy : MonoBehaviour
{
    private DungeonCharacter dungeonCharacter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void InvokeDeathAnimation()
    {
        if (ItemsManager.Instance.currentWaitingPanel != null && ItemsManager.Instance.currentWaitingPanel.currentState == PanelItemState.WaitingForDrawing)
        {
            int val = Random.Range(0, 11);
            if (val == 0)
            {
                ItemsManager.Instance.OpenWaitingPanel();
            }
        }
        animator.SetTrigger("Death");
    }
}