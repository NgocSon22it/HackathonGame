using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spell_Manager : MonoBehaviour
{
    [Header("Instance")]
    public static Spell_Manager Instance;

    [Header("Instance")]
    [SerializeField] GameObject SpellInformation_Panel;
    [SerializeField] TMP_Text SpellInformation_Name;
    [SerializeField] TMP_Text SpellInformation_Description;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpellInformation_On()
    {
        SpellInformation_Panel.SetActive(true);
    }

    public void SpellInformation_Off()
    {
        SpellInformation_Panel.SetActive(false);
    }

}
