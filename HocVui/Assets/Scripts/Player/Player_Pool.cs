using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pool : MonoBehaviour
{
    [SerializeField] List<GameObject> List_Effect_ClickToMove = new List<GameObject>();

    public GameObject GetEffect_ClickToMove()
    {
        for (int i = 0; i < List_Effect_ClickToMove.Count; i++)
        {
            if (!List_Effect_ClickToMove[i].activeInHierarchy)
            {
                return List_Effect_ClickToMove[i];
            }
        }
        return null;
    }

}
