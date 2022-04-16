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

    public List<RewardUIItem> rewardsList = new List<RewardUIItem>();

    public void Awake()
    {
        Instance = this;
    }

    public void AddItem()
    {
        var newAnvilItem = panelsStorage.panelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        RewardUIItem newItem = Instantiate(itemPrefab, group.transform);

        if (newAnvilItem.currentEquipmentType == ItemEquipment.EquipmentType.Weapon)
        {
            newItem.InitializePanel(weaponSprite, "New weapon blueprint");
        }
        else if (newAnvilItem.currentEquipmentType == ItemEquipment.EquipmentType.Armor)
        {
            newItem.InitializePanel(armorSprite, "New armor blueprint");
        }
        rewardsList.Add(newItem);
    }

    public void AddMoneyItem(string currencyCount)
    {
        RewardUIItem newItem = Instantiate(itemPrefab, group.transform);
        newItem.InitializePanel(moneySprite, currencyCount);
        rewardsList.Add(newItem);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        foreach (var item in rewardsList)
            Destroy(item.gameObject);
        rewardsList.Clear();
        DungeonManager.Instance.ResetDungeon();
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
        DungeonManager.Instance.isDungeonStarted = false;
        for (int i = 0; i < starsViewImage.Length; i++)
        {
            starsViewImage[i].transform.localScale = Vector3.zero;
        }

        if (starsValue == 3)
            titleText.text = resultInTitle[0];
        else
            titleText.text = resultInTitle[1];

        panel.SetActive(true);

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(StarsInitialization(starsValue));
    }
}