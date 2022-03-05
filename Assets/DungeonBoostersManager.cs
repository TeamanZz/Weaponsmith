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

    public List<SpellButton> boosters = new List<SpellButton>();

    private void Awake()
    {
        Instance = this;
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
                boosters[0].HandleUIOnTimerDisable();
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
                boosters[1].HandleUIOnTimerDisable();
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
                boosters[2].HandleUIOnTimerDisable();
            }
        }
    }

    public void EnableGoldBooster()
    {
        boostersHalos[0].SetActive(true);
        goldBoosterEnabled = true;
    }

    public void EnableSpeedBooster()
    {
        boostersHalos[1].SetActive(true);
        speedBoosterEnabled = true;
    }

    public void EnableStrengthBooster()
    {
        boostersHalos[2].SetActive(true);
        strengthBoosterEnabled = true;
    }
}