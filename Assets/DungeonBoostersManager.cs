using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBoostersManager : MonoBehaviour
{
    public List<GameObject> boostersHalos = new List<GameObject>();
    public void EnableGoldBooster(int time)
    {
        StartCoroutine(IEStartGoldBooster(time));
    }

    public void EnableSpeedBooster(int time)
    {
        StartCoroutine(IEStartSpeedBooster(time));
    }

    public void EnableStrengthBooster(int time)
    {
        StartCoroutine(IEStartStrengthBooster(time));
    }

    private IEnumerator IEStartGoldBooster(int time)
    {
        boostersHalos[0].SetActive(true);
        yield return new WaitForSeconds(time);
        boostersHalos[0].SetActive(false);
    }

    private IEnumerator IEStartSpeedBooster(int time)
    {
        boostersHalos[1].SetActive(true);
        yield return new WaitForSeconds(time);
        boostersHalos[1].SetActive(false);
    }

    private IEnumerator IEStartStrengthBooster(int time)
    {
        boostersHalos[2].SetActive(true);
        yield return new WaitForSeconds(time);
        boostersHalos[2].SetActive(false);
    }
}