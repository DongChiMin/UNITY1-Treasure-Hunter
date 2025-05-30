using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyBullet : MonoBehaviour
{
    //Script này cần sử dụng ObjectPooling

    [SerializeField] private float flySpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Debug")]
    [SerializeField] private int direction;
    private void OnEnable()
    {
        FetchFly();
    }

    private void Update()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if(collision.tag == "Ground" || collision.tag == "Player")
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        animator.Play("Explosion");
        rb.velocity = Vector2.zero;
        FragmentsEffect fragmentsEffect = GetComponent<FragmentsEffect>();
        if(fragmentsEffect != null)
        {
            fragmentsEffect.Explode();
        }
        else
        {
            Debug.LogWarning("Không getComponent được FragmentEffect");
        }
    }

    public void DestroyBullet()
    {
        PoolManager.Instance.poolCannonBall.AddToPool(gameObject);
    }

    public void SetDirection(int newDirection)
    {
        direction = newDirection;
        FetchFly();
    }

    private void FetchFly()
    {
        rb.velocity = new Vector2(flySpeed * direction, 0);
    }
}
