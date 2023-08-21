using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    private List<int> listnumber = new List<int> { 0, 1, 2, 3, 4, 5 };

    void Update()

    {

        if (Input.GetMouseButtonDown(0))
        {
            List<Spell_Entity> randomAccounts = GetRandomSpell_Entitys(References.ListSpell, 3);

            Debug.Log($" {randomAccounts[0].Spell} {randomAccounts[1].Spell} {randomAccounts[2].Spell}");
        }
    }


    List<Spell_Entity> GetRandomSpell_Entitys(List<Spell_Entity> sourceList, int count)
    {

        List<Spell_Entity> result = new List<Spell_Entity>();
        List<Spell_Entity> availableSpell_Entitys = new List<Spell_Entity>(sourceList);

        System.Random random = new System.Random();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = random.Next(0, availableSpell_Entitys.Count);
            Spell_Entity selectedSpell_Entity = availableSpell_Entitys[randomIndex];
            result.Add(selectedSpell_Entity);
            availableSpell_Entitys.RemoveAt(randomIndex);
        }

        return result;
    }
}
