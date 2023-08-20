using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AllUI : MonoBehaviour
{
    [Header("Pick up Pile")]
    [SerializeField] GameObject PickUp_AlreadyMessage;
    Coroutine PickUp_AlreadyCoroutine;

    public void PickUp_Already_Show()
    {
        if(PickUp_AlreadyCoroutine != null)
        {
            StopCoroutine(PickUp_AlreadyCoroutine);
        }

        PickUp_AlreadyCoroutine = StartCoroutine(PickUp_Already());
    }

    IEnumerator PickUp_Already()
    {
        PickUp_AlreadyMessage.SetActive(true);

        yield return new WaitForSeconds(3f);

        PickUp_AlreadyMessage.SetActive(false);
    }
}
