using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class References 
{
    public static List<Spell_Entity> ListSpell = new List<Spell_Entity>()
    {
        new Spell_Entity("Freeze", "Đóng băng", SpellType.Type1),
        new Spell_Entity("Fire", "Lửa", SpellType.Type2),
        new Spell_Entity("Water", "Nước", SpellType.Type3),
    };


    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder sb = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            sb.Append(chars[new System.Random().Next(chars.Length)]);
        }

        return sb.ToString();
    }

}

public enum SpellType
{
    Type1, Type2, Type3
}
