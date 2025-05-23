using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuidePopUp : MonoBehaviour
{

    [SerializeField] private RectTransform box;
    [SerializeField] private TextMeshProUGUI textMP;
    [SerializeField] GameObject canvas;
    private FadeCanvasEffect canvasEffect;

    [Header("Attributes")]
    [SerializeField] private float boxWidth;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private Vector2 boxOffset;
    [TextArea(3, 10)]
    [SerializeField] private string text;
    private void Start()
    {
        textMP.text = text;
        canvasEffect = canvas.GetComponent<FadeCanvasEffect>();
        canvasEffect.speed = fadeSpeed;

        canvas.transform.localPosition = new Vector3(boxOffset.x, boxOffset.y, 0);
        box.sizeDelta = new Vector2(box.sizeDelta.x, boxWidth);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canvasEffect.FadeOut();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canvasEffect.FadeIn();
        }
    }
}
