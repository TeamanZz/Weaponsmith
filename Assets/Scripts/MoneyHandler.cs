using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyHandler : MonoBehaviour
{
    public static MoneyHandler Instance;

    public int moneyCount;
    [SerializeField] private int moneyPerSecond;
    [SerializeField] private GameObject currencyPopupPrefab;

    [SerializeField] private TextMeshProUGUI moneyPerSecondText;
    [SerializeField] private TextMeshProUGUI moneyCountText;

    private float timeBetweenMoneyIncrease = 0.5f;
    public Transform canvasTransform;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(IEIncreaseMoneyCount());
        StartCoroutine(IESpawnCurrency());
    }

    public void IncreaseMoneyPerSecondValue(int value)
    {
        moneyPerSecond += value;
    }

    private void FixedUpdate()
    {
        moneyPerSecondText.text = "$ " + FormatNumsHelper.FormatNum((float)moneyPerSecond) + "/s";
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
            moneyCountText.text = FormatNumsHelper.FormatNum((float)moneyCount);
        }
    }

    private IEnumerator IESpawnCurrency()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            var newPopup = Instantiate(currencyPopupPrefab, canvasTransform);
            newPopup.GetComponent<CurrencyPopup>().currencyText.text = "+ " + FormatNumsHelper.FormatNum((float)moneyPerSecond * 3) + "$";
            newPopup.transform.SetAsFirstSibling();
        }
    }
}