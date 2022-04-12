using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DungeonChest : MonoBehaviour
{
    [HideInInspector] public int currentMoneyReward;

    [SerializeField] private ChestFilling currentFilling;
    [SerializeField] private GameObject[] rewardsVisualObject;
    [SerializeField] private Transform chestover;
    [SerializeField] private Vector3 endPosition = Vector3.zero;
    [SerializeField] private float openTime = 0.25f;

    private bool canInteract;

    void Start()
    {
        transform.DOLocalJump(transform.localPosition + new Vector3(0, 0, 10), 4, 1, 1f).SetEase(Ease.OutBounce);
        Invoke("EnableInteract", 0.3f);
        GetComponent<AudioSource>().Play();
        chestover.eulerAngles = new Vector3(0, 0, 0);
    }

    public void Initialization(ChestFilling filling)
    {
        switch (filling)
        {
            case ChestFilling.Money:
                rewardsVisualObject[0].SetActive(true);
                currentMoneyReward = 100;
                //DungeonRewardPanel.dungeonRewardPanel.AddItem(DungeonRewardPanel.dungeonRewardPanel.moneySprite, currentMoneyReward.ToString());
                break;

            case ChestFilling.BlueprintAndMoney:
                rewardsVisualObject[1].SetActive(true);
                //DungeonRewardPanel.dungeonRewardPanel.AddItem(DungeonRewardPanel.dungeonRewardPanel.armorSprite, "New Blueprint");
                //DungeonRewardPanel.dungeonRewardPanel.AddItem(DungeonRewardPanel.dungeonRewardPanel.weaponSprite, "New Blueprint");
                break;
        }
        Debug.Log(currentFilling);
    }

    public void TakeReward()
    {
        if (!canInteract)
            return;

        switch (currentFilling)
        {
            case ChestFilling.Money:
                MoneyHandler.Instance.moneyCount += currentMoneyReward;
                break;

            case ChestFilling.BlueprintAndMoney:
                //CraftPanelItemsManager.Instance.OpenWaitingPanel();
                break;
        }

        Debug.Log("Reward");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!canInteract)
            return;

        DungeonCharacter character;
        if (other.TryGetComponent<DungeonCharacter>(out character))
        {
            chestover.DOLocalRotate(endPosition, openTime);
            TakeReward();
            canInteract = false;
        }
    }

    private void EnableInteract()
    {
        canInteract = true;
    }

    public enum ChestFilling
    {
        Money,
        BlueprintAndMoney
    }
}