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
    private PlayerInput input;

    public void OnInit(PlayerContext player)
    {
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerItemPickup = player.playerItemPickup;
        playerCombat = player.playerCombat;
        input = player.playerInput;
    }

    public void OnEnter()
    {
        PoolManager.Instance.poolJump.GetFromPool(playerMovement.transform.position + Vector3.down * 0.05f, Quaternion.identity, playerMovement.transform.localScale);
        playerMovement.ChangeAnim("Jump");
        playerMovement.canDoubleJump = false;
        playerMovement.JumpPlayer();
    }

    public void OnExecute()
    {
        if (input.dashKeyPressed && playerMovement.canDash)
        {
            playerStateMachine.ChangeState(playerStateMachine.dashState);
            return;
        }
        if (playerItemPickup.GetHaveSword())
        {
            if (input.throwKeyPressed)
            {
                playerStateMachine.ChangeState(playerStateMachine.throwSwordState);
                return;
            }
            if (input.attackKeyPressed && playerCombat.GetCanStartAirCombo())
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

    public void OnFixedExecute()
    {
        playerMovement.MovePlayer();
    }

    public void OnExit()
    {

    }
}

