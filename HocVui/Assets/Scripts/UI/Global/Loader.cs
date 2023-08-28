using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] GameObject obj;
    Coroutine coroutine;
    [SerializeField] float rotationValue;

    public void StartLoader()
    {
        coroutine = StartCoroutine(LoaderStart());
    }

    public void StopLoader()
    {
            StopCoroutine(coroutine);
    }

    IEnumerator LoaderStart()
    {
        float rotation = 0;
        float value = 270;
        float flag = -1;
        while (true)
        {
            yield return new WaitForSeconds(0.5f / 360);
            obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
            value += flag;
            obj.GetComponent<Slider>().value = value;
            if (rotation >= 360)
            {
                rotation = 0f;
            }
            else
            {
                rotation += rotationValue;
            }

            if(value == 0 || value == 360)
            {
                flag *= -1;
            }
        
        }
    }
}
