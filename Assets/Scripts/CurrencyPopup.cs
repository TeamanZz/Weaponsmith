using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyPopup : MonoBehaviour
{
    public TextMeshProUGUI currencyText;

    private void Start()
    {
        Destroy(gameObject, 10);
    }
}