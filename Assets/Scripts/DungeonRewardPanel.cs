using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class DungeonRewardPanel : MonoBehaviour
{
    public static DungeonRewardPanel Instance;
    public GameObject panel;

    [Header("Viewing Elements")]
    public TextMeshProUGUI titleText;
    public string[] resultInTitle;
    public Image[] starsViewImage;
    public float openStarTime = 0.25f;
    public float timeToNewStar = 0.5f;
    public Coroutine coroutine;
    public Sprite moneySprite;
    public Sprite weaponSprite;
    public Sprite armorSprite;
    public GridLayoutGroup group;
    public RewardUIItem itemPrefab;
    public PanelsStortage panelsStorage;

    public RewardUIItem moneyReward;
    public RewardUIItem blueprintReward;

    public void Awake()
    {
        Instance = this;
    }

    public void InitializeBlueprintItem()
    {
        var newAnvilItem = panelsStorage.panelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (newAnvilItem == null)
            return;
        if (newAnvilItem.currentEquipmentType == ItemEquipment.EquipmentType.Weapon)
        {
            blueprintReward.InitializePanel(weaponSprite, "New weapon blueprint");
        }
        else if (newAnvilItem.currentEquipmentType == ItemEquipment.EquipmentType.Armor)
        {
            blueprintReward.InitializePanel(armorSprite, "New armor blueprint");
        }
    }

    public void ClosePanel()
    {
        blueprintReward.gameObject.SetActive(false);
        panel.SetActive(false);
    }

    public IEnumerator StarsInitialization(int number)
    {
        number = Mathf.Clamp(number, 0, starsViewImage.Length);
        for (int i = 0; i < number; i++)
        {
            yield return new WaitForSeconds(timeToNewStar);
            starsViewImage[i].rectTransform.DOScale(Vector3.one, openStarTime);
        }
    }

    public void OpenRewardPanel(int starsValue)
    {
        DungeonCharacter.Instance.animator.SetBool("IsDungeonStarted", false);
        DungeonCharacter.Instance.animator.SetBool("EnemyIsNear", false);
        DungeonCharacter.Instance.isInBattle = false;
        DungeonManager.Instance.isDungeonStarted = false;
        DungeonManager.Instance.bossZPosition = 0;

        moneyReward.InitializePanel(moneySprite, DungeonManager.Instance.currentLevelEarnedMoneyCount.ToString());
        DungeonManager.Instance.currentLevelEarnedMoneyCount = 0;

        if (DungeonManager.Instance.blueprintRecieved)
        {
            blueprintReward.gameObject.SetActive(true);
        }
        DungeonManager.Instance.blueprintRecieved = false;

        for (int i = 0; i < starsViewImage.Length; i++)
        {
            starsViewImage[i].transform.localScale = Vector3.zero;
        }

        if (starsValue == 3)
        {
            if (DungeonBuilder.Instance.lastChest != null)
                DungeonBuilder.Instance.lastChest.PlayRewardAnimation();

            titleText.text = resultInTitle[0];
        }
        else
            titleText.text = resultInTitle[1];

        panel.SetActive(true);

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(StarsInitialization(starsValue));
    }
}