using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Point : MonoBehaviour
{
    public Transform TargetPosition;

    private void Update()
    {
        transform.position = TargetPosition.position;
    }
}
