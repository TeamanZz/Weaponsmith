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
    public ParticleSystem openParticles;
    public Transform cube;
    public BoxCollider boxCollider;
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
                CraftPanelItemsManager.Instance.OpenNewBlueprint();
                DungeonCharacter.Instance.DisableCharacterRun();
                MoneyHandler.Instance.AddImprovementPoint();
                break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        DungeonCharacter character;
        if (other.TryGetComponent<DungeonCharacter>(out character))
        {
            boxCollider.enabled = false;
            chestLid.DOLocalRotate(openedLidRotation, openTime);
            TakeReward();
            RemoveChest();
            openParticles.Play();
        }
    }

    private void RemoveChest()
    {
        cube.DOLocalMoveY(cube.transform.position.y - 5, 0.5f).SetEase(Ease.InBack);
        transform.DOLocalRotate(new Vector3(0, -1000, 0), 1.5f).SetEase(Ease.InBack);
        transform.DOLocalMoveY(transform.position.y - 6f, 1.5f).SetEase(Ease.InBack);
    }

    public enum ChestFilling
    {
        Money,
        BlueprintAndMoney
    }
}