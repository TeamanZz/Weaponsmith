using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DungeonWeaponBlueprint : MonoBehaviour
{
    private bool canInteract;
    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<Animator>().Play("Dungeon Blueprint Jump");
        transform.DOLocalJump(transform.localPosition + new Vector3(0, 0, 10), 4, 1, 1f).SetEase(Ease.OutBounce);
        Invoke("EnableInteract", 0.3f);
    }

    private void EnableInteract()
    {
        canInteract = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canInteract)
            return;
        Debug.Log("entered");

        DungeonCharacter character;
        if (other.TryGetComponent<DungeonCharacter>(out character))
        {
            Debug.Log("entered2");
            ItemsManager.Instance.OpenWaitingPanel();
            canInteract = false;
        }
    }
}
