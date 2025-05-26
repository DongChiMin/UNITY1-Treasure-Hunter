using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState<PlayerContext>
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

        playerMovement.ChangeAnim("Idle");
    }

    public void OnExecute(PlayerContext player)
    {
        if(Mathf.Abs(playerMovement.input.horizontal) > 0.1f)
        {
            playerStateMachine.ChangeState(playerStateMachine.runState);
            return;
        }
        if(playerMovement.input.jumpKeyPressed)
        {
            playerStateMachine.ChangeState(playerStateMachine.jumpState);
            return;
        }
        if(playerMovement.input.dashKeyPressed && playerMovement.canDash)
        {
            playerStateMachine.ChangeState(playerStateMachine.dashState);
            return;
        }
        if (playerItemPickup.GetHaveSword())
        {
            if (playerMovement.input.throwKeyPressed)
            {
                playerStateMachine.ChangeState(playerStateMachine.throwSwordState);
            }
            if(playerMovement.input.attackKeyPressed && playerCombat.GetCanStartCombo())
            {
                playerStateMachine.ChangeState(playerStateMachine.attack1State);
            }
        }
    }

    public void OnFixedExecute(PlayerContext player)
    {

    }

    public void OnExit(PlayerContext player)
    {

    }
}
