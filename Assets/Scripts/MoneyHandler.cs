using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyHandler : MonoBehaviour
{
    public static MoneyHandler Instance;

    [SerializeField] private int moneyCount;
    [SerializeField] private int moneyPerSecond;

    [SerializeField] private TextMeshProUGUI moneyPerSecondText;
    [SerializeField] private TextMeshProUGUI moneyCountText;

    private float timeBetweenMoneyIncrease = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(IEIncreaseMoneyCount());
    }

    public void IncreaseMoneyPerSecondValue(int value)
    {
        moneyPerSecond += value;
    }

    private void FixedUpdate()
    {
        moneyPerSecondText.text = "$ " + moneyPerSecond.ToString() + "/s";
    }

    public int GetRewardForCraft()
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
            moneyCountText.text = moneyCount.ToString() + " $";
        }
    }
}