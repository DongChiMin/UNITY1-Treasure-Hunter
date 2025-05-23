using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestKey : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float effectLength;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            animator.SetBool("Effect", true);
            StartCoroutine(DestroyAfterAnim(effectLength));
        }
    }

    IEnumerator DestroyAfterAnim(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
