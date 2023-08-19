using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Entity
{
    public string Name;
    public string Description;
    public SpellType Type;

    public Spell_Entity(string name, string description,  SpellType type)
    {
        Name = name;
        Description = description;
        Type = type;
    }
}
