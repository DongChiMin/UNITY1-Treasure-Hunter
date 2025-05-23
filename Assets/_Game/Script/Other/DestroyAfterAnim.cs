using System.Collections;
using UnityEngine;

public class DestroyAfterAnim : MonoBehaviour
{
    private void OnEnable()
    {
        //Tat gameObject sau khi animation chay xong
        StartCoroutine(SetActiveFalse(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
    }

    IEnumerator SetActiveFalse(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
