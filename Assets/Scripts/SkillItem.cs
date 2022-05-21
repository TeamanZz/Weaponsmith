using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable][CreateAssetMenu(fileName = "Skill", menuName = "Skills Data", order = 0)]
public class SkillItem: ScriptableObject
{
    public bool isActive = false;

    public int ID;
    public string skillName;
    public int skillLvl;

    public Sprite iconImage;
    public Color borderColor;

    public List<float> skillValue;
}
