using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.TextCore.Text;
using System.IO;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.PlayerLoop;
using System.Linq;
using System.Security.Cryptography;
using Assets.Scripts.Database.Entity;
using UnityEngine.InputSystem;
using System.Xml;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject PlayerPrefabs;

    [SerializeField] GameObject MasterClientPrefabs;

    [SerializeField] GameObject StartBtn;

    [Header("Player")]
    public GameObject PlayerManager;

    [SerializeField] Transform SpawnPosition;

    [SerializeField] public PolygonCollider2D CameraBox;

    [Header("Instance")]
    public static GameManager Instance;

    [Header("Count down Show Questions")]
    float TotalTime_ShowQuestion = 3f, currentTime_ShowQuestion;
    [SerializeField] TMP_Text Countdown_ShowQuestionTxt;

    [Header("List Pile")]
    [SerializeField] List<GameObject> ListPile;
    [SerializeField] Collider2D randomBounds;
    Vector2 minPosition, maxPosition;
    float randomX, randomY;
    Vector3 randomPosition;

    [Header("List PileBase")]
    [SerializeField] public List<GameObject> ListPileBase;

    [Header("PlayerList")]
    GameObject[] ListPlayer;
    string PlayerTag = "Player";

    [Header("Result")]
    [SerializeField] TMP_Text ResultTxt;
    public int ResultIndex;

    [Header("Ranking")]
    public TMP_Text rankingText;
    [SerializeField] GameObject Player_RankItem;
    [SerializeField] Transform RankItem_Content;

    private List<Transform> sortObjects = new List<Transform>();

    public bool IsBusy;

    public static Dictionary<string, int> List = new Dictionary<string, int>();

    ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitRank();
        SetUp_Player();
    }

    public void SetUp_Player()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            PlayerManager = PhotonNetwork.Instantiate("Player/" + PlayerPrefabs.name, SpawnPosition.position, Quaternion.identity);
            PlayerManager.GetComponent<Player_Base>().CameraBox = CameraBox;
            StartBtn.SetActive(false);
        }
        else
        {
            StartBtn.SetActive(true);
            Instantiate(MasterClientPrefabs, new(0, 0, 0), Quaternion.identity);
        }

    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        List = (Dictionary<string, int>)propertiesThatChanged["List"];

    }

    #region Set Up Pile
    public Vector3 GetRandomPosition()
    {
        // Get the bounds of the collider
        minPosition = randomBounds.bounds.min;
        maxPosition = randomBounds.bounds.max;

        // Generate random X and Y coordinates within the collider bounds
        randomX = Random.Range(minPosition.x, maxPosition.x);
        randomY = Random.Range(minPosition.y, maxPosition.y);

        // Create a new position using the random coordinates
        randomPosition = new Vector3(randomX, randomY, -5);

        return randomPosition;
    }

    public void SetRandomPile()
    {
        FindAllPlayer();

        for (int i = 0; i < ListPlayer.Length; i++)
        {
            ListPile[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < ListPlayer.Length; i++)
        {
            ListPile[i].transform.position = GetRandomPosition();
            ListPile[i].transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, -240f));

            ListPile[i].gameObject.SetActive(true);
        }
    }

    #endregion

    public void FindAllPlayer()
    {
        ListPlayer = GameObject.FindGameObjectsWithTag(PlayerTag);
    }

    public void SelectOption(int Index)
    {
        PlayerProperties["SelectOption"] = Index;

        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties);
    }

    public void InitRank()
    {
        List = (Dictionary<string, int>)PhotonNetwork.CurrentRoom.CustomProperties["List"];

        int rank = 1;
        foreach (KeyValuePair<string, int> entry in List)
        {
            Instantiate(Player_RankItem, RankItem_Content).GetComponent<Player_RankItem>().SetUp(rank, entry.Key, entry.Value);
            rank++;
        }

        foreach (Transform child in RankItem_Content)
        {
            sortObjects.Add(child);
        }

        Ranking_Sort();
    }

    public void Ranking_Sort()
    {
        var sortedDict = List.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        for (int i = 0; i < sortObjects.Count; i++)
        {
            sortObjects[i].gameObject.GetComponent<Player_RankItem>().SetUp(i + 1, sortedDict.ElementAt(i).Key, sortedDict.ElementAt(i).Value);
        }
    }

    public void AddScore(string PlayerName, int Score)
    {
        if (List.ContainsKey(PlayerName))
        {
            List[PlayerName] += Score;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "List", List } });
    }

    public void ResetRound()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            References.X2 = 1;
            References.TimeAnswer = 0;
            References.SelectedAnswer = -1;
            References.TimeFreeze = false;


            GameManager.Instance.PlayerManager.GetComponent<Player_Base>().CallReset();
            GameManager.Instance.PlayerManager.GetComponent<Player_Base>().PlayerAllUIInstance.GetComponent<Player_AllUI>().SelectedAnswer_Off();
            GameManager.Instance.PlayerManager.GetComponent<Player_Base>().PlayerAllUIInstance.GetComponent<Player_AllUI>().BuffInfo_Off();
        }
        else
        {
            PhotonNetwork.RaiseEvent(EventCode.Play_ResetPile, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            foreach (var playerObject in ListPileBase)
            {
                playerObject.GetComponent<Pile_Base>().ResetData();
            }
            Boat.Instance.ResetBoat();
        }



    }
}
