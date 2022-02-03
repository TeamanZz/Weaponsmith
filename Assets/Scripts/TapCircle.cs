using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TapCircle : MonoBehaviour
{
    [SerializeField] private float circleLifeTime = 0.3f;

    void Start()
    {
        transform.DOScale(new Vector3(25, 25, 25), circleLifeTime);
        Destroy(gameObject, circleLifeTime);
        GetComponent<Image>().DOFade(0f, circleLifeTime);
    }
}