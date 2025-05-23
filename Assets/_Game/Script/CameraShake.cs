using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    private Vector3 originalPos;
    public enum ShakeType{
        Horizontal,
        Vertical
    }

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    public void ShakeX(float speed)
    {
        Vector3 target = transform.localPosition + new Vector3(Random.Range(0.2f, 0.35f), 0, 0);
        StartCoroutine(Shake(target, speed));
    }

    public void ShakeY(float speed)
    {
        Vector3 target = transform.localPosition + new Vector3(0, Random.Range(0.2f, 0.35f), 0);
        StartCoroutine(Shake(target, speed));
    }

    public void ShakeXY(float speed)
    {
        StopAllCoroutines();
        Vector3 target = transform.localPosition + new Vector3(Random.Range(0.2f, 0.35f), Random.Range(0.2f, 0.35f), 0);
        StartCoroutine(Shake(target, speed));
    }

    public IEnumerator Shake(Vector3 target, float speed)
    {
        while (Vector3.Distance(transform.localPosition,target) > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, speed);
            yield return null; 
        }
        while (Vector3.Distance(transform.localPosition, originalPos) > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, speed);
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
