using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile_Common : MonoBehaviourPunCallbacks
{
    [Header("Enviroment Interaction")]
    [SerializeField] protected LayerMask PlayerLayer;
    [SerializeField] protected Vector2 DetectGroundVector;
    [SerializeField] protected Transform DetectGroundTransform;
    [SerializeField] protected float DetectGroundDistance;


    public bool IsPlayerCloseEnough()
    {
        return Physics2D.BoxCast(DetectGroundTransform.position, DetectGroundVector, 0, -DetectGroundTransform.up, DetectGroundDistance, PlayerLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(DetectGroundTransform.position - DetectGroundTransform.up * DetectGroundDistance, DetectGroundVector);
    }
}
