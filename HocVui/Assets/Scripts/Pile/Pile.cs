using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pile : MonoBehaviourPunCallbacks
{
    public float collectionRadius = 2.0f;
    private bool isActive = true;

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
            CollectWood();
        }
    }

    [PunRPC]
    void SyncActivation(bool newState)
    {
        isActive = newState;
        gameObject.SetActive(isActive);
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
            gameObject.SetActive(isActive);
        }
    }

    private bool IsPlayerCloseEnough()
    {
        float distance = Vector3.Distance(transform.position, GameManager.Instance.PlayerManager.transform.position);

        return distance <= collectionRadius;
    }

    private void CollectWood()
    {
        Player_Base refer = GameManager.Instance.PlayerManager.GetComponent<Player_Base>();

        if (!refer.IsPile)
        {
            isActive = !isActive;
            photonView.RPC(nameof(SyncActivation), RpcTarget.All, isActive);
        }

        refer.CollectPile();

    }
}
