using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MousePosition : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;

    public LayerMask tableMask;
    public LayerMask setConnectLayer;

    public bool delivered = true;
    public bool goodPoint = false;

    public ViewItem viewItem;
    public Renderer renderer;
    public Color mainColor;

    public float destroyTime = 5f;
    public float setTime;

    public void Awake()
    {
        if (GetComponent<Renderer>() == false)
            renderer = GetComponentInChildren<Renderer>();
        else
            renderer = GetComponent<Renderer>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //if (delivered == false)
        //    return;

        Debug.Log("PrefDrag");

        setTime = destroyTime;
        renderer.material.color = Color.red;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (delivered == false)
            return;

        setTime -= Time.deltaTime;
        if (setTime <= 0)
            Destroy(gameObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (delivered == false)
            return;

        setTime = destroyTime;
        renderer.material.color = mainColor;
    }

    public void Update()
    {
        if (delivered == true)
            return;

        RaycastHit hit;
        Ray ray = GameManager.mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, float.MaxValue, tableMask))
        {
            transform.position = hit.point;
            goodPoint = false;
            renderer.material.color = Color.red;
        }

        if (Physics.Raycast(ray, out hit, float.MaxValue, setConnectLayer))
        {
            transform.position = hit.collider.transform.position;
            transform.parent = hit.collider.transform;

            goodPoint = true;
            renderer.material.color = Color.green;
        }
    }
    
}
