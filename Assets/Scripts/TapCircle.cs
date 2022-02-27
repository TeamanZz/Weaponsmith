using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TapCircle : MonoBehaviour
{
    [SerializeField] private float circleLifeTime = 0.3f;
    private Image image;

    private Tween scaleTween;
    private Tween fadeTween;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        scaleTween = transform.DOScale(new Vector3(25, 25, 25), circleLifeTime);
        fadeTween = image.DOFade(0f, circleLifeTime);
        StartCoroutine(IEDestroyCircle());
    }

    private IEnumerator IEDestroyCircle()
    {
        yield return new WaitForSeconds(circleLifeTime);
        scaleTween.Complete();
        fadeTween.Complete();
        Destroy(gameObject);
    }
}