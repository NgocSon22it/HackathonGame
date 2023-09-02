using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pile : Pile_Common
{
    private float collectionRadius = 10.0f;
    private bool isActive = true;

    [SerializeField] SpriteRenderer spriteRenderer;

    private void OnMouseEnter()
    {
        spriteRenderer.color = Color.red;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (IsPlayerCloseEnough())
        {
            CollectWood();
            spriteRenderer.color = Color.white;
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
