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
    public bool panelIsActive = true;

    [Header("Viewing Elements")]
    //public TextMeshProUGUI titleText;
    public Image[] starsViewImage;
    public float openStarTime = 0.25f;
    public float timeToNewStar = 0.5f;
    public Coroutine coroutine;
    public Sprite moneySprite;
    public Sprite weaponSprite;
    public Sprite armorSprite;
    public GridLayoutGroup group;
    //public RewardUIItem itemPrefab;
    public PanelsStortage panelsStorage;

    //public RewardUIItem moneyReward;
    //public RewardUIItem blueprintReward;

    [Space]
    public GameObject crystalGroup;
    public GameObject blueprintGroup;

    //public TextMeshProUGUI allMoney;
    public TextMeshProUGUI moneyForEnemies;
    public TextMeshProUGUI moneyForChest;

    public SkillController skillController;

    [Space]
    public GameObject bottomBlueprintsGroup;
    public GameObject bottomCrystalGroup;
    public TextMeshProUGUI allMoneyBottomText;

    public GameObject successTitle;
    public GameObject lostTitle;

    public GameObject successParticles;
    public GameObject lostParticles;

    public void Awake()
    {
        Instance = this;
    }

    public void InitializeBlueprintItem()
    {
        var newAnvilItem = panelsStorage.panelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (newAnvilItem == null)
            return;

    }

    
    public void ClosePanel()
    {
        blueprintGroup.SetActive(false);
        crystalGroup.SetActive(false);
        
        panelIsActive = false;
        panel.SetActive(false);
        PanelsHandler.Instance.StartCoolDown();
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

    [ContextMenu("Debug")]
    public void Debuger()
    {
        OpenRewardPanel(3);
    }

    public void OpenRewardPanel(int starsValue)
    {
        panelIsActive = true;
        DungeonCharacter.Instance.animator.SetBool("IsDungeonStarted", false);
        DungeonCharacter.Instance.animator.SetBool("EnemyIsNear", false);
        DungeonCharacter.Instance.isInBattle = false;
        DungeonManager.Instance.isDungeonStarted = false;
        DungeonManager.Instance.bossZPosition = 0;

        moneyForEnemies.text = "enemies: " + FormatNumsHelper.FormatNum((double)DungeonManager.Instance.currentLevelEarnedMoneyCount);
        moneyForChest.text = "chest: " + FormatNumsHelper.FormatNum((double)DungeonManager.Instance.currentChestReward);

        allMoneyBottomText.text = FormatNumsHelper.FormatNum((double)(DungeonManager.Instance.currentLevelEarnedMoneyCount + DungeonManager.Instance.currentChestReward));

        DungeonManager.Instance.currentLevelEarnedMoneyCount = 0;
        DungeonManager.Instance.currentChestReward = 0;

        DungeonManager.Instance.blueprintRecieved = false;

        for (int i = 0; i < starsViewImage.Length; i++)
        {
            starsViewImage[i].transform.localScale = Vector3.zero;
        }

        if (starsValue == 3)
        {
            lostTitle.SetActive(false);
            successTitle.SetActive(true);
            successParticles.SetActive(true);
            lostParticles.SetActive(false);
            if (DungeonBuilder.Instance.lastChest != null)
                DungeonBuilder.Instance.lastChest.PlayRewardAnimation();

            DungeonManager.Instance.currentDungeonLevelId = Mathf.Clamp(DungeonManager.Instance.currentDungeonLevelId + 1, 0, DungeonBuilder.Instance.levels.Count);
            DungeonManager.Instance.SaveData();

            Debug.Log("ID = " + DungeonManager.Instance.currentDungeonLevelId);

            SkillController.skillController.OpenNewSkill();
            SkillController.skillController.AddImprovementPoint();
        }


        var nextItem = CraftPanelItemsManager.Instance.craftPanelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (starsValue == 3)
        {
            if (nextItem == null)
            {
                bottomBlueprintsGroup.SetActive(false);
                blueprintGroup.SetActive(false);
            }
            else
            {
                bottomBlueprintsGroup.SetActive(true);
                blueprintGroup.SetActive(true);
            }

            int value = skillController.improvementCount + skillController.improvementCount;

            if (value >= skillController.maxPoints)
            {
                bottomCrystalGroup.SetActive(false);
                crystalGroup.SetActive(false);
            }
            else
            {
                bottomCrystalGroup.SetActive(true);
                crystalGroup.SetActive(true);
            }
        }
        else
        {
            blueprintGroup.SetActive(false);

            bottomCrystalGroup.SetActive(false);
            crystalGroup.SetActive(false);

            lostTitle.SetActive(true);
            successTitle.SetActive(false);
            successParticles.SetActive(false);
            lostParticles.SetActive(true);
        }

        panel.SetActive(true);

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(StarsInitialization(starsValue));
    }
}