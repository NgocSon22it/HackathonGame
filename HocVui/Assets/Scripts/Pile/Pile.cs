using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pile : MonoBehaviour
{
    public float collectionRadius = 2.0f;


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

    private bool IsPlayerCloseEnough()
    {
        float distance = Vector3.Distance(transform.position, GameManager.Instance.PlayerManager.transform.position);

        return distance <= collectionRadius;
    }

    private void CollectWood()
    {
        GameManager.Instance.PlayerManager.GetComponent<Player_Base>().CollectPile(this.gameObject);
    }
}
