using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentUI : MonoBehaviour
{
    [SerializeField] GameObject BackgroundUp;
    [SerializeField] GameObject BackgroundDown;

    public void WaterUp()
    {
        BackgroundUp.SetActive(true);
    }

    public void WaterDown()
    {
        BackgroundUp.SetActive(false);
    }
}
