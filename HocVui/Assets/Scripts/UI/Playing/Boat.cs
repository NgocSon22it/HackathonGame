using Assets.Scripts.Game;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
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

    public static Boat Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void MoveAnswers(int Ans)
    {
        switch (Ans)
        {
            case 0: Move(AnsA); break;
            case 1: Move(AnsB); break;
            case 2: Move(AnsC); break;
            case 3: Move(AnsD); break;
        }
    }

    private void Move(GameObject obj)
    {
        Vector3 position = obj.transform.position + new Vector3(0, 0.5f, 0); ;
        MoveGo(position, timeMove);
    }
    private void idle()
    {
        LeanTween.rotate(gameObject, new Vector3(0, 0, goc), duration).setEase(type);
        goc *= -1;
    }

    private void MoveGo(Vector3 position, float time)
    {
        LeanTween.move(gameObject, position, time);
        Playing_Manager.Instance.SetUp_Wave();
        Invoke(nameof(next), time);
    }
    
    private void next()
    {
        Playing_Manager.Instance.Wave_Off();
        Playing_Manager.Instance.ShowResultBoat();
    }

    public void ResetBoatPosition()
    {
        transform.position = new Vector3(28.05f, 0.38f,0);
        Playing_Manager.Instance.ResetPushBox();
    }

    public void ResetBoat()
    {
        PhotonNetwork.RaiseEvent(EventCode.Play_ResetBoat, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }


}
