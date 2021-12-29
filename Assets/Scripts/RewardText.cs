using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RewardText : MonoBehaviour
{
    private void Start()
    {
        transform.DOLocalMoveZ(-2, 1f).SetEase(Ease.InFlash);
        Destroy(gameObject, 2);
    }
}
