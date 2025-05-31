using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirAttack2State : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerInput input;
    private DamageDealer playerDamageDealer;

    float previousGravityScale;
    private Coroutine crt;

    public void OnInit(PlayerContext player)
    {
        //Gán các component
        this.playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        input = player.playerInput;
        playerDamageDealer = player.playerDamageDealer;
    }
    public void OnEnter()
    {
        //Set các giá trị component 
        playerMovement.ChangeAnim("Air Attack 2");
        crt = playerMovement.StartCoroutine(WaitForAnimation());
        playerMovement.rb.velocity = Vector2.zero;

        playerDamageDealer.SetAttackBox(true, AttackType.PlayerAirAttack);

        //Set giá trị các biến
        previousGravityScale = playerMovement.rb.gravityScale;
        playerMovement.rb.gravityScale = 0.25f;
    }

    public void OnExecute()
    {

    }

    public void OnFixedExecute()
    {

    }

    public void OnExit()
    {
        playerMovement.rb.gravityScale = previousGravityScale;

        playerDamageDealer.SetAttackBox(false, AttackType.PlayerAirAttack);
        if(crt != null)
        {
            playerMovement.StopCoroutine(crt);
        }
    }

    IEnumerator WaitForAnimation()
    {
        //Particle
        PoolManager.Instance.poolAirAttack1.GetFromPool(
            new Vector2(playerMovement.transform.position.x + 1f * playerMovement.transform.localScale.x, playerMovement.transform.position.y - 0.5f),
            Quaternion.identity,
            playerMovement.transform.localScale);

        //Doi 1 frame sau do lay do dai cua animation dang chay
        yield return new WaitForEndOfFrame();
        float animLength = playerMovement.animator.GetCurrentAnimatorStateInfo(1).length;

        float timer = 0f;
        while(timer < animLength)
        {
            timer += Time.deltaTime;
            if (input.dashKeyPressed)
            {
                playerStateMachine.ChangeState(playerStateMachine.dashState);
                yield break;
            }
            yield return null;
        }
        //Sau 10% animation: neu nhan phim attack thi se go next Combo
        playerStateMachine.ChangeState(playerStateMachine.fallState);
    }
}

