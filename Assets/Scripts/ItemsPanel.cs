using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsPanel : MonoBehaviour
{
    public static ItemsPanel itemsPanel;

    [Space(5f)]
    [Header("Data settings")]
    public List<Item> bodyItems = new List<Item>();
    public List<Item> tipItems = new List<Item>();
    public List<Item> accessoryItems = new List<Item>();

    [HideInInspector] public TypesOfPanels setTypesOfPanel;
    public enum TypesOfPanels
    {
        body,
        tip,
        accessory
    }

    [Space(5f)]
    [Header("View settings")]
    public ScrollRect scrollRect;
    [Tooltip("����� � ������� ������� ������ ��������, � ������� ����")]
    public GridLayoutGroup[] group;
    [Tooltip("����� ��������� image")]
    public RectTransform[] viewGrupGameObject;
    public Image[] buttonBackground;

    public List<ViewItem> viewingBodyItems = new List<ViewItem>();
    public List<ViewItem> viewingTipItems = new List<ViewItem>();
    public List<ViewItem> viewingAccessoryItems = new List<ViewItem>();

    public GameObject emptyView;


    public void Awake()
    {
        itemsPanel = this;

        viewGrupGameObject[0].sizeDelta = new Vector2(0, viewGrupGameObject[0].sizeDelta.y);
        viewGrupGameObject[1].sizeDelta = new Vector2(0, viewGrupGameObject[1].sizeDelta.y);
        viewGrupGameObject[2].sizeDelta = new Vector2(0, viewGrupGameObject[2].sizeDelta.y);

        foreach (var item in bodyItems)
        {
            CreatingNewPanels(item);
        }

        foreach (var item in tipItems)
        {
            CreatingNewPanels(item);
        }

        foreach (var item in accessoryItems)
        {
            CreatingNewPanels(item);
        }
        FirstDiscovery();
    }

    public void FirstDiscovery()
    {
        setTypesOfPanel = TypesOfPanels.body;
        ShowItems(setTypesOfPanel);
    }
    //  opening a window for the first time
    //  every time this window is opened we use the function

    public void LevelChange(int number)
    {
        TypesOfPanels types = new TypesOfPanels();

        switch (number)
        {
            case 0:
                types = TypesOfPanels.body;
                break;

            case 1:
                types = TypesOfPanels.tip;
                break;

            case 2:
                types = TypesOfPanels.accessory;
                break;
        }

        if (types == setTypesOfPanel)
            return;
        //  open the current window

        switch (types)
        {
            case TypesOfPanels.body:
                scrollRect.content = viewGrupGameObject[0];
                scrollRect.content.anchoredPosition = new Vector3(0, 0, 0);

                setTypesOfPanel = types;
                ShowItems(types);
                break;

            case TypesOfPanels.tip:
                scrollRect.content = viewGrupGameObject[1];
                scrollRect.content.anchoredPosition = new Vector3(0, 0, 0);

                setTypesOfPanel = types;
                ShowItems(types);
                break;

            case TypesOfPanels.accessory:
                scrollRect.content = viewGrupGameObject[2];
                scrollRect.content.anchoredPosition = new Vector3(0, 0, 0);

                setTypesOfPanel = types;
                ShowItems(types);
                break;
        }

        //Debug.Log(setTypesOfPanel);
    }

    public void CreatingNewPanels(Item newItem)
    {
        Transform newItemParant = null;
        switch (newItem.setTypes)
        {
            case TypesOfPanels.body:
                newItemParant = viewGrupGameObject[0].transform;
                viewGrupGameObject[0].sizeDelta = new Vector2(viewGrupGameObject[0].sizeDelta.x + 360, viewGrupGameObject[0].sizeDelta.y);
                break;

            case TypesOfPanels.tip:
                newItemParant = viewGrupGameObject[1].transform;
                viewGrupGameObject[1].sizeDelta = new Vector2(viewGrupGameObject[1].sizeDelta.x + 360, viewGrupGameObject[1].sizeDelta.y);
                break;

            case TypesOfPanels.accessory:
                newItemParant = viewGrupGameObject[2].transform;
                viewGrupGameObject[2].sizeDelta = new Vector2(viewGrupGameObject[2].sizeDelta.x + 360, viewGrupGameObject[2].sizeDelta.y);
                break;
        }

        GameObject newPanel = Instantiate(emptyView, newItemParant);
        newPanel.transform.parent = newItemParant;

        newPanel.GetComponent<ViewItem>().Initialization(newItem);

    }

    public void ShowItems(TypesOfPanels types)
    {
        viewGrupGameObject[0].gameObject.SetActive(false);
        viewGrupGameObject[1].gameObject.SetActive(false);
        viewGrupGameObject[2].gameObject.SetActive(false);


        buttonBackground[0].color = new Vector4(1f, 1f, 1f, 1f);
        buttonBackground[1].color = new Vector4(1f, 1f, 1f, 1f);
        buttonBackground[2].color = new Vector4(1f, 1f, 1f, 1f);

        switch (types)
        {
            case TypesOfPanels.body:
                viewGrupGameObject[0].gameObject.SetActive(true);
                buttonBackground[0].color = new Vector4(1f, 1f, 1f, 0.75f);
                break;

            case TypesOfPanels.tip:
                viewGrupGameObject[1].gameObject.SetActive(true);
                buttonBackground[1].color = new Vector4(1f, 1f, 1f, 0.75f);
                break;

            case TypesOfPanels.accessory:
                viewGrupGameObject[2].gameObject.SetActive(true);
                buttonBackground[2].color = new Vector4(1f, 1f, 1f, 0.75f);
                break;
        }
    }
}
