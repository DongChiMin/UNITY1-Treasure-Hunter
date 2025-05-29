using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    //Có thể mở rộng: Thêm biến blocking (đỡ đòn, phản đòn trong hàm takeDamage), giáp, gọi hiệu ứng 
    [Header("Attributes")]
    [SerializeField] private float maxHP;

    [Header("Knockback")]
    [SerializeField] private float minKnockX = 1f;
    [SerializeField] private float maxKnockX = 2f;
    [SerializeField] private float minKnockY = 4f;
    [SerializeField] private float maxKnockY = 6f;

    [Header("Events")]
    public UnityEvent onDie; // Gắn từ editor tùy vai trò (Player, Enemy...)
    public UnityEvent<float> onHealthReduced;

    [Header("Debug")]
    [SerializeField] private float currentHP;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        OnInit();
    }

    void OnInit()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(DamageData damageData)
    {
        //Giảm máu
        currentHP -= damageData.damageAmount;
        onHealthReduced?.Invoke(currentHP);

        //Hiệu ứng knockback
        ApplyKnockback(damageData);

        //Animation Hit
    

        //Nếu máu <= 0
        if(currentHP <= 0)
        {
            Die();
        }
    }

    private void ApplyKnockback(DamageData damageData)
    {
        if (rb == null)
        {
            Debug.LogWarning("Chưa có RigidBody2D");
            return;
        }
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        float directionX = (damageData.knockbackDirectionRight == true ? 1 : -1) * Random.Range(minKnockX, maxKnockX);
        float directionY = Random.Range(minKnockY, maxKnockY);

        rb.AddForce(new Vector2(directionX * damageData.knockbackForce, directionY), ForceMode2D.Impulse);
    }


    private void Die()
    {
        onDie?.Invoke();
    }
}
