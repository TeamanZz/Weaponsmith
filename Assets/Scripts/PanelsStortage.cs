using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsStortage : MonoBehaviour
{
    public List<GameObject> panels = new List<GameObject>();

    public GameObject mainRoom;
    [Header("Persons skin")]
    //public GameObject skinStortageObject;
    public List<GameObject> skinsInRoom = new List<GameObject>();
    public List<GameObject> dungeonSkins = new List<GameObject>();

    [Header("Workshop")]
    public List<GameObject> roomObjects = new List<GameObject>();
    public List<WorkshopItem> workshopPanelItems = new List<WorkshopItem>();

    [Header("Upgrades")]
    public List<PanelItem> panelItemsList = new List<PanelItem>();

    [Header("Dungeon")]
    public List<DungeonPanelItem> dungeonPanelItemsList = new List<DungeonPanelItem>();

    [Header("Boosters")]
    public List<BoostersItemPanel> boostersItemPanels = new List<BoostersItemPanel>();

    [Header("Transition button panel")]
    public GameObject transitionPanel;
    public bool currentEndEra = false;
}
