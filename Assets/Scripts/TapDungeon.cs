using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapDungeon : MonoBehaviour, IPointerClickHandler
{
    public GameObject tapCirclePrefab;
    public Transform canvasTransform;
    // public Camera dungeonCamera;
    public void OnPointerClick(PointerEventData eventData)
    {
        MoneyHandler.Instance.IncreaseIncomeByTap();
        SpawnTapCircle();
    }

    private void SpawnTapCircle()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform as RectTransform, Input.mousePosition, canvasTransform.GetComponent<Canvas>().worldCamera, out pos);
        var neededPos = canvasTransform.TransformPoint(pos);
        var ob = Instantiate(tapCirclePrefab, neededPos, Quaternion.identity);
        ob.transform.SetParent(canvasTransform);
    }
}
