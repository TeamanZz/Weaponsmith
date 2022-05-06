using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DungeonPopupText : MonoBehaviour
{
    [SerializeField] TextMeshPro text;
    public void InitializeRewardText(string value)
    {
        text.text = $"+ {value}$";
        transform.DOLocalMoveY(transform.position.y + 10, 1.5f);
        text.DOFade(0, 1.5f);
        Destroy(gameObject, 1.5f);
    }

    public void InitializeCharacterDamageText(string value)
    {
        text.text = $"{value}";
        text.color = Color.red;
        transform.localScale = Vector3.one / 2;
        transform.DOLocalMoveY(transform.position.y + 2, 1f);
        text.DOFade(0, 1f);
        Destroy(gameObject, 1.5f);
    }

    public void InitializeEnemyDamageText(string value)
    {
        text.text = $"{value}";
        text.color = Color.white;
        transform.localScale = Vector3.one / 2;
        transform.DOLocalMoveY(transform.position.y + 2, 1f);
        text.DOFade(0, 1f);
        Destroy(gameObject, 1.5f);
    }
}