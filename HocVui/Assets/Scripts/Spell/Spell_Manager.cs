using Photon.Pun;
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

    [Header("SpellInformation")]
    [SerializeField] GameObject SpellInformation_Panel;
    [SerializeField] TMP_Text SpellInformation_Name;
    [SerializeField] TMP_Text SpellInformation_Description;

    [Header("PlayerList")]
    List<GameObject> ListPlayer = new List<GameObject>();
    string PlayerTag = "Player";

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUp_Spell();
    }

    // Update is called once per frame
    void Update()
    {

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
        foreach (Image image in ListContainer)
        {
            image.sprite = DefaultContainer;
        }
        FindNonLocalPlayers();
        ListContainer[Index].sprite = SelectContainer;
        GameManager.Instance.PlayerManager.GetComponent<Player_Base>().SpellPoint_On(ListPlayer[0].transform);
        //Debug.Log(ListPlayer[0].transform.position);
    }

    private void FindNonLocalPlayers()
    {
        // Find all GameObjects with the specified tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Iterate through each GameObject and check if PhotonView.isMine is false
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && !photonView.IsMine)
            {
                // Add the GameObject to the list
                ListPlayer.Add(player);
            }
        }
    }

}
