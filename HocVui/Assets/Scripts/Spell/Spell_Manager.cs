using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spell_Manager : MonoBehaviour
{
    [Header("Instance")]
    public static Spell_Manager Instance;

    [Header("Select Spell")]
    [SerializeField] Sprite DefaultContainer, SelectContainer;
    [SerializeField] List<Image> ListContainer;

    [Header("SpellInformation")]
    [SerializeField] GameObject SpellInformation_Panel;
    [SerializeField] TMP_Text SpellInformation_Name;
    [SerializeField] TMP_Text SpellInformation_Description;

    [Header("PlayerList")]
    GameObject[] ListPlayer;
    string PlayerTag = "Player";

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FindAllPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpellInformation_On(int Index)
    {
        SpellInformation_Name.text = References.ListSpell[Index].Name;
        SpellInformation_Description.text = References.ListSpell[Index].Description;
        SpellInformation_Panel.SetActive(true);
    }

    public void SpellInformation_Off()
    {
        SpellInformation_Panel.SetActive(false);
    }

    public void SelectedSpell(int Index)
    {
        foreach (Image image in ListContainer)
        {
            //image.sprite = DefaultContainer;
        }

        //ListContainer[Index].sprite = SelectContainer;
    }

    public void FindAllPlayer()
    {
        ListPlayer = GameObject.FindGameObjectsWithTag(PlayerTag);
    }

}
