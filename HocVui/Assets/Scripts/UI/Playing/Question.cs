using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class Question : MonoBehaviour
{
    [SerializeField] GameObject QuestionField;
    [SerializeField] GameObject title;
    [SerializeField] GameObject ans1;
    [SerializeField] GameObject ans2;
    [SerializeField] GameObject ans3;
    [SerializeField] GameObject ans4;
    [SerializeField] GameObject image;

    [SerializeField] float duration;
    [SerializeField] LeanTweenType type;
    [SerializeField] Vector3 defaultPostion;

    [SerializeField] LeanTweenType typeFadeInImage;
    [SerializeField] LeanTweenType typeFadeOutImage;

    [SerializeField] float time;
    [SerializeField] TMP_Text TextCount;
    Coroutine Timer;
    private float positionX = 800;

    public void startAnimation()
    {
        time = 10;
        TitleMoveX(title.transform.position.x - positionX);
        AnsMoveX(0.25f);
        fadeInImage(1);
        Timer = StartCoroutine(counter());
        LeanTween.alpha(title, 1f, duration);

    }

    public void stopAnimation()
    {
        TitleMoveX(title.transform.position.x + positionX);
        positionX *= -1;
        AnsMoveX(0.25f);
        fadeOutImage(0);
        LeanTween.alpha(title, 0f, duration);
    }
    public void TitleMoveX(float position)
    {
        LeanTween.moveX(title, position, duration).setEase(type);

    }
        

    public void AnsMoveX(float deplay)
    {
        Invoke(nameof(Ans1Move), deplay);
        Invoke(nameof(Ans2Move), 2 * deplay);
        Invoke(nameof(Ans3Move), 3 * deplay);
        Invoke(nameof(Ans4Move), 4 * deplay);
    }

    public void Ans1Move()
    {
        LeanTween.moveX(ans1, ans1.transform.position.x - positionX, duration).setEase(type);
    }

    public void Ans2Move()
    {
        LeanTween.moveX(ans2, ans2.transform.position.x - positionX, duration).setEase(type);
    }

    public void Ans3Move( )
    {
        LeanTween.moveX(ans3, ans3.transform.position.x - positionX, duration).setEase(type);
    }

    public void Ans4Move( )
    {
        LeanTween.moveX(ans4, ans4.transform.position.x - positionX, duration).setEase(type);
    }

    public void fadeInImage(float scale)
    {
      
        LeanTween.scale(image, new Vector3(scale, scale, 0), duration).setEase(typeFadeInImage);
    }

    public void fadeOutImage(float scale)
    {
        LeanTween.scale(image, new Vector3(scale, scale, 0), duration).setEase(typeFadeInImage);
    }

    private void timeFadeIn()
    {
        LeanTween.scale(TextCount.gameObject, new Vector3(1.5f, 1.5f, 0), 0.5f).setEase(LeanTweenType.punch);
        Invoke(nameof(timeFadeOut), 0.5f);
    }

    private void timeFadeOut()
    {
        LeanTween.scale(TextCount.gameObject, new Vector3(1, 1, 0), 0.5f).setEase(LeanTweenType.pingPong);
    }

    IEnumerator counter()
    {

        while (time > 0)
        {
            TextCount.text = string.Format("{0}", time);
            timeFadeIn();
            yield return new WaitForSeconds(1f);
            time--;
        }

        if (time <= 0)
        {
            stopAnimation();
            TextCount.text = string.Format("Hết giờ", time);
        }
    }
}
