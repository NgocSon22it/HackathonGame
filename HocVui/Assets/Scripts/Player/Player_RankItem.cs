using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_RankItem : MonoBehaviour
{
    public int Score;

    [SerializeField] TMP_Text NoTxt;
    [SerializeField] TMP_Text NameTxt;
    [SerializeField] TMP_Text ScoreTxt;

    public void SetUp(int rank, string Name, int Score)
    {
        this.Score = Score;

        NoTxt.text = rank.ToString();
        NameTxt.text = Name;
        ScoreTxt.text = this.Score.ToString();
    }
}
