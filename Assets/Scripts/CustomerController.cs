using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerController : MonoBehaviour, IPointerClickHandler
{
    public static CustomerController Instance;

    public Animator swordAnvilAnimator;
    public GameObject tapCirclePrefab;
    public Transform canvasTransform;
    public ParticleSystem particles;
    public GameObject hammer;

    private void Awake()
    {
        Instance = this;
    }

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

    public void PlayAnvilParticles()
    {
        particles.Play();
        swordAnvilAnimator.Play("Jump Scale2", 0, 0);
    }
}