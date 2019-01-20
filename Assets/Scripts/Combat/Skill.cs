using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Skill
{
    public Texture icon;
    public string name;
    public string description;
    public int id;
    // Start is called before the first frame update

    public Skill(Skill s)
    {
        icon = s.icon;
        name = s.name;
        description = s.description;
        id = s.id;
    }
    
}
