using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class PanelsHandler : MonoBehaviour
{
    public static PanelsHandler Instance;
    public SkillController skillController;
    public LoadoutController loadoutController;

    public int dropChanceImprovements = 7;

    [Header("Panel Label")]
    [SerializeField] private TextMeshProUGUI panelNameLabel;
    [SerializeField] private GameObject panelLabelObject;

    [Header("Unlockable Buttons")]
    public Button boostersButtonComponent;
    public Image dungeonButtonImageComponent;
    public Image boostersButtonImageComponent;

    [Header("Camera's")]
    public GameObject mainCamera;
    public GameObject dungeonCamera;

    [Space]
    public GameObject dungeonPreviewUI;
    public GameObject dungeonBarsGroup;
    public GameObject dungeonEnteringPanel;
    public GameObject panelsContainer;

    public DungeonRewardPanel dungeonRewardPanel;
    public static bool currentLocationInTheDungeon = false;
    public GameObject loadoutObject;

    [Space]
    [SerializeField] private List<string> panelNames = new List<string>();
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    [SerializeField] private List<BottomButton> bottomButtons = new List<BottomButton>();

    [Header("Cool Down Settings")]
    public Button buttonToDungeon;
    public TextMeshProUGUI coolDownText;
    public List<GameObject> buttonsToDungeonGroup = new List<GameObject>();
    public Coroutine coolDownCoroutine;

    public bool timerRunning = false;

    public float reloadTime = 60;
    [SerializeField] private float currentTime;

    public GameObject anvilSound;
    public GameObject tapArea;

    public int currentIndex = 0;
    public void Awake()
    {
        Instance = this;

        currentLocationInTheDungeon = false;
        mainCamera.SetActive(true);
        dungeonCamera.SetActive(false);
        loadoutObject.SetActive(false);

        DisableDungeonPreviewUI();
        OpenPanel(1);
    }

    public void Update()
    {
        if (coolDownCoroutine == null && Input.GetKeyUp(KeyCode.Q))
            StartCoolDown();
    }

    public void Initialization(PanelsStortage stortage)
    {
        panels.Clear();
        panels.AddRange(stortage.panels);
    }

    public void HandleDungeonPanelUI()
    {
        if (DungeonManager.Instance.isDungeonStarted)
        {
            panelsContainer.SetActive(false);
            dungeonBarsGroup.SetActive(false);
        }
        else
        {
            EnablePanelsContainer();
            dungeonPreviewUI.SetActive(true);
            dungeonBarsGroup.SetActive(true);
        }
    }

    public void DisablePanelsContainer()
    {
        if (DungeonManager.Instance.isDungeonStarted)
        {
            panelsContainer.SetActive(false);
            dungeonBarsGroup.SetActive(false);
        }
    }

    public void EnablePanelsContainer()
    {
        panelsContainer.SetActive(true);
    }

    public void DisableDungeonPreviewUI()
    {
        dungeonPreviewUI.SetActive(false);
    }

    public void EnableDungeonEnteringPanel()
    {
        // Debug.Log("YSO START:" + DungeonManager.Instance.currentDungeonLevelId + 1);
        YsoCorp.GameUtils.YCManager.instance.OnGameStarted(DungeonManager.Instance.currentDungeonLevelId + 1);
        dungeonEnteringPanel.SetActive(true);
        loadoutObject.SetActive(false);
        dungeonEnteringPanel.transform.localScale = Vector3.one;
        StartCoroutine(IEEnableDungeonEnteringPanel());
    }

    public void DisableDungeonEnteringPanel()
    {
        dungeonEnteringPanel.transform.DOScale(0, 1f).SetEase(Ease.InBack);
    }

    private IEnumerator IEEnableDungeonEnteringPanel()
    {
        DungeonManager.Instance.SetCharacterOnStart();
        DisablePanelsContainer();
        yield return new WaitForSeconds(2);
        DisableDungeonEnteringPanel();
        DungeonCharacter.Instance.animator.SetBool("IsDungeonStarted", true);
        DungeonCharacter.Instance.EnableCharacterRun();
        SceneObjectsOptimizeHandler.Instance.EnableFirstPieces();
        SceneObjectsOptimizeHandler.Instance.DisablePreviewScene();
        yield return new WaitForSeconds(1);
        dungeonEnteringPanel.SetActive(false);
    }

    public GameObject mainDungeon;

    public void OpenPanel(int panelIndex)
    {
        currentIndex = Mathf.Clamp(panelIndex, 0, panels.Count - 1);

        foreach (var panel in panels)
            panel.SetActive(false);

        panels[currentIndex].SetActive(true);

        switch (currentIndex)
        {
            case 0:
                currentLocationInTheDungeon = false;
                mainCamera.SetActive(true);
                dungeonCamera.SetActive(false);
                loadoutObject.SetActive(false);

                anvilSound.SetActive(true);
                tapArea.SetActive(false);

                panelLabelObject.SetActive(true);

                dungeonPreviewUI.SetActive(false);
                break;

            case 1:
                currentLocationInTheDungeon = false;
                mainCamera.SetActive(true);
                dungeonCamera.SetActive(false);
                loadoutObject.SetActive(false);

                anvilSound.SetActive(true);
                tapArea.SetActive(true);

                panelLabelObject.SetActive(true);

                dungeonPreviewUI.SetActive(false);
                break;

            case 2:
                loadoutController.Initialization();

                currentLocationInTheDungeon = true;
                mainCamera.SetActive(false);
                dungeonCamera.SetActive(true);
                loadoutObject.SetActive(true);

                anvilSound.SetActive(false);
                tapArea.SetActive(false);

                panelLabelObject.SetActive(true);

                dungeonPreviewUI.SetActive(true);
                DungeonHubManager.dungeonHubManager.OpenPanel(1);
                break;

            case 3:
                currentLocationInTheDungeon = false;
                mainCamera.SetActive(true);
                dungeonCamera.SetActive(false);
                loadoutObject.SetActive(false);

                anvilSound.SetActive(true);
                tapArea.SetActive(true);

                panelLabelObject.SetActive(false);

                dungeonPreviewUI.SetActive(false);

                if (skillController.selectedPanel != null)
                    skillController.DeselectedAllPanelsBorder();
                break;
        }
    }

    [ContextMenu("Cool Down")]
    public void StartCoolDown()
    {
        coolDownCoroutine = StartCoroutine(CoolDown());
    }

    public IEnumerator CoolDown()
    {
        Debug.Log("Start Cool Down");

        currentTime = reloadTime;
        buttonToDungeon.interactable = false;
        timerRunning = false;
        buttonsToDungeonGroup[0].SetActive(false);
        buttonsToDungeonGroup[1].SetActive(true);

        coolDownText.text = currentTime.ToString("F2");

        while (!timerRunning)
        {
            yield return new WaitForSeconds(1f);
            currentTime -= 1;
            float seconds = currentTime % 60;
            string secondsText;
            if (seconds < 10)
                secondsText = 0 + seconds.ToString();
            else
                secondsText = seconds.ToString();

            coolDownText.text = (int)(currentTime / 60) + ":" + secondsText;

            if (currentTime <= 0)
                timerRunning = true;
        }

        coolDownCoroutine = null;
        buttonToDungeon.interactable = true;

        buttonsToDungeonGroup[0].SetActive(true);
        buttonsToDungeonGroup[1].SetActive(false);
    }

    public void ResetButtonColorOnDeselect()
    {
        for (int i = 0; i < bottomButtons.Count; i++)
        {
            bottomButtons[i].ResetColor();
        }
    }

    public void ShowPanelName(int panelIndex)
    {
        panelNameLabel.text = panelNames[panelIndex];
    }
}