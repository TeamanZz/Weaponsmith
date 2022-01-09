using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;

    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void DeleteAllProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("needLaunch", "true");
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}