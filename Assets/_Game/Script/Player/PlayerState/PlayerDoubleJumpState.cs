using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerDoubleJumpState : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerItemPickup playerItemPickup;
    private PlayerCombat playerCombat;

    public void OnEnter(PlayerContext player)
    {
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerItemPickup = player.playerItemPickup;
        playerCombat = player.playerCombat;

        PoolManager.Instance.poolJump.GetFromPool(player.transform.position + Vector3.down * 0.05f, Quaternion.identity, player.transform.localScale);
        playerMovement.ChangeAnim("Jump");
        playerMovement.canDoubleJump = false;
        playerMovement.JumpPlayer();
    }

    public void OnExecute(PlayerContext player)
    {
        if (playerMovement.input.dashKeyPressed && playerMovement.canDash)
        {
            playerStateMachine.ChangeState(playerStateMachine.dashState);
            return;
        }
        if (playerItemPickup.GetHaveSword())
        {
            if (playerMovement.input.throwKeyPressed)
            {
                playerStateMachine.ChangeState(playerStateMachine.throwSwordState);
                return;
            }
            if (playerMovement.input.attackKeyPressed && playerCombat.GetCanStartAirCombo())
            {
                playerStateMachine.ChangeState(playerStateMachine.airAttack1State);
            }
        }
        if (playerMovement.rb.velocity.y < -0.1f)
        {
            playerStateMachine.ChangeState(playerStateMachine.fallState);
            return;
        }
        playerMovement.FlipPlayer();
    }

    public void OnFixedExecute(PlayerContext player)
    {
        playerMovement.MovePlayer();
    }

    public void OnExit(PlayerContext player)
    {

    }
}

