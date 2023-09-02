using Assets.Scripts.Common;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile_Base : Pile_Common
{
    private bool isActive;
    private bool isLock;

    [SerializeField] GameObject IsPile;

    [SerializeField] public int answer;

    [SerializeField] GameObject Outline;

    [SerializeField] GameObject WrongAnswer;

    [SerializeField] GameObject Lock;


    [Header("Effect")]
    Coroutine BlockPileBase_Coroutine;


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
        if ((GameManager.Instance.PlayerManager.GetComponent<Player_Base>().Spell_Entity == null)
            || (GameManager.Instance.PlayerManager.GetComponent<Player_Base>().Spell_Entity.Spell_Target != Spell_Target.PileBase))
        {
            if (isLock)
            {
                GameManager.Instance.PlayerManager.GetComponent<Player_Base>()
                .PlayerAllUIInstance.GetComponent<Player_AllUI>().
                Message_On(Message.Game_PileBaseLock);
            }
            else
            {
                if (IsPlayerCloseEnough())
                {
                    PuttingPile();
                }
            }
        }
        else if (GameManager.Instance.PlayerManager.GetComponent<Player_Base>().Spell_Entity.Spell_Target == Spell_Target.PileBase)
        {
            photonView.RPC(nameof(BlockBase), RpcTarget.All);
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

    [PunRPC]
    void BlockBase()
    {
        isLock = true;
        Lock.gameObject.SetActive(isLock);
        if (BlockPileBase_Coroutine != null)
        {
            StopCoroutine(BlockPileBase_Coroutine);
        }
        GameManager.Instance.PlayerManager.GetComponent<Player_Base>().Spell_Entity = null;
        Spell_Manager.Instance.SetUp_SkillUse();
        BlockPileBase_Coroutine = StartCoroutine(BlockBase_Start());

    }

    IEnumerator BlockBase_Start()
    {
        yield return new WaitForSeconds(5f);
        photonView.RPC(nameof(ClearEffect), RpcTarget.All);
    }

    [PunRPC]
    public void ClearEffect()
    {
        isLock = false;
        Lock.gameObject.SetActive(isLock);
    }

}
