using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerAirAttack1State : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerCombat playerCombat;
    private PlayerInput input;
    private DamageDealer playerDamageDealer;

    bool goNextCombo;
    float previousGravityScale;
    private Coroutine crt;

    public void OnInit(PlayerContext player)
    {
        //Gán các component
        this.playerMovement = player.playerMovement;
        this.playerStateMachine = player.playerStateMachine;
        this.playerCombat = player.playerCombat;
        this.input = player.playerInput;
        this.playerDamageDealer = player.playerDamageDealer;
    }
    public void OnEnter()
    {
        //Set giá trị các component
        playerMovement.ChangeAnim("Air Attack 1");
        crt = playerMovement.StartCoroutine(WaitForAnimation());
        playerMovement.rb.velocity = Vector2.zero;

        playerDamageDealer.SetAttackBox(true, AttackType.PlayerAirAttack);

        playerCombat.SetCanStartAirCombo(false);

        //Set giá trị các biến 
        goNextCombo = false;
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

        //Sau 10% animation: neu nhan phim attack thi se go next Combo
        yield return new WaitForSeconds(animLength * 0.1f);
        while (timer < animLength * 0.9f)
        {
            timer += Time.deltaTime;
            if (input.attackKeyPressed)
            {
                goNextCombo = true;
            }
            if (input.dashKeyPressed)
            {
                playerStateMachine.ChangeState(playerStateMachine.dashState);
                yield break;
            }
            yield return null;
        }

        if (goNextCombo)
        {
            playerStateMachine.ChangeState(playerStateMachine.airAttack2State);
            yield break;
        }
        playerStateMachine.ChangeState(playerStateMachine.fallState);
    }
}

