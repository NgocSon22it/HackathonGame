using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.Demo.PunBasics;

public class GameManager : MonoBehaviourPunCallbacks
{
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

    private void Awake()
    {
        Instance = this;
        PlayerManager = GameObject.FindGameObjectWithTag("Player");
    }


    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(CountDown_ShowQuestion());
    }

    // Update is called once per frame
    void Update()
    {

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
        FindAllPlayer();
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
}
