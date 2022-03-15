using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DungeonWeaponBlueprint : MonoBehaviour
{
    private bool canInteract;

    void Start()
    {
        transform.DOLocalJump(transform.localPosition + new Vector3(0, 0, 10), 4, 1, 1f).SetEase(Ease.OutBounce);
        Invoke("EnableInteract", 0.3f);
        GetComponent<AudioSource>().Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canInteract)
            return;

        DungeonCharacter character;
        if (other.TryGetComponent<DungeonCharacter>(out character))
        {
            CraftPanelItemsManager.Instance.OpenWaitingPanel();
            canInteract = false;
        }
    }

    private void EnableInteract()
    {
        canInteract = true;
    }
}