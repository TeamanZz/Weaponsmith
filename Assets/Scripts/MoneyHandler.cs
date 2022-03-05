using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoneyHandler : MonoBehaviour
{
    public static MoneyHandler Instance;

    public long moneyCount;

    [SerializeField] private int moneyPerSecond;
    [SerializeField] private GameObject currencyPopupPrefab;
    [SerializeField] private Transform currencyPopupContainer;
    [SerializeField] private TextMeshProUGUI moneyPerSecondText;
    [SerializeField] private TextMeshProUGUI moneyCountText;
    [SerializeField] private Transform canvasTransform;

    private float timeBetweenMoneyIncrease = 0.5f;
    private Coroutine currencyCoroutine;

    [Header("All connect initialization")]
    public PanelsHandler panelsHandler;
    public ItemsManager itemsManager;
    public RoomObjectsHandler objectsHandler;
    public DungeonItemManager dungeonItemManager;
    public BoostersManager boostersManager;

    public float boosterCoefficient = 1;

    private void Awake()
    {
        Instance = this;
        moneyPerSecond = PlayerPrefs.GetInt("MoneyPerSecond");
        if (moneyPerSecond == 0)
            moneyPerSecond = 1;
    }

    public void ChangeBoosterCoefficient(float value)
    {
        boosterCoefficient = value;
    }

    private void SaveMoneyPerSec()
    {
        PlayerPrefs.SetInt("MoneyPerSecond", moneyPerSecond);
    }

    private void Start()
    {
        InvokeRepeating("SaveMoneyPerSec", 1, 3);
        StartCoroutine(IEIncreaseMoneyCount());

        string state = PlayerPrefs.GetString("WorkshopGameobject3");
        if (state != "unlocked")
        {
            currencyCoroutine = StartCoroutine(IESpawnCurrency());
        }
    }

    public void StopCurrencyCoroutine()
    {
        StopCoroutine(currencyCoroutine);
    }

    public void IncreaseMoneyPerSecondValue(int value)
    {
        moneyPerSecond += value;
    }

    private void FixedUpdate()
    {
        moneyPerSecondText.text = FormatNumsHelper.FormatNum((double)moneyPerSecond * (double)boosterCoefficient) + "/s";
    }

    private IEnumerator IEIncreaseMoneyCount()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenMoneyIncrease);
            moneyCount += moneyPerSecond * (long)boosterCoefficient;
            moneyCountText.text = FormatNumsHelper.FormatNum((double)moneyCount);
        }
    }

    private IEnumerator IESpawnCurrency()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            SpawnCurrencyPopUp();
        }
    }

    public void SpawnCurrencyPopUp()
    {
        var newPopup = Instantiate(currencyPopupPrefab, currencyPopupContainer);
        newPopup.GetComponent<CurrencyPopup>().currencyText.text = "+ " + FormatNumsHelper.FormatNum((double)moneyPerSecond * 3 * (double)boosterCoefficient) + "$";
        newPopup.transform.SetAsFirstSibling();
    }

    public void IncreaseIncomeByTap()
    {
        var increaseValue = moneyPerSecond / 10;
        if (increaseValue == 0)
            moneyCount++;
        else
            moneyCount += increaseValue;
        moneyCountText.text = FormatNumsHelper.FormatNum((double)moneyCount);

    }
}