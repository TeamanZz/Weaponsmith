using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DungeonChest : MonoBehaviour
{
    public GameObject textPopup;
    public BoxCollider boxCollider;
    public int moneyReward;

    [SerializeField] private ParticleSystem openParticles;
    [SerializeField] private Transform chestLid;
    [SerializeField] private Vector3 openedLidRotation = Vector3.zero;
    [SerializeField] private float openTime = 0.25f;
    [SerializeField] private List<GameObject> rewardsVisualObject = new List<GameObject>();

    private ChestFilling currentFilling;

    public List<ParticleSystem> rewardParticles = new List<ParticleSystem>();

    public void Initialize(ChestFilling filling, int moneyPerChest)
    {
        currentFilling = filling;
        moneyReward = moneyPerChest - (int)(Random.Range(-1, 2) * moneyPerChest * Random.Range(0.03f, 0.1f)); //Отнимаем или прибавляем от 3 до 10 процентов награды сундука
        switch (filling)
        {
            case ChestFilling.Money:
                rewardsVisualObject[0].SetActive(true);
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
                MoneyHandler.Instance.moneyCount += moneyReward;

                break;

            case ChestFilling.BlueprintAndMoney:
                DungeonManager.Instance.blueprintRecieved = true;
                DungeonCharacter.Instance.DisableCharacterRun();
                CraftPanelItemsManager.Instance.OpenNewBlueprint();
                DungeonRewardPanel.Instance.OpenRewardPanel(3);
                SkillController.skillController.AddImprovementPoint();
                break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        DungeonCharacter character;
        if (other.TryGetComponent<DungeonCharacter>(out character))
        {
            DungeonManager.Instance.currentLevelEarnedMoneyCount += moneyReward;
            boxCollider.enabled = false;
            chestLid.DOLocalRotate(openedLidRotation, openTime).SetEase(Ease.OutBack);
            TakeReward();
            RemoveChest();
            openParticles.Play();
            var newPopup = Instantiate(textPopup, transform.position + new Vector3(0, 3, 0), Quaternion.Euler(32, 0, 0));
            newPopup.GetComponent<DungeonPopupText>().InitializeRewardText(moneyReward.ToString());
            SFX.Instance.PlayChestOpen();
        }
    }

    public void PlayRewardAnimation()
    {
        foreach(var particl in rewardParticles)
        {
            particl.gameObject.SetActive(true);
            particl.Play();
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