using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MousePosition : MonoBehaviour
{
    public LayerMask tipLayer;
    public Item item;
    public Transform tipConnectPoint;

    public LayerMask tableMask;
    public LayerMask setConnectLayer;

    public bool delivered = true;
    public bool goodPoint = false;

    public ViewItem viewItem;
    public Renderer renderer;
    public Color mainColor;
    public Color tipColor;

    public float timeToMove = 1f;
    public float destroyTime = 1f;
    public float setTime;
    public bool move = false;
    public bool remove = false;

    public GameObject thisObject;
    public MousePosition tipChildren;
    public MousePosition accesuarChildren;

    public Transform thisParent;

    public Vector3 lastPosition;

    public void Awake()
    {
        if (GetComponent<Renderer>() == false)
            renderer = GetComponentInChildren<Renderer>();
        else
            renderer = GetComponent<Renderer>();

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
            thisParent = hit.collider.transform.parent;

            goodPoint = true;
            renderer.material.color = Color.green;
        }
    }

    public void InitializationChild()
    {
        if (thisParent.GetComponent<MousePosition>() != null)
        {
            MousePosition parentObject;
            parentObject = thisParent.GetComponent<MousePosition>();
            Debug.Log(parentObject.gameObject + "------ Collider");

            LayerMask tipLayer = 1 << LayerMask.NameToLayer("Tip");

            if (setConnectLayer == tipLayer)
            {
                Debug.Log("tip");
                parentObject.tipChildren = this;
                Debug.Log(parentObject.tipChildren);
            }
        }
    }

    public void RemoveParent(MousePosition newMouse)
    {
        Debug.Log(newMouse.gameObject + "  new pos and script");
        //transform.parent = newMouse.transform;

        Debug.Log(thisParent.GetComponent<MousePosition>().gameObject);
        newMouse.tipChildren = thisParent.GetComponent<MousePosition>().tipChildren;
        newMouse.tipChildren.thisParent = newMouse.gameObject.transform;

        if (newMouse.transform.GetComponentInChildren<SphereCollider>() != null)
        {
            //Debug.Log("Find tip point");
            Transform newMouseTransform;
            newMouseTransform = newMouse.transform.GetComponentInChildren<SphereCollider>().transform;
            transform.parent = newMouseTransform.transform;
            transform.position = newMouseTransform.position;
        }
    }

    public void RecordTheResult()
    {
        LayerMask bodyLayer = 1 << LayerMask.NameToLayer("Body");
        LayerMask tipLayer = 1 << LayerMask.NameToLayer("Tip");

        Debug.Log("Recording result layer --" + bodyLayer);
        Debug.Log("Recording result layer --" + tipLayer);

        if (item.setLayer == bodyLayer)
        {
            GameManager.gameManager.resultBody = item;
            Debug.Log(item.itemPrefab + " -- body result");
        }

        if (item.setLayer == tipLayer)
        {
            GameManager.gameManager.resultTip = item;
            Debug.Log(item.itemPrefab + " -- body result");
        }
    }

    public void OnMouseDown()
    {
        if (delivered == false)
            return;

        lastPosition = transform.position;

        Debug.Log("PrefDrag");

        setTime = timeToMove;
        move = true;
        remove = false;

        RecordTheResult();
        if (tipChildren != null)
            tipChildren.RecordTheResult();

    }

    public void OnMouseDrag()
    {
        if (delivered == false)
            return;

        RaycastHit hit;
        Ray ray = GameManager.mainCamera.ScreenPointToRay(Input.mousePosition);

        //if (thisParent.gameObject == GameManager.mainTable)
        //    Debug.Log("This object main");

        if (move == true && remove == false)
        {
            float distance = Vector3.Distance(transform.position, lastPosition);
            //Debug.Log(distance + " - distance" + " | " + move + " - move" + " | " + remove + " - remove");

            if (distance < 0.75f)
            {
                if (setTime > 0)
                {
                    if (Physics.Raycast(ray, out hit, float.MaxValue, tableMask) && thisParent.gameObject == GameManager.mainTable)
                    {
                        transform.position = hit.point;
                    }
                }
                else
                {
                    move = false;
                    remove = true;
                    setTime = destroyTime;
                }

                setTime -= Time.deltaTime;
            }
            else
            {
                float distanceToBox = Vector3.Distance(transform.position, GameManager.gameManager.exportPoint.position);

                if (Physics.Raycast(ray, out hit, float.MaxValue, tableMask) && thisParent.gameObject == GameManager.mainTable)
                {
                    transform.position = hit.point;
                }

                transform.localScale = new Vector3(Mathf.Clamp(distanceToBox / 2, 0.25f, 1f), Mathf.Clamp(distanceToBox / 2, 0.25f, 1f), Mathf.Clamp(distanceToBox / 2, 0.25f, 1f));

                if (distanceToBox < 0.5f)
                {
                    RecordTheResult();
                    if (tipChildren != null)
                        tipChildren.RecordTheResult();

                    GameManager.gameManager.ResultCalculation();
                }
                //setTime -= destroyTime;
            }
        }

        if (move == false && remove == true)
        {
            if (setTime > 0)
            {
                renderer.material.color = Color.red;
                if (tipChildren != null)
                {
                    tipChildren.OnMouseDrag();
                    tipChildren.renderer.material.color = Color.red;

                    //  ������� ���
                }

            }
            else
            {
                ClearResult();
                if (tipChildren != null)
                    tipChildren.ClearResult();

                Destroy(gameObject);
            }

            setTime -= Time.deltaTime;
        }

    }

    public void ClearResult()
    {
        if (item.setLayer == LayerMask.NameToLayer("Body"))
            GameManager.gameManager.resultBody = null;

        if (item.setLayer == LayerMask.NameToLayer("Tip"))
            GameManager.gameManager.resultTip = null;
    }

    // public void OnMouseUp()
    // {
    //     if (delivered == false)
    //         return;

    //     if (thisParent.gameObject == GameManager.mainTable)
    //     {
    //         Debug.Log("Main object return");
    //         transform.position = GameManager.gameManager.craftPoint.position;
    //     }
    //     move = false;
    //     remove = false;

    //     transform.localScale = new Vector3(1f, 1f, 1f);
    //     setTime = 99;
    //     renderer.material.color = mainColor;

    //     if (tipChildren != null)
    //     {
    //         //tipColor = tipChildren.mainColor;
    //         tipChildren.OnMouseUp();
    //         //  ������� ���
    //     }
    // }
}