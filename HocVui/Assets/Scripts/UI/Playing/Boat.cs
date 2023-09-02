using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Boat : MonoBehaviour
{
    [SerializeField] float positionSpawn;
    [SerializeField] float goc;
    [SerializeField] float duration;
    [SerializeField] LeanTweenType type;
    [SerializeField] GameObject AnsA;
    [SerializeField] GameObject AnsB;
    [SerializeField] GameObject AnsC;
    [SerializeField] GameObject AnsD;
    [SerializeField] float timeMove;
    private void Start()
    {
        InvokeRepeating(nameof(idle), 0, duration);
        MoveAnswers(1);       
    }

    private void MoveAnswers(int Ans)
    {
        switch (Ans)
        {
            case 1: Move(AnsA); break;
            case 2: Move(AnsB); break;
            case 3: Move(AnsC); break;
            case 4: Move(AnsD); break;
        }
    }

    private void Move(GameObject obj)
    {
        Vector3 position = obj.transform.position + new Vector3(0, 0.5f, 0); ;
        MoveGo(position, timeMove);
    }
    private void idle()
    {

        LeanTween.rotate(gameObject, new Vector3(0 ,0 ,goc), duration).setEase(type);
        goc *= -1;
    }

    private void MoveGo(Vector3 position, float time)
    {
        LeanTween.move(gameObject, position, time);
    }
}
