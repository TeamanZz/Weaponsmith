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
    [HideInInspector] public float goldBoosterRemainingTimeCooldown = 1;
    public float goldBoosterTotalTime;
    public float goldBoosterTotalTimeCooldown;

    [HideInInspector] public float speedBoosterRemainingTime = 1;
    [HideInInspector] public float speedBoosterRemainingTimeCooldown = 1;
    public float speedBoosterTotalTime;
    public float speedBoosterTotalTimeCooldown;

    [HideInInspector] public float strengthBoosterRemainingTime = 1;
    [HideInInspector] public float strengthBoosterRemainingTimeCooldown = 1;
    public float strengthBoosterTotalTime;
    public float strengthBoosterTotalTimeCooldown;

    public DungeonCharacter dungeonCharacter;

    private void Awake()
    {
        Instance = this;

        goldBoosterRemainingTime = 1;
        goldBoosterRemainingTimeCooldown = 1;

        speedBoosterRemainingTime = 1;
        speedBoosterRemainingTimeCooldown = 1;

        strengthBoosterRemainingTime = 1;
        strengthBoosterRemainingTimeCooldown = 1;
    }

    private void Update()
    {
        if (goldBoosterEnabled)
        {
            goldBoosterRemainingTime -= Time.deltaTime / goldBoosterTotalTime;
            goldBoosterRemainingTimeCooldown -= Time.deltaTime / goldBoosterTotalTimeCooldown;

            if (goldBoosterRemainingTimeCooldown <= 0)
            {
                boostersHalos[0].SetActive(false);
                boostersHalos[3].SetActive(false);
                MoneyHandler.Instance.ChangeBoosterCoefficient(1f);
            }

            if (goldBoosterRemainingTime <= 0)
            {
                goldBoosterEnabled = false;
                goldBoosterRemainingTime = 1;
                BoostersPanelItemsManager.Instance.panelItemsList[0].HandleUIOnTimerDisable();
            }
        }

        if (speedBoosterEnabled)
        {
            speedBoosterRemainingTime -= Time.deltaTime / speedBoosterTotalTime;
            speedBoosterRemainingTimeCooldown -= Time.deltaTime / speedBoosterTotalTimeCooldown;

            if (speedBoosterRemainingTimeCooldown <= 0)
            {
                boostersHalos[1].SetActive(false);
                boostersHalos[4].SetActive(false);
                dungeonCharacter.ChangeCharacterRunAndAttackSpeed(1f, false);
            }

            if (speedBoosterRemainingTime <= 0)
            {
                speedBoosterEnabled = false;
                speedBoosterRemainingTime = 1;
                BoostersPanelItemsManager.Instance.panelItemsList[1].HandleUIOnTimerDisable();
            }
        }

        if (strengthBoosterEnabled)
        {
            strengthBoosterRemainingTime -= Time.deltaTime / strengthBoosterTotalTime;
            strengthBoosterRemainingTimeCooldown -= Time.deltaTime / strengthBoosterTotalTimeCooldown;

            if (strengthBoosterRemainingTimeCooldown <= 0)
            {
                boostersHalos[2].SetActive(false);
                boostersHalos[5].SetActive(false);
                dungeonCharacter.ChangeCharacterDamageCoefficient(1);
            }

            if (strengthBoosterRemainingTime <= 0)
            {
                strengthBoosterEnabled = false;
                strengthBoosterRemainingTime = 1;
                BoostersPanelItemsManager.Instance.panelItemsList[2].HandleUIOnTimerDisable();
            }
        }
    }

    public void EnableGoldBooster()
    {
        goldBoosterRemainingTimeCooldown = 1;
        boostersHalos[0].SetActive(true);
        boostersHalos[3].SetActive(true);
        goldBoosterEnabled = true;
        MoneyHandler.Instance.ChangeBoosterCoefficient(1.3f);
    }

    public void EnableSpeedBooster()
    {
        speedBoosterRemainingTimeCooldown = 1;
        boostersHalos[1].SetActive(true);
        boostersHalos[4].SetActive(true);
        speedBoosterEnabled = true;
        dungeonCharacter.ChangeCharacterRunAndAttackSpeed(1.3f, true);
    }

    public void EnableStrengthBooster()
    {
        strengthBoosterRemainingTimeCooldown = 1;
        boostersHalos[2].SetActive(true);
        boostersHalos[5].SetActive(true);
        strengthBoosterEnabled = true;
        dungeonCharacter.ChangeCharacterDamageCoefficient(2);
    }
}