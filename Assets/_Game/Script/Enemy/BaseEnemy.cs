using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("Attribute")]
    public int maxHealth;
    private int currentHealth;
    [SerializeField] float knockbackForce;
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
        currentHealth = maxHealth;
        animator.Play("Idle");
    }

    public void TakeDamage(int damage, float localScaleX)
    {
        //Hiệu ứng rung camera
        CameraShake.Instance.ShakeXY(0.2f);

        //Giảm máu
        currentHealth -= damage;

        //Hiệu ứng bị đánh bật về phía sau
        transform.rotation = Quaternion.identity;
        float directionX = Mathf.Sign(localScaleX) * Random.Range(1f, 2f);
        float directionY = Random.Range(4f, 6f);
        rb.AddForce(new Vector2(directionX * knockbackForce, directionY), ForceMode2D.Impulse);
        Debug.Log(directionX);

        //Animation Hit
        animator.Play("Hit", 0, 0);

        //Hết máu --> chết
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        animator.Play("Destroyed", 0, 0);
        StartCoroutine(DestroyCrate());
        foreach(Rigidbody2D rb in cratePieces)
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
        for(int i = 0; i < cratePieces.Length; i++)
        {
            cratePieces[i].gameObject.GetComponent<Collider2D>().enabled = false;
        }
        coll.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
