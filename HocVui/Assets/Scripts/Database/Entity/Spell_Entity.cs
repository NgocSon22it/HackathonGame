using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spell_Entity
{
    public string Name;
    public string Description;
    public string Image;
    public Spell Spell;
    public Spell_Type Spell_Type;
    public Spell_Target Spell_Target;
    public bool IsUse;

    public Spell_Entity(string name, string description, string image, Spell spell, Spell_Type spell_Type, Spell_Target spell_Target)
    {
        Name = name;
        Description = description;
        Image = image;
        Spell = spell;
        Spell_Type = spell_Type;
        IsUse = false;
        Spell_Target = spell_Target;
    }
}
