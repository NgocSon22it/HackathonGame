using Assets.Scripts.Game;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowLeader : MonoBehaviour
{
    [SerializeField] GameObject RankImg;
    [SerializeField] GameObject AvatarPlayer;
    [SerializeField] GameObject PlayerName;
    [SerializeField] GameObject ScorePlayer;
    [SerializeField] GameObject ListScore;
    [SerializeField] GameObject Background;

    [SerializeField] LeanTweenType typeFadeIn;
    [SerializeField] LeanTweenType typeFadeOut;
    [SerializeField] float duration;
    [SerializeField] float positonX;

    [SerializeField] GameObject Top1Player;
    [SerializeField] GameObject Top2Player;
    [SerializeField] GameObject Top3Player;
    [SerializeField] GameObject Leader;

    [Header("Ranking")]
    public TMP_Text rankingText;
    [SerializeField] GameObject Player_RankItem;
    [SerializeField] Transform RankItem_Content;
    private List<Transform> sortObjects = new List<Transform>();

    [SerializeField] List<TMP_Text> ListName = new List<TMP_Text>();

    public static Dictionary<string, int> List = new Dictionary<string, int>();

    public List<Sprite> TopRank;
    Coroutine animation;

    public void run()
    {
        ResetUI();
        animation = StartCoroutine(startAnimation());
    }

    public void stop()
    {
        StopCoroutine(animation);
    }
    IEnumerator startAnimation()
    {
        Background.SetActive(true);

        List = (Dictionary<string, int>)PhotonNetwork.CurrentRoom.CustomProperties["List"];

        var sortedDict = List.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        if (sortedDict.Count > 2)
        {
            for (int i = 2; i >= 0; i--)
            {
                StartCoroutine(AnimationShowPlayerTop(i + 1, sortedDict.ElementAt(i).Value, "", sortedDict.ElementAt(i).Key));
                ListName[i].text = sortedDict.ElementAt(i).Key;
                yield return new WaitForSeconds(6);
            }

            ShowListRank();
            Background.SetActive(false);
        }
    }

    private void ResetUI()
    {
        ScorePlayer.SetActive(false);
        ListScore.SetActive(false);
        Background.SetActive(false);
    }

    private void ShowListRank()
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

        var sortedDict = List.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        for (int i = 0; i < sortObjects.Count; i++)
        {
            sortObjects[i].gameObject.GetComponent<Player_RankItem>().SetUp(i + 1, sortedDict.ElementAt(i).Key, sortedDict.ElementAt(i).Value);
        }

        ListScore.SetActive(true);
        Playing_Manager.Instance.EndBXH();
    }

    IEnumerator AnimationShowPlayerTop(int rank, float score, string avatar, string name)
    {
        RankImg.GetComponent<Image>().sprite = TopRank[rank - 1];
        ScorePlayer.GetComponent<TMP_Text>().text = score.ToString();
        /*AvatarPlayer.GetComponent<Image>().sprite = avatar;*/
        PlayerName.GetComponent<TMP_Text>().text = name;

        StartCoroutine(EffectAnimation(RankImg));
        yield return new WaitForSeconds(duration * 2);
        StartCoroutine(EffectAnimation(ScorePlayer));
        yield return new WaitForSeconds(duration * 2);
        StartCoroutine(EffectAnimation(AvatarPlayer));
        yield return new WaitForSeconds(duration * 2);
        StartCoroutine(EffectAnimation(PlayerName));
        yield return new WaitForSeconds(duration * 2);

        switch (rank)
        {
            case 1: StartCoroutine(Top1Effect()); break;
            case 2: StartCoroutine(Top2Effect()); break;
            case 3: StartCoroutine(Top3Effect()); break;

            default: break;
        }
    }

    IEnumerator EffectAnimation(GameObject obj)
    {
        obj.SetActive(true);
        FadeIn(obj);
        yield return new WaitForSeconds(duration);
        FadeOut(obj);
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
    }

    private void FadeIn(GameObject obj)
    {
        obj.transform.localPosition = new Vector3(-positonX, 0, 0);
        LeanTween.moveLocalX(obj, 0f, duration).setEase(typeFadeIn);
    }

    private void FadeOut(GameObject obj)
    {
        LeanTween.moveLocalX(obj, positonX, duration).setEase(typeFadeOut);
    }

    IEnumerator Top1Effect()
    {
        Top1Player.SetActive(true);
        Top1Player.gameObject.transform.position = this.gameObject.transform.position;
        Top1Player.gameObject.transform.localScale = Vector3.zero;

        LeanTween.scale(Top1Player, Vector3.one, duration).setEase(LeanTweenType.easeOutElastic);
        yield return new WaitForSeconds(duration);
        LeanTween.move(Top1Player, Leader.transform.position, 1f).setEase(LeanTweenType.easeInBack);
    }

    IEnumerator Top2Effect()
    {
        Top2Player.SetActive(true);

        Top2Player.gameObject.transform.position = this.gameObject.transform.position;
        Top2Player.gameObject.transform.localScale = Vector3.zero;

        LeanTween.scale(Top2Player, Vector3.one, duration).setEase(LeanTweenType.easeOutElastic);
        yield return new WaitForSeconds(duration);
        LeanTween.move(Top2Player, Leader.transform.position + new Vector3(-1.75f, 0, 0), 1f).setEase(LeanTweenType.easeInBack);
    }

    IEnumerator Top3Effect()
    {
        float y = Leader.transform.position.y;
        Top3Player.SetActive(true);

        Top3Player.gameObject.transform.position = this.gameObject.transform.position;
        Top3Player.gameObject.transform.localScale = Vector3.zero;

        LeanTween.scale(Top3Player, Vector3.one, duration).setEase(LeanTweenType.easeOutElastic);
        yield return new WaitForSeconds(duration);
        LeanTween.move(Top3Player, Leader.transform.position + new Vector3(1.75f, 0, 0), 1f).setEase(LeanTweenType.easeInBack);
    }
}
