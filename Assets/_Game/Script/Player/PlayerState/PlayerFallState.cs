using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerFallState : PlayerBaseState<PlayerContext>
{
    LayerMask groundMask;
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerItemPickup playerItemPickup;
    private PlayerCombat playerCombat;
    private PlayerInput input;
    public void OnEnter(PlayerContext player)
    {
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerItemPickup = player.playerItemPickup;
        playerCombat = player.playerCombat;
        input = player.playerInput;

        groundMask = LayerMask.GetMask("Ground");
        playerMovement.ChangeAnim("Fall");
    }

    public void OnExecute(PlayerContext player)
    {
        if(playerMovement.rb.velocity.y < playerMovement.GetMaxFallSpeed())
        {
            playerMovement.rb.velocity = new Vector2(playerMovement.rb.velocity.x, playerMovement.GetMaxFallSpeed());
        }
        if (input.jumpKeyPressed && playerMovement.canDoubleJump)
        {
            playerStateMachine.ChangeState(playerStateMachine.doubleJumpState);
            return;
        }
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
        if (Physics2D.Raycast(player.transform.position + Vector3.down * 0.2f, Vector2.down, 0.2f, groundMask))
        {
            playerStateMachine.ChangeState(playerStateMachine.groundState);
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
        playerMovement.rb.velocity = Vector2.zero;
    }
}
