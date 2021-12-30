using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RewardText : MonoBehaviour
{
    private void Start()
    {
        transform.DOLocalMoveZ(-0.75f, 1f).SetEase(Ease.OutBack);
        // Destroy(gameObject, 2);
        StartCoroutine(IEFlyHigh());
    }

    private IEnumerator IEFlyHigh()
    {
        yield return new WaitForSeconds(1);
        transform.DOLocalMoveZ(-2, 0.2f).SetEase(Ease.Flash);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

    }
}
