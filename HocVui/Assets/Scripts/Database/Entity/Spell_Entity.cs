using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Entity
{
    public string Name;
    public string Description;
    public string Image;
    public Spell Spell;

    public Spell_Entity(string name, string description, string image, Spell spell)
    {
        Name = name;
        Description = description;
        Image = image;
        Spell = spell;
    }
}
