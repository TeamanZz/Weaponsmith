using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class Item 
{
    public ItemsPanel.TypesOfPanels setTypes;
    public LayerMask setLayer;
    public GameObject itemPrefab;

    [Header("UI settings")]
    public Vector3 prefabScale;
    public Vector3 prefabRotation;

    [Header("World settings")]
    public Vector3 prefabInWorldScale;
    public Vector3 prefabWorldRotation;
}
