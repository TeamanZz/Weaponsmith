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

    public GameObject[] rewardsVisualObject;

    public int currentMoneyReward;
    public Transform chest—over;

    public Vector3 endPosition = Vector3.zero;
    public float openTime = 0.25f;

    public Material material;
    void Start()
    {
        material.color = Color.white;
        transform.DOLocalJump(transform.localPosition + new Vector3(0, 0, 10), 4, 1, 1f).SetEase(Ease.OutBounce);
        Invoke("EnableInteract", 0.3f);
        GetComponent<AudioSource>().Play();
        chest—over.eulerAngles = new Vector3(0, 0, 0);
    }

    public void Initialization(ChestFilling newFilling, int starsNumber)
    {
        Debug.Log(newFilling);
        switch (newFilling)
        {
            case ChestFilling.money:
                rewardsVisualObject[0].SetActive(true);
                rewardsVisualObject[1].SetActive(false);
                currentMoneyReward = MoneyHandler.Instance.moneyPerSecond * starsNumber;
                //DungeonRewardPanel.dungeonRewardPanel.AddItem(DungeonRewardPanel.dungeonRewardPanel.moneySprite, currentMoneyReward.ToString());
                break;

            case ChestFilling.drawing:
                rewardsVisualObject[0].SetActive(false);
                rewardsVisualObject[1].SetActive(true);
                //DungeonRewardPanel.dungeonRewardPanel.AddItem(DungeonRewardPanel.dungeonRewardPanel.armorSprite, "New Blueprint");
                //DungeonRewardPanel.dungeonRewardPanel.AddItem(DungeonRewardPanel.dungeonRewardPanel.weaponSprite, "New Blueprint");
                break;
        }
        Debug.Log(currentFilling);
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
            chest—over.DOLocalRotate(endPosition, openTime);
            //CheckReward();
            canInteract = false;
        }
    }

    private void EnableInteract()
    {
        canInteract = true;
    }
}