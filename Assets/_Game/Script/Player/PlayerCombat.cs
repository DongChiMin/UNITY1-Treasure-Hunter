using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] Player player;
    [SerializeField] int attackDamage;
    PlayerBaseState<Player> previousState;
    LayerMask enemyLayer;

    private void Start()
    {
        enemyLayer = LayerMask.GetMask("Crates");
        previousState = player.idleState;
    }

    void LateUpdate()
    {
        //Phát hiện enemy trong tầm đánh bằng OverlapBox, chỉ chạy 1 lần mỗi khi đổi state
        if(player.currentState != previousState)
        {
            previousState = player.currentState;
            Collider2D[] hitEnemies = new Collider2D[0];

            //Attack va AirAttack co AttackBox khac nhau
            if (previousState == player.attack1State || previousState == player.attack2State || previousState == player.attack3State)
            {
                hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(1.5f, 0.8f), 0f, enemyLayer);
            }
            else if(previousState == player.airAttack1State || previousState == player.airAttack2State)
            {
                hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position + Vector3.down * 0.5f, new Vector2(1.65f, 0.8f), 0f, enemyLayer);
            }

            //Duyet enemy
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<BaseEnemy>().TakeDamage(attackDamage, player.transform.localScale.x);
            }
        }
 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(attackPoint.position, new Vector2(1.5f, 0.8f));
        Gizmos.DrawWireCube(attackPoint.position + Vector3.down * 0.5f, new Vector2(1.65f, 0.8f));
    }
}
