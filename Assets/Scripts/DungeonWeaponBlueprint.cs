using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DungeonWeaponBlueprint : MonoBehaviour
{
    private bool canInteract;

    public ChestFilling currentFilling;
    public enum ChestFilling
    {
        money,
        drawing
    }

    public ParticleSystem particle;
    public Color[] rewardsColor;

    public int currentMoneyReward;
    void Start()
    {
        transform.DOLocalJump(transform.localPosition + new Vector3(0, 0, 10), 4, 1, 1f).SetEase(Ease.OutBounce);
        Invoke("EnableInteract", 0.3f);
        GetComponent<AudioSource>().Play();
    }

    public void Initialization(ChestFilling newFilling)
    {
        switch (newFilling)
        {
            case ChestFilling.money:
                particle.startColor = rewardsColor[0];
                currentMoneyReward = MoneyHandler.Instance.moneyPerSecond * DungeonBuilder.Instance.currentStarValue;
                break;

            case ChestFilling.drawing:
                particle.startColor = rewardsColor[1];
                break;
        }
    }

    public void CheckReward()
    {
        if (!canInteract)
            return;

        switch (currentFilling)
        {
            case ChestFilling.money:
                MoneyHandler.Instance.moneyCount += currentMoneyReward;
                break;

            case ChestFilling.drawing:
                CraftPanelItemsManager.Instance.OpenWaitingPanel();
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canInteract)
            return;

        DungeonCharacter character;
        if (other.TryGetComponent<DungeonCharacter>(out character))
        {
            CheckReward();
            canInteract = false;
        }
    }

    private void EnableInteract()
    {
        canInteract = true;
    }
}