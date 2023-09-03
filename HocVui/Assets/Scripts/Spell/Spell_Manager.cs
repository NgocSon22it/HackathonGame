using Assets.Scripts.Common;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Spell_Manager : MonoBehaviour
{
    [Header("Instance")]
    public static Spell_Manager Instance;

    [Header("Select Spell")]
    [SerializeField] Sprite DefaultContainer, SelectContainer;
    [SerializeField] List<Image> ListContainer;
    [SerializeField] List<Image> ListSpell;
    [SerializeField] List<Image> ListSpellCooldown;

    [Header("SpellInformation")]
    [SerializeField] GameObject SpellInformation_Panel;
    [SerializeField] TMP_Text SpellInformation_Name;
    [SerializeField] TMP_Text SpellInformation_Description;

    [Header("PlayerList")]
    List<GameObject> ListPlayer = new List<GameObject>();
    string PlayerTag = "Player";


    int CurrentSelect = -1;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUp_Spell();
    }

    public void SetUp_Spell()
    {
        for (int i = 0; i < 3; i++)
        {
            ListSpell[i].sprite = Resources.Load<Sprite>(References.ListSpell_Own[i].Image);
        }

    }

    public void SpellInformation_On(int Index)
    {
        SpellInformation_Name.text = References.ListSpell_Own[Index].Name;
        SpellInformation_Description.text = References.ListSpell_Own[Index].Description;
        SpellInformation_Panel.SetActive(true);
    }

    public void SpellInformation_Off()
    {
        SpellInformation_Panel.SetActive(false);
    }

    public void SelectedSpell(int Index)
    {
        switch (References.ListSpell_Own[Index].Spell_Type)
        {
            case Spell_Type.ClickToUse:
                SetUp_Spell_ClickToUse(Index);
                break;
            case Spell_Type.ConfirmToUse:
                SetUp_Spell_ConfirmToUse(Index);
                break;
        }
    }

    public void SetUp_Spell_ConfirmToUse(int Index)
    {
        if (References.ListSpell_Own[Index].IsUse == false)
        {
            if (CurrentSelect != Index)
            {
                CurrentSelect = Index;
                foreach (Image image in ListContainer)
                {
                    image.sprite = DefaultContainer;
                }

                ListContainer[Index].sprite = SelectContainer;

                SetUp_PlayerSpell(References.ListSpell_Own[Index]);

            }
            else
            {
                CurrentSelect = -1;
                ListContainer[Index].sprite = DefaultContainer;
                SetUp_PlayerSpell(null);
            }
        }
        else
        {
            GameManager.Instance.PlayerManager.GetComponent<Player_Base>()
            .PlayerAllUIInstance.GetComponent<Player_AllUI>().
            Message_On(string.Format(Message.Game_SpellUseAlready, References.ListSpell_Own[Index].Name));


        }

    }

    public void SetUp_PlayerSpell(Spell_Entity spell_Entity)
    {
        GameManager.Instance.PlayerManager.GetComponent<Player_Base>().Spell_Entity = spell_Entity;
    }

    public void SetUp_Spell_ClickToUse(int Index)
    {
        if (References.ListSpell_Own[Index].IsUse == false)
        {
            switch (References.ListSpell_Own[Index].Spell)
            {
                case Spell.FiftyFifty:
                    FiftyFifty(Index);
                    break;
                case Spell.DoubleScore:
                    DoubleScore(Index);
                    break;
                case Spell.TimeFreeze:
                    TimeFreeze(Index);
                    break;
            }
        }
        else
        {
            GameManager.Instance.PlayerManager.GetComponent<Player_Base>()
            .PlayerAllUIInstance.GetComponent<Player_AllUI>().
            Message_On(string.Format(Message.Game_SpellUseAlready, References.ListSpell_Own[Index].Name));

        }

    }

    public void FiftyFifty(int Index)
    {
        References.ListSpell_Own[Index].IsUse = true;
        ListSpellCooldown[Index].fillAmount = 1f;

        List<int> listAnswer = new List<int>();
        List<int> listRemove = new List<int>();
        int randomIndex;

        foreach (var value in GameManager.Instance.ListPileBase)
        {
            listAnswer.Add(value.GetComponent<Pile_Base>().answer);
        }

        listAnswer.Remove(GameManager.Instance.ResultIndex);

        for (int i = 0; i < 2; i++)
        {
            randomIndex = UnityEngine.Random.Range(0, listAnswer.Count);
            listRemove.Add(listAnswer[randomIndex]);
            listAnswer.Remove(listAnswer[randomIndex]);
        }

        foreach (var value in GameManager.Instance.ListPileBase)
        {
            value.GetComponent<Pile_Base>().SetUp_WrongAnswer(false);
        }

        foreach (var obj in GameManager.Instance.ListPileBase)
        {
            var script = obj.GetComponent<Pile_Base>();

            // Check if the script value is in the valuesToCheck list
            if (script != null && listRemove.Contains(script.answer))
            {
                script.SetUp_WrongAnswer(true);
            }
        }
    }

    public void DoubleScore(int Index)
    {
        References.ListSpell_Own[Index].IsUse = true;
        ListSpellCooldown[Index].fillAmount = 1f;

        References.X2 = 2;
        GameManager.Instance.PlayerManager.GetComponent<Player_Base>()
            .PlayerAllUIInstance.GetComponent<Player_AllUI>().BuffInfo_On(Message.Buff_X2Score);
    }

    public void TimeFreeze(int Index)
    {
        References.ListSpell_Own[Index].IsUse = true;
        ListSpellCooldown[Index].fillAmount = 1f;
        References.TimeFreeze = true;

        GameManager.Instance.PlayerManager.GetComponent<Player_Base>()
            .PlayerAllUIInstance.GetComponent<Player_AllUI>().BuffInfo_On(Message.Buff_TimeFreeze);
    }

    public void SetUp_SkillUse()
    {
        References.ListSpell_Own[CurrentSelect].IsUse = true;
        ListSpellCooldown[CurrentSelect].fillAmount = 1f;
        ListContainer[CurrentSelect].sprite = DefaultContainer;
    }


}
