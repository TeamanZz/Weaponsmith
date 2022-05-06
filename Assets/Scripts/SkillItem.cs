using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SkillItem
{
    public int ID;
    public string skillName;
    public int skillLvl;

    public Sprite iconImage;
    public Color borderColor;

    public List<float> skillValue;
}
