using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DungeonChest : MonoBehaviour
{
    [HideInInspector] public int currentMoneyReward;

    [SerializeField] private Transform chestLid;
    [SerializeField] private Vector3 openedLidRotation = Vector3.zero;
    [SerializeField] private float openTime = 0.25f;
    [SerializeField] private List<GameObject> rewardsVisualObject = new List<GameObject>();

    private ChestFilling currentFilling;

    public void Initialize(ChestFilling filling)
    {
        currentFilling = filling;
        switch (filling)
        {
            case ChestFilling.Money:
                rewardsVisualObject[0].SetActive(true);
                currentMoneyReward = 100;
                break;

            case ChestFilling.BlueprintAndMoney:
                rewardsVisualObject[1].SetActive(true);
                break;
        }
    }

    public void TakeReward()
    {
        switch (currentFilling)
        {
            case ChestFilling.Money:
                MoneyHandler.Instance.moneyCount += currentMoneyReward;
                break;

            case ChestFilling.BlueprintAndMoney:
                DungeonRewardPanel.Instance.OpenRewardPanel(3);
                break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        DungeonCharacter character;
        if (other.TryGetComponent<DungeonCharacter>(out character))
        {
            chestLid.DOLocalRotate(openedLidRotation, openTime);
            TakeReward();
        }
    }

    public enum ChestFilling
    {
        Money,
        BlueprintAndMoney
    }
}