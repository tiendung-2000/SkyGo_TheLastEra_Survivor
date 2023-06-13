using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextScroll : MonoBehaviour
{
    private float speed;
    [SerializeField]
    private TextMeshProUGUI textComponent;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private float delayTime;
    private float width;

    Coroutine coroutine;

    private void Awake()
    {
        OnInit();
    }

    private void OnEnable()
    {
        OnInit();
    }

    public void OnInit()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        speed = 5f;
        rectTransform.anchoredPosition = Vector2.zero;
        textComponent.ForceMeshUpdate();
        width = textComponent.textBounds.size.x;
        if (width > rectTransform.rect.width)
        {
            coroutine = StartCoroutine(ScrollText());
        }
    }
    //FIXED: Just scroll right to left
    //IEnumerator ScrollText()
    //{
    //    bool moveRight = false;

    //    while (true)
    //    {
    //        if (!moveRight && rectTransform.anchoredPosition.x - speed * Time.deltaTime <= -width)
    //        {
    //            yield return new WaitForSeconds(delayTime);
    //            moveRight = true;
    //        }
    //        else if (moveRight && rectTransform.anchoredPosition.x + speed * Time.deltaTime >= 0)
    //        {
    //            yield return new WaitForSeconds(delayTime);
    //            moveRight = false;
    //        }
    //        rectTransform.anchoredPosition += new Vector2((moveRight ? 1 : -1)  speed  Time.deltaTime, 0f);
    //        yield return null;
    //    }
    //}
    IEnumerator ScrollText()
    {
        while (true)
        {
            rectTransform.anchoredPosition -= new Vector2(speed, 0f);

            if (rectTransform.anchoredPosition.x <= -width)
            {
                rectTransform.anchoredPosition += new Vector2(width * 1.2f, 0f);
            }

            yield return null;
        }
    }
}
