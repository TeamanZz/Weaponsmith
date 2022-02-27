using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CurrencyPopup : MonoBehaviour
{
    public TextMeshProUGUI currencyText;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.Play("CurrencyFlying" + Random.Range(1, 4));
        Destroy(gameObject, 8);
    }
}