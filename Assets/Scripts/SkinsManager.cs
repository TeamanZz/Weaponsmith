using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    public static SkinsManager Instance;

    public int currentSkinIndex;
    [HideInInspector] public int dungeonEnemySkinCount = 0;

    public List<ParticleSystem> particles = new List<ParticleSystem>();
    public List<GameObject> skins = new List<GameObject>();
    public List<GameObject> dungeonSkins = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        currentSkinIndex = PlayerPrefs.GetInt("skinIndex");
        dungeonEnemySkinCount = PlayerPrefs.GetInt("EnemySkinCount");
    }

    public void Initialization(List<GameObject> newSkins, List<GameObject> newDungeonSkins)
    {
        //skins.Clear();
        //dungeonSkins.Clear();

        //skins = newSkins;
        //dungeonSkins = newDungeonSkins;

        //ChangeSkin(needShowParticles: false);
    }

    [ContextMenu("Change Skin")]
    public void ChangeSkin(bool needShowParticles = true)
    {
        //foreach (GameObject dungeSkin in dungeonSkins)
        //{
        //    dungeSkin.SetActive(false);
        //}

        //foreach (GameObject skin in skins)
        //{
        //    skin.SetActive(false);
        //}
        //if (needShowParticles)
        //{
        //    foreach (var part in particles)
        //    {
        //        part.Play();
        //    }
        //}

        //skins[0/*currentNumber*/].SetActive(true);
        //dungeonSkins[currentSkinIndex].SetActive(true);
    }

    public void ChangeSkin(int skinIndex)
    {
        //foreach (GameObject dungeSkin in dungeonSkins)
        //{
        //    dungeSkin.SetActive(false);
        //}

        //foreach (GameObject skin in skins)
        //{
        //    skin.SetActive(false);
        //}

        //foreach (var part in particles)
        //{
        //    part.Play();
        //}

        //currentSkinIndex = skinIndex;
        //skins[currentSkinIndex].SetActive(true);
        //dungeonSkins[currentSkinIndex].SetActive(true);
    }
}