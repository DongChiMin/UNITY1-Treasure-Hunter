using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField] float piecesForce;
    [SerializeField] float disableDelay;
    [SerializeField] float fallingDelay;
    [SerializeField] float destroyDelay;

    [Header("Drag")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D coll;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject crate;
    [SerializeField] private Rigidbody2D[] cratePieces;

    private void Start()
    {
        animator.Play("Idle");
    }

    public void GetHit()
    {
        animator.Play("Hit", 0, 0);
    }

    public void Death()
    {
        animator.Play("Destroyed", 0, 0);
        StartCoroutine(DestroyCrate());
        foreach (Rigidbody2D rb in cratePieces)
        {
            rb.gameObject.SetActive(true);
            Vector2 direction = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 4f));
            rb.AddForce(direction * piecesForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator DestroyCrate()
    {
        yield return new WaitForSeconds(disableDelay);
        coll.enabled = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        crate.SetActive(false);
        yield return new WaitForSeconds(destroyDelay);
        StartCoroutine(DisappearEffect());
    }

    IEnumerator DisappearEffect()
    {
        yield return new WaitForSeconds(fallingDelay);
        for (int i = 0; i < cratePieces.Length; i++)
        {
            cratePieces[i].gameObject.GetComponent<Collider2D>().enabled = false;
        }
        coll.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
