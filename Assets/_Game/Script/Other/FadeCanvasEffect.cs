using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvasEffect : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float speed;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(1));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        if(gameObject.activeSelf)
        {
            StartCoroutine(Fade(0));
        }
    }

    IEnumerator Fade(float targetAlpha)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha)) // Kiem tra alpha co gan voi target khong 
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null; // Doi den frame tiep theo
        }
    }
}
