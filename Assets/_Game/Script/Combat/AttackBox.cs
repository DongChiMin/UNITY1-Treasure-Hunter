using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField] private AttackType attackType;
    [SerializeField] private float damageAmount;
    [SerializeField] private float knockbackForce;
    [SerializeField] private GameObject owner;

    [Header("Collision Filtering")]
    [SerializeField] private List<string> validTags; // VD: "Enemy", "Destructible"
    [SerializeField] private List<string> ignoreTags; // VD: "Untagged", "Projectile"
    [SerializeField] private LayerMask targetLayers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check tag
        if (ignoreTags.Contains(collision.tag)) return;
        if (validTags.Count > 0 && !validTags.Contains(collision.tag)) return;

        // Check layermask
        int objLayer = collision.gameObject.layer;
        if ((targetLayers.value & (1 << objLayer)) == 0) return;

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Vector2 dir = (collision.transform.position - owner.transform.position).normalized;
            DamageData data = new DamageData
            {
                damageAmount = this.damageAmount,
                knockbackDirectionRight = dir.x > 0 ? true : false, //true --> Knockback sang bên phải 
                knockbackForce = this.knockbackForce,
                source = owner
            };

            damageable.TakeDamage(data);
        }
    }

    public AttackType GetAttackType()
    {
        return attackType;
    }
}
