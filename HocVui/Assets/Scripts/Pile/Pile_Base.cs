using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile_Base : Pile_Common
{
    private bool isActive;
    [SerializeField] GameObject IsPile;

    [SerializeField] public int answer;

    [SerializeField] GameObject Outline;

    [SerializeField] GameObject WrongAnswer;

    private void OnMouseEnter()
    {
        Outline.SetActive(true);
    }

    private void OnMouseExit()
    {
        Outline.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (IsPlayerCloseEnough())
        {
            PuttingPile();
        }
    }

    [PunRPC]
    void SyncActivation(bool newState)
    {
        isActive = newState;
        IsPile.SetActive(isActive);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isActive);
        }
        else
        {
            isActive = (bool)stream.ReceiveNext();
            IsPile.SetActive(isActive);
        }
    }

    private void PuttingPile()
    {
        Player_Base refer = GameManager.Instance.PlayerManager.GetComponent<Player_Base>();

        if (!refer.IsPileBase)
        {
            if (refer.IsPile)
            {
                isActive = true;
                photonView.RPC(nameof(SyncActivation), RpcTarget.All, isActive);
            }

        }

        refer.PuttingPile(answer);
    }

    public void SetUp_WrongAnswer(bool value)
    {
        WrongAnswer.SetActive(value);
    }
    
}
