using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RewardText : MonoBehaviour
{

    private void Start()
    {
        transform.DOLocalMoveZ(-2, 2);
        Destroy(gameObject, 2);
    }
}
