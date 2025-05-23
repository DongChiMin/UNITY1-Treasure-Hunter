using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordThrow : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private float pickupDelayTime;
    [SerializeField] private float maxFlyingDuration;
    [SerializeField] private int attackDamage;
    private bool isFlying;


    private void OnEnable()
    {
        //Neu sword quay sang ben phai thi bay sang ben phai
        if(transform.localScale == Vector3.one)
        {
            rb.velocity = Vector2.right * speed;
        }
        else
        {
            rb.velocity = Vector2.left * speed;
        }
        isFlying = true;
        gameObject.layer = LayerMask.NameToLayer("SwordThrow");
        StartCoroutine(ChangeLayerAfterDelay(pickupDelayTime));
        StartCoroutine(FlyDestroy(maxFlyingDuration));
    }

    IEnumerator FlyDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        if(isFlying )
        {
            //Destroy hoac set Active False
            PoolManager.Instance.poolSwordThrowPool.AddToPool(gameObject);
        }
    }

    IEnumerator ChangeLayerAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void Embedded(Vector3 position)
    {
        isFlying = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        animator.SetBool("Embedded", true);
        PoolManager.Instance.poolSwordEmbeddedParticlePool.GetFromPool(
            position,
            Quaternion.Euler(0, 0, 90 * transform.localScale.x),
            new Vector3(transform.localScale.x * 0.6f, transform.localScale.y, transform.localScale.z)
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Embedded(transform.position - Vector3.right * 0.25f * transform.localScale.x);
        }
        if(collision.tag == "Player")
        {
            isFlying = false;

            rb.isKinematic = false;
            PoolManager.Instance.poolSwordThrowPool.AddToPool(gameObject);
        }
        if(collision.tag == "Enemy")
        {
            BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
            enemy.TakeDamage(attackDamage, transform.localScale.x);

            //Thành sword item và trả game object về pool 
            PoolManager.Instance.poolSwordItem.GetFromPool(
                transform.position + Vector3.down * 0.35f,
                Quaternion.identity,
                transform.localScale
            );
            gameObject.SetActive(false);


        }
    }

}
