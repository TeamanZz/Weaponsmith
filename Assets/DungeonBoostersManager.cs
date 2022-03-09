using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBoostersManager : MonoBehaviour
{
    public static DungeonBoostersManager Instance;

    public List<GameObject> boostersHalos = new List<GameObject>();
    private bool goldBoosterEnabled;
    private bool speedBoosterEnabled;
    private bool strengthBoosterEnabled;

    [HideInInspector] public float goldBoosterRemainingTime = 1;
    public float goldBoosterTotalTime;

    [HideInInspector] public float speedBoosterRemainingTime = 1;
    public float speedBoosterTotalTime;

    [HideInInspector] public float strengthBoosterRemainingTime = 1;
    public float strengthBoosterTotalTime;
    public DungeonCharacter dungeonCharacter;

    private void Awake()
    {
        Instance = this;
        goldBoosterRemainingTime = 1;
        speedBoosterRemainingTime = 1;
        strengthBoosterRemainingTime = 1;
    }

    private void Update()
    {
        if (goldBoosterEnabled)
        {
            goldBoosterRemainingTime -= Time.deltaTime / goldBoosterTotalTime;

            if (goldBoosterRemainingTime <= 0)
            {
                boostersHalos[0].SetActive(false);
                goldBoosterEnabled = false;
                goldBoosterRemainingTime = 1;
                MoneyHandler.Instance.ChangeBoosterCoefficient(1f);
                BoostersManager.Instance.panelItemsList[0].HandleUIOnTimerDisable();
            }
        }

        if (speedBoosterEnabled)
        {
            speedBoosterRemainingTime -= Time.deltaTime / speedBoosterTotalTime;

            if (speedBoosterRemainingTime <= 0)
            {
                boostersHalos[1].SetActive(false);
                speedBoosterEnabled = false;
                speedBoosterRemainingTime = 1;
                BoostersManager.Instance.panelItemsList[1].HandleUIOnTimerDisable();
                dungeonCharacter.ChangeCharacterRunAndAttackSpeed(1f, false);
            }
        }

        if (strengthBoosterEnabled)
        {
            strengthBoosterRemainingTime -= Time.deltaTime / strengthBoosterTotalTime;

            if (strengthBoosterRemainingTime <= 0)
            {
                boostersHalos[2].SetActive(false);
                strengthBoosterEnabled = false;
                strengthBoosterRemainingTime = 1;
                BoostersManager.Instance.panelItemsList[2].HandleUIOnTimerDisable();
                dungeonCharacter.ChangeCharacterDamageCoefficient(1);
            }
        }
    }

    public void EnableGoldBooster()
    {
        boostersHalos[0].SetActive(true);
        goldBoosterEnabled = true;
        MoneyHandler.Instance.ChangeBoosterCoefficient(1.3f);
    }

    public void EnableSpeedBooster()
    {
        boostersHalos[1].SetActive(true);
        speedBoosterEnabled = true;
        dungeonCharacter.ChangeCharacterRunAndAttackSpeed(1.3f, true);
    }

    public void EnableStrengthBooster()
    {
        boostersHalos[2].SetActive(true);
        strengthBoosterEnabled = true;
        dungeonCharacter.ChangeCharacterDamageCoefficient(2);
    }
}