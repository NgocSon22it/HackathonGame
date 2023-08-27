using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile_Base : MonoBehaviourPunCallbacks
{
    public float collectionRadius = 2.0f;
    private bool isActive;
    [SerializeField] GameObject IsPile;
    [SerializeField] int Index;

    private void OnMouseEnter()
    {
        GameManager.Instance.PlayerManager.GetComponent<Player_Base>().MouseOverEffect_Pile_On(transform.position);

    }

    private void OnMouseExit()
    {
        GameManager.Instance.PlayerManager.GetComponent<Player_Base>().MouseOverEffect_Pile_Off();


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

    private bool IsPlayerCloseEnough()
    {
        float distance = Vector3.Distance(transform.position, GameManager.Instance.PlayerManager.transform.position);

        return distance <= collectionRadius;
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

        refer.PuttingPile(Index);
    }
}
