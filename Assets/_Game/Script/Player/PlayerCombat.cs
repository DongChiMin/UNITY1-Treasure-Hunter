using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private PlayerMovement player;

    [Header("Stats")]
    [SerializeField] private float startComboCooldown;
    [SerializeField] private int attackDamage;
    [SerializeField] private float hitForce;

    [Header("Debug")]
    [SerializeField] private PlayerContext playerContext;
    [SerializeField] private bool canStartCombo;
    [SerializeField] private bool canStartAirCombo;
    [SerializeField] private bool imortal;

    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;

    private void Start()
    {
        playerContext = GetComponent<PlayerContext>();
        playerMovement = playerContext.playerMovement;
        playerStateMachine = playerContext.playerStateMachine;

        OnInit();
    }

    void OnInit()
    {
        canStartCombo = true;
        canStartAirCombo = true;
        imortal = false;
    }

    void LateUpdate()
    {
        ////Phát hiện enemy trong tầm đánh bằng OverlapBox, chỉ chạy 1 lần mỗi khi đổi state
        //if(playerStateMachine.GetCurrentState() != previousState)
        //{
        //    previousState = playerStateMachine.GetCurrentState();
        //    Collider2D[] hitEnemies = new Collider2D[0];

        //    //Attack va AirAttack co AttackBox khac nhau
        //    if (previousState == playerStateMachine.attack1State || previousState == playerStateMachine.attack2State || previousState == playerStateMachine.attack3State)
        //    {
        //        hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(1.5f, 0.8f), 0f, enemyLayer);
        //    }
        //    else if(previousState == playerStateMachine.airAttack1State || previousState == playerStateMachine.airAttack2State)
        //    {
        //        hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position + Vector3.down * 0.5f, new Vector2(1.65f, 0.8f), 0f, enemyLayer);
        //    }

        //    //Duyet enemy
        //    foreach (Collider2D enemy in hitEnemies)
        //    {
        //        enemy.GetComponent<BaseEnemy>().TakeDamage(attackDamage, player.transform.localScale.x);
        //    }
        //}
 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(attackPoint.position, new Vector2(1.5f, 0.8f));
        Gizmos.DrawWireCube(attackPoint.position + Vector3.down * 0.5f, new Vector2(1.65f, 0.8f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            playerStateMachine.ChangeState(playerStateMachine.hitState);

            playerMovement.rb.velocity = Vector2.zero;
            playerMovement.rb.AddForce(Vector2.up * hitForce, ForceMode2D.Impulse);
        }
    }

    public bool GetCanStartAirCombo()
    {
        return canStartAirCombo;
    }

    public void SetCanStartAirCombo(bool newBool)
    {
        canStartAirCombo = newBool;
    }

    public bool GetCanStartCombo()
    {
        return canStartCombo;
    }

    public void SetCanStartCombo(bool newBool)
    {
        canStartCombo = newBool;
    }

    public bool GetImortal()
    {
        return imortal;
    }

    public void SetImortal(bool newBool)
    {
        imortal = newBool;
    }

    public float GetStartComboCooldown()
    {
        return startComboCooldown;
    }
}
