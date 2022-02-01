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
    [SerializeField] private TextMeshProUGUI moneyPerSecondText;
    [SerializeField] private TextMeshProUGUI moneyCountText;
    [SerializeField] private Transform canvasTransform;

    private float timeBetweenMoneyIncrease = 0.5f;

    private Coroutine currencyCoroutine;

    [ContextMenu("Awake")]
    private void Awake()
    {
        Instance = this;
        moneyPerSecond = PlayerPrefs.GetInt("MoneyPerSecond");
        if (moneyPerSecond == 0)
            moneyPerSecond = 1;
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
        moneyPerSecondText.text = FormatNumsHelper.FormatNum((double)moneyPerSecond) + "/s";
    }

    public int GetRewardForCraft(int weaponCoefficient)
    {
        int reward = moneyPerSecond * 10;
        moneyCount += reward;
        return reward;
    }

    private IEnumerator IEIncreaseMoneyCount()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenMoneyIncrease);
            moneyCount += moneyPerSecond;
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
        var newPopup = Instantiate(currencyPopupPrefab, canvasTransform);
        newPopup.GetComponent<CurrencyPopup>().currencyText.text = "+ " + FormatNumsHelper.FormatNum((double)moneyPerSecond * 3) + "$";
        newPopup.transform.SetAsFirstSibling();
    }
}