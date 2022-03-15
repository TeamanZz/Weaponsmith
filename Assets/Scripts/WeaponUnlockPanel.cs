using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponUnlockPanel : MonoBehaviour
{
    public TextMeshProUGUI title;

    public void InitializePanel(Image weaponPicture, string weaponTitle)
    {
        title.text = weaponTitle;
    }
}
