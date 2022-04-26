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

    //[Header("improvement points")]
    //public int maxPoints = 24;
    //public int improvementCount = 0;
    //public int currentImprovementPoints = 0;

    private void Awake()
    {
        Instance = this;
        moneyPerSecond = PlayerPrefs.GetInt("MoneyPerSecond");
        if (moneyPerSecond == 0)
            moneyPerSecond = 1;
    }

    //[ContextMenu("Add Improvement Point")]
    //public void AddImprovementPoint()
    //{
    //    if (improvementCount >= maxPoints)
    //        return;

    //    currentImprovementPoints += 1;
    //    LoadoutController.Instance.UpdateImprovementPoints();
    //}

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