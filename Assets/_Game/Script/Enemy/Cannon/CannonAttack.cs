using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private bool canStartAttack;

    private void Start()
    {
        canStartAttack = true;
    }
    public void Attack()
    {
        canStartAttack=false;
        StartCoroutine(Cooldown(attackCooldown));

        //Xử lý attack : Gọi từ pool viên đạn và effect
        EnemyBullet bullet = PoolManager.Instance.poolCannonBall.GetFromPool(transform.position, Quaternion.identity, Vector3.one).GetComponent<EnemyBullet>();
        bullet.SetDirection((int) Mathf.Sign(transform.localScale.x) * -1);

        PoolManager.Instance.poolCannonFireEffect.GetFromPool(transform.position + new Vector3(transform.localScale.x * 0.7f * -1, 0, 0), Quaternion.identity, transform.localScale);
    }

    IEnumerator Cooldown(float attackCooldown)
    {
        yield return new WaitForSeconds(attackCooldown);
        canStartAttack = true;
    }

    public bool GetCanStartAttack()
    {
        return canStartAttack;
    }

    public void SetCanStartAttack(bool newBool)
    {
        canStartAttack = newBool;
    }
}
