using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerAirAttack1State : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerCombat playerCombat;
    bool goNextCombo;
    float previousGravityScale;
    public void OnEnter(PlayerContext player)
    {
        this.playerMovement = player.playerMovement;
        this.playerStateMachine = player.playerStateMachine;
        this.playerCombat = player.playerCombat;

        playerCombat.SetCanStartAirCombo(false);
        goNextCombo = false;
        previousGravityScale = playerMovement.rb.gravityScale;
        playerMovement.ChangeAnim("Air Attack 1");
        playerMovement.StartCoroutine(WaitForAnimation());
        playerMovement.rb.gravityScale = 0.25f;
        playerMovement.rb.velocity = Vector2.zero;
    }

    public void OnExecute(PlayerContext player)
    {

    }

    public void OnFixedExecute(PlayerContext player)
    {

    }

    public void OnExit(PlayerContext player)
    {
        playerMovement.rb.gravityScale = previousGravityScale;
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
            if (playerMovement.input.attackKeyPressed)
            {
                goNextCombo = true;
            }
            if (playerMovement.input.dashKeyPressed)
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

