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
    public GameObject textPopup;

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
                transform.localScale = Vector3.one * 1.5f;
                boxCollider.center = new Vector3(0, 0, boxCollider.center.z / 1.5f);
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
            chestLid.DOLocalRotate(openedLidRotation, openTime).SetEase(Ease.OutBack);
            TakeReward();
            RemoveChest();
            openParticles.Play();
            var newPopup = Instantiate(textPopup, transform.position + new Vector3(0, 3, 0), Quaternion.Euler(32, 0, 0));
            newPopup.GetComponent<DungeonPopupText>().InitializeRewardText(currentMoneyReward.ToString());
            SFX.Instance.PlayChestOpen();
        }
    }

    private void RemoveChest()
    {
        var s = DOTween.Sequence();
        s.Append(transform.DOLocalMoveY(transform.position.y + 3, 0.8f).SetEase(Ease.OutBack));
        s.Append(transform.DOLocalMoveY(transform.position.y - 9f, 0.6f).SetEase(Ease.InBack));
        transform.DOLocalRotate(new Vector3(0, -720, 0), 1.5f).SetEase(Ease.InOutBack);
        transform.DOScale(Vector3.zero, 1.5f).SetEase(Ease.InBack);
    }

    public enum ChestFilling
    {
        Money,
        BlueprintAndMoney
    }
}