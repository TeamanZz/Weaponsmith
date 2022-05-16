using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoneyHandler : MonoBehaviour
{
    public static MoneyHandler Instance;

    [Header("Currency Values")]
    public long moneyCount;
    public int moneyPerSecond;

    [Header("Currency Popup")]
    [SerializeField] private GameObject currencyPopupPrefab;
    [SerializeField] private Transform currencyPopupContainer;

    [Header("View")]
    [SerializeField] private TextMeshProUGUI moneyCountText;
    [SerializeField] private TextMeshProUGUI moneyPerSecondText;

    private float timeBetweenMoneyIncrease = 0.5f;
    private float boosterCoefficient = 1;

    public Coroutine increaseByTapCoroutine;
    public int increaseByTapMoneyCount;

    public bool isIncreasedByTap;
    private int increaseByTapAdditiveValue;

    private void Awake()
    {
        Instance = this;
        moneyPerSecond = PlayerPrefs.GetInt("MoneyPerSecond");
        if (moneyPerSecond == 0)
            moneyPerSecond = 1;
    }

    private void Start()
    {
        InvokeRepeating("SaveMoneyPerSec", 1, 3);
        StartCoroutine(IEIncreaseMoneyCount());
        StartCoroutine(IESpawnCurrency());
    }

    private void FixedUpdate()
    {
        UpdateMoneyPerSecondView();
    }

    private void UpdateMoneyPerSecondView()
    {
        moneyPerSecondText.text = FormatNumsHelper.FormatNum((double)moneyPerSecond * (double)boosterCoefficient) + "/s";
    }

    public void ChangeBoosterCoefficient(float value)
    {
        boosterCoefficient = value;
    }

    private void SaveMoneyPerSec()
    {
        // PlayerPrefs.SetInt("MoneyPerSecond", moneyPerSecond);
    }

    public void IncreaseMoneyPerSecondValue(int value)
    {
        moneyPerSecond += value;
    }

    public void IncreaseIncomeByTap()
    {
        if (isIncreasedByTap)
            return;

        isIncreasedByTap = true;
        increaseByTapAdditiveValue = (int)(moneyPerSecond * 0.1f);
        moneyPerSecond += increaseByTapAdditiveValue;
    }

    public void DecreaseIncomeByTap()
    {
        isIncreasedByTap = false;
        moneyPerSecond -= increaseByTapAdditiveValue;
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
        var newPopup = Instantiate(currencyPopupPrefab, new Vector3(-1000, 1, 0), Quaternion.identity, currencyPopupContainer);
        newPopup.GetComponent<CurrencyPopup>().currencyText.text = "+ " + FormatNumsHelper.FormatNum((double)moneyPerSecond * (double)boosterCoefficient) + "$";
        newPopup.transform.SetAsFirstSibling();
    }
}