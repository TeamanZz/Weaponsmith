using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraController : MonoBehaviour
{
    public static EraController Instance;

    public PanelsHandler panelsHandler;
    public WorkshopPanelItemsManager roomObjectsHandler;
    public CraftPanelItemsManager itemsManager;
    public DungeonPanelItemsManager dungeonItemManager;
    public BoostersPanelItemsManager boostersManager;
    public SkinsManager skinsManager;

    //  rooms
    public SceneObjectsOptimizeHandler sceneObjectsOptimize;

    [Header("Era settings")]
    public int currentEraNumber;
    public List<GameObject> panelsStortage = new List<GameObject>();
    public List<GameObject> rooms = new List<GameObject>();

    public List<PanelsStortage> stortages = new List<PanelsStortage>();

    public List<GameObject> skinsRoomStortage = new List<GameObject>();
    public List<GameObject> skinsDungeonStortage = new List<GameObject>();

    public void Awake()
    {
        Instance = this;

        currentEraNumber = PlayerPrefs.GetInt("currentEraNumber");
        // Debug.Log(currentEraNumber);

        //  objects
        for (int i = 0; i < panelsStortage.Count; i++)
        {
            panelsStortage[i].SetActive(false);
            rooms[i].SetActive(false);
        }

        panelsStortage[currentEraNumber].SetActive(true);
        rooms[currentEraNumber].SetActive(true);

        //  skins
        for (int i = 0; i < skinsRoomStortage.Count; i++)
        {
            skinsRoomStortage[i].SetActive(false);
        }
        skinsRoomStortage[currentEraNumber].SetActive(true);

        //  skins in dungeon
        for (int i = 0; i < skinsDungeonStortage.Count; i++)
        {
            skinsDungeonStortage[i].SetActive(false);
        }
        skinsDungeonStortage[currentEraNumber].SetActive(true);

        //  ui
        for (int i = 0; i < stortages[currentEraNumber].panels.Count; i++)
        {
            stortages[currentEraNumber].panels[i].SetActive(true);
        }

        stortages[currentEraNumber].panels[1].SetActive(true);
        InitializationAllPanel();
    }

    public void InitializationAllPanel()
    {
        //  room
        sceneObjectsOptimize.Initialization(stortages[currentEraNumber].mainRoom);
        //  panels
        panelsHandler.Initialization(stortages[currentEraNumber]);
        //  room objects
        roomObjectsHandler.Initialization(stortages[currentEraNumber].roomObjects, stortages[currentEraNumber].workshopPanelItems, stortages[currentEraNumber].transitionPanel, stortages[currentEraNumber].currentEndEra);
        //  uprage
        itemsManager.Initialization(stortages[currentEraNumber].panelItemsList);
        //  dungeon
        dungeonItemManager.Initialization(stortages[currentEraNumber].dungeonPanelItemsList);
        //  boosters
        boostersManager.Initialization(stortages[currentEraNumber].boostersItemPanels);
        //  open panel
        panelsHandler.OpenPanel(1);
        //  skin stortage
        //stortages[currentEraNumber].skinStortageObject.SetActive(true);
        skinsManager.Initialization(stortages[currentEraNumber].skinsInRoom, stortages[currentEraNumber].dungeonSkins);

        // PlayerPrefs.SetInt("currentEraNumber", currentEraNumber);
        if (SettingsManager.settingsManager != null)
            SettingsManager.settingsManager.RemovingOldEraProgress();
    }

    [ContextMenu("Transition")]
    public void TransitionToNewEra()
    {
        if (currentEraNumber < panelsStortage.Count - 1)
            currentEraNumber += 1;

        //  objects
        for (int i = 0; i < panelsStortage.Count; i++)
        {
            panelsStortage[i].SetActive(false);
            rooms[i].SetActive(false);
        }

        panelsStortage[currentEraNumber].SetActive(true);
        rooms[currentEraNumber].SetActive(true);

        //  skins in room
        for (int i = 0; i < skinsRoomStortage.Count; i++)
        {
            skinsRoomStortage[i].SetActive(false);
        }
        skinsRoomStortage[currentEraNumber].SetActive(true);

        //  skins in dungeon
        for (int i = 0; i < skinsDungeonStortage.Count; i++)
        {
            skinsDungeonStortage[i].SetActive(false);
        }
        skinsDungeonStortage[currentEraNumber].SetActive(true);

        //  ui
        for (int i = 0; i < stortages[currentEraNumber].panels.Count; i++)
        {
            stortages[currentEraNumber].panels[i].SetActive(true);
        }

        stortages[currentEraNumber].panels[1].SetActive(true);
        InitializationAllPanel();
    }
}
