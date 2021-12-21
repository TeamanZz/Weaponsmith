using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ViewItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public LayerMask setLayer;
    public LayerMask tableMask = 6;

    public bool objectDelivered;

    [Header("View settings")]
    public Transform prefabPoint;
    public Item item;
    public GameObject itemPrefab;

    public GameObject viewObject;
    [Header("UI settings")]
    public Vector3 prefabScale;
    public Vector3 prefabRotation;

    [Header("World settings")]
    public Vector3 prefabInWorldScale;
    public Vector3 prefabWorldRotation;
    public Color objectColor;

    [Header("drag settings")]
    private GameObject copyMainPanel;
    private Image cloneImage;
    //private RectTransform rectTransform;

    //public GameObject cloneViewObect;
    MousePosition mouse;

    public void Initialization(Item newItem)
    {
        item = newItem;
        itemPrefab = newItem.itemPrefab;

        prefabScale = newItem.prefabScale;
        prefabRotation = newItem.prefabRotation;

        prefabInWorldScale = newItem.prefabInWorldScale;
        prefabWorldRotation = newItem.prefabWorldRotation;

        viewObject = Instantiate(newItem.itemPrefab, prefabPoint); 
        
        viewObject.transform.localScale = prefabScale;
        viewObject.transform.localRotation = Quaternion.Euler(prefabRotation);
        setLayer = newItem.setLayer;
    }

    public void DestroyItem()
    {
        Destroy(this);
    }

    private Vector3 screenPoint;
    private Vector3 offset;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");

        //copyMainPanel = Instantiate(gameObject, transform);
        //copyMainPanel.transform.localPosition = transform.position;

        //copyMainPanel.transform.parent = GameManager.mainCanvas.transform;

        //cloneImage = copyMainPanel.GetComponent<Image>();

        //rectTransform = cloneImage.rectTransform;
        //rectTransform.sizeDelta = new Vector2(300f, 300f);

        ////copyMainPanel.transform.localPosition = transform.position;
        //Debug.Log("Start drag");

        //var script = copyMainPanel.GetComponent<ViewItem>();
        //cloneViewObect = script.viewObject;
        //Destroy(script); 

        LayerMask mask = 5;
        copyMainPanel = Instantiate(itemPrefab);
        copyMainPanel.AddComponent<MousePosition>();
        copyMainPanel.layer = mask;

        copyMainPanel.transform.localScale = prefabInWorldScale;
        copyMainPanel.transform.rotation = Quaternion.Euler(prefabWorldRotation);

        mouse = copyMainPanel.GetComponent<MousePosition>();
        mouse.viewItem = this;
        mouse.delivered = false;
        mouse.item = item;
        mouse.thisObject = copyMainPanel;

        mouse.tableMask = tableMask;
        mouse.setConnectLayer = setLayer;
        objectColor = mouse.renderer.material.color;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ////int uiMask = 5;

        ////Vector3 mouse = Input.mousePosition;
        ////Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        ////Ray ray = GameManager.mainCamera.ScreenPointToRay(Input.mousePosition);

        ////if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, uiMask))
        ////{
        ////    Debug.Log("UiLayer");
        ////    copyMainPanel.transform.parent = GameManager.mainCanvas.transform;
        ////    copyMainPanel.transform.localScale = new Vector3(1f, 1f, 1f);
        ////    cloneViewObect.transform.localScale = prefabScale;

        ////    rectTransform.anchoredPosition += eventData.delta / GameManager.mainCanvas.scaleFactor;
        ////    cloneImage.color = Color.green;
        ////    objectDelivered = false;
        ////}
        ////else
        ////{
        ////    Debug.Log("Default");
        ////    copyMainPanel.transform.parent = null;
        ////    copyMainPanel.transform.localScale = new Vector3(1f, 1f, 1f);
        ////    cloneViewObect.transform.localScale = prefabInWorldScale;
        ////    copyMainPanel.transform.position = hit.point;

        ////    Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        ////    Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
        ////    copyMainPanel.transform.position = cursorPosition;

        ////    rectTransform.anchoredPosition += eventData.delta / GameManager.mainCanvas.scaleFactor;
        ////    cloneImage.color = Color.blue;
        ////    objectDelivered = true;
        ////    Debug.Log(hit.point);
        ////}

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // if(objectDelivered == false)
        //Destroy(copyMainPanel);
        Debug.Log("end drag");
        Debug.Log(mouse.goodPoint);

        if (mouse.goodPoint == true)
        {
            mouse.InitializationChild();

            if (mouse.transform.parent != null)
            {
                if (mouse.transform.parent.childCount > 1)
                {
                    Debug.Log(mouse.transform.parent.childCount);
                    GameObject removingObject = null;

                    Debug.Log(mouse.transform.parent.GetChild(0).gameObject + "  Mouse parent obj");
                    removingObject = mouse.transform.parent.GetChild(0).gameObject;
                    removingObject.transform.parent = mouse.transform;

                    if (removingObject.GetComponent<MousePosition>())
                    {
                        MousePosition remoovingObjectScript;
                        remoovingObjectScript = removingObject.GetComponent<MousePosition>();

                        if (remoovingObjectScript.tipChildren != null)
                        {
                            remoovingObjectScript.tipChildren.RemoveParent(mouse);
                            Debug.Log(remoovingObjectScript.tipChildren + " new");
                        }
                        else
                            Debug.Log("Removing object children = null");
                    }

                    //Debug.Log(removingObject + "   Removing object");
                    Destroy(removingObject);
                }
            }

            GameManager.gameManager.newObjects.Add(mouse.gameObject);
        }
        else
        {
            Destroy(mouse.gameObject);
            //  если не поставили - удаляем
        }
        //  UI control

        mouse.renderer.material.color = objectColor;
        mouse.mainColor = objectColor;

        mouse.delivered = true;
    }

}
