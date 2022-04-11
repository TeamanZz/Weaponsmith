using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class DungeonRewardPanel : MonoBehaviour
{
    public static DungeonRewardPanel dungeonRewardPanel;
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

    public List<RewardUIItem> rewardsList = new List<RewardUIItem>();

    [Header("Data")]
    public int currentStar = 2;
    public bool isWon = false;

    public void Awake()
    {
        dungeonRewardPanel = this;
    }

    [ContextMenu("Test")]
    public void Test()
    {
        Initialization(isWon, currentStar);
    }

    public void Initialization(bool isWon, int starsValue)
    {
        Debug.Log("State = " + isWon + "Value = " + starsValue);
        for (int i = 0; i < starsViewImage.Length; i++)
        {
            starsViewImage[i].transform.localScale = Vector3.zero;
        }
         
        switch (isWon)
        {
            case true:
                titleText.text = resultInTitle[0]; 
                break;

            case false:
                titleText.text = resultInTitle[1];
                break;
        }

        panel.SetActive(true);

        if (coroutine != null)
            StopCoroutine(coroutine);
        
       coroutine = StartCoroutine(StarsInitialization(starsValue));
    }

    public void AddItem(Sprite sprite, string title)
    {
        Debug.Log("Add " + title);
        RewardUIItem newItem = Instantiate(itemPrefab, group.transform);
        newItem.InitializationPanel(sprite, title);

        rewardsList.Add(newItem);
    }

    public void ClosedPanel()
    {
        panel.SetActive(false);
        foreach(var item in rewardsList)
        {
            Destroy(item.gameObject);
        }
        rewardsList.Clear();
    }
    public IEnumerator StarsInitialization(int number)
    {
        number = Mathf.Clamp(number, 0, starsViewImage.Length);
        for (int i = 0; i < number; i++)
        {
            yield return new WaitForSeconds(timeToNewStar);
            starsViewImage[i].rectTransform.DOScale(Vector3.one, openStarTime);
            Debug.Log(i);
        }
    }
}
