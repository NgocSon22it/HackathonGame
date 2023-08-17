using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Effect : MonoBehaviour
{
    protected List<string> AttackAble_Tag = new List<string>()
    {
        "Enemy", "Ground", "Clone", "BossObstacle"
    };

    protected int Damage;

    protected float LifeTime;

    public void OnEnable()
    {
        LifeTime = 0.5f;
        Invoke(nameof(TurnOff), LifeTime);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        CancelInvoke();
    }
}
