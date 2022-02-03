using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponUnlockPanel : MonoBehaviour
{
    public Image weaponImage;
    public TextMeshProUGUI title;

    public void InitializationPanel(Image weaponPicture, string weaponTitle)
    {
        title.text = weaponTitle;
        weaponImage.sprite = weaponImage.sprite;
    }
}
