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

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] GameObject player;

    [Header("Player")]
    public GameObject PlayerManager;

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
    Vector2 randomPosition;

    [Header("PlayerList")]
    GameObject[] ListPlayer;
    string PlayerTag = "Player";

    [Header("Event")]
    private const byte Start_CountDownEventCode = 1;
    private const byte Start_ShowResultCode = 2;

    [Header("Result")]
    [SerializeField] TMP_Text ResultTxt;
    int ResultIndex;

    [Header("Ranking")]
    public TMP_Text rankingText;

    ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            PlayerManager = PhotonNetwork.Instantiate("Player/" + player.name, new(0, 0, 0), Quaternion.identity);
        }
        else
        {
            Debug.Log("K co");
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby 1");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && PhotonNetwork.IsMasterClient)
        {
            Start_CountDownPanel();
        }

        if (Input.GetKeyDown(KeyCode.J) && PhotonNetwork.IsMasterClient)
        {
            Start_ShowResultPanel();
        }

        if (Input.GetKeyDown(KeyCode.K) && PhotonNetwork.IsMasterClient)
        {
            ResultIndex = Random.Range(1, 5);
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Result", ResultIndex } });
        }
        if (Input.GetKeyDown(KeyCode.B) && PhotonNetwork.IsMasterClient)
        {
            int a = Random.Range(0, 4);
            SubmitValue("Son " + a, ResultIndex);
        }

    }

    #region Set Up Pile
    public Vector2 GetRandomPosition()
    {
        // Get the bounds of the collider
        minPosition = randomBounds.bounds.min;
        maxPosition = randomBounds.bounds.max;

        // Generate random X and Y coordinates within the collider bounds
        randomX = Random.Range(minPosition.x, maxPosition.x);
        randomY = Random.Range(minPosition.y, maxPosition.y);

        // Create a new position using the random coordinates
        randomPosition = new Vector2(randomX, randomY);

        return randomPosition;
    }

    public void SetRandomPile()
    {
        for (int i = 0; i < ListPile.Count; i++)
        {
            ListPile[i].transform.position = GetRandomPosition();
            ListPile[i].gameObject.SetActive(true);
        }
    }

    #endregion

    public void FindAllPlayer()
    {
        ListPlayer = GameObject.FindGameObjectsWithTag(PlayerTag);
    }

    IEnumerator CountDown_ShowQuestion()
    {
        currentTime_ShowQuestion = TotalTime_ShowQuestion;
        Countdown_ShowQuestionTxt.gameObject.SetActive(true);

        while (currentTime_ShowQuestion > 0)
        {
            Countdown_ShowQuestionTxt.text = string.Format("{0}", currentTime_ShowQuestion);

            yield return new WaitForSeconds(1f);

            currentTime_ShowQuestion--;
        }

        Countdown_ShowQuestionTxt.gameObject.SetActive(false);
        SetRandomPile();
    }

    public void Start_CountDownPanel()
    {
        PhotonNetwork.RaiseEvent(Start_CountDownEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void Start_ShowResultPanel()
    {
        PhotonNetwork.RaiseEvent(Start_ShowResultCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == Start_CountDownEventCode)
        {
            SetRandomPile();
        }
        else if (photonEvent.Code == Start_ShowResultCode)
        {
            SetResult();

        }
    }
    public void SelectOption(int Index)
    {
        PlayerProperties["SelectOption"] = Index;

        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties);
    }

    public void SetResult()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Result", out object randomValueObj))
        {
            int randomValue = (int)randomValueObj;
            ResultTxt.text = randomValue.ToString();
        }
        UpdateRanking();
    }

    public void SubmitValue(string playerName, int playerSelection)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Result", out object randomValueObj))
        {
            int randomValue = (int)randomValueObj;

            if (playerSelection == randomValue)
            {
                AddScore(playerName, 100);
            }
            else
            {
                AddScore(playerName, 0);

            }
        }
    }
    public void AddScore(string PlayerName, int Score)
    {
        if (References.RankingList.ContainsKey(PlayerName))
        {
            References.RankingList[PlayerName] += Score;
        }
        else
        {
            References.RankingList.Add(PlayerName, Score);
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "List", References.RankingList } });
    }

    private void UpdateRanking()
    {
        // Sort the players by score in descending order.
        Dictionary<string, int> a = (Dictionary<string, int>)PhotonNetwork.CurrentRoom.CustomProperties["List"];

        var sortedRanking = a.OrderByDescending(pair => pair.Value);

        // Create a ranking string.
        string ranking = "Ranking:\n";
        int rank = 1;

        foreach (var entry in sortedRanking)
        {
            ranking += $"{rank}. {entry.Key} - {entry.Value} points\n";
            rank++;
        }

        // Display the ranking in a Text UI element.
        if (rankingText != null)
        {
            rankingText.text = ranking;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdateRanking();
    }

}
