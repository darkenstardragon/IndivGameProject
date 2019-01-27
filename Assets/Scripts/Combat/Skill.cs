using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Skill
{
    public Image icon;
    public string name;
    public string description;
    public int id;
    public float cooldown;
    [HideInInspector]
    public float currentCooldown = 0;
    // Start is called before the first frame update

    public Skill(Skill s)
    {
        icon = s.icon;
        name = s.name;
        description = s.description;
        id = s.id;
        cooldown = s.cooldown;
    }
    
}
