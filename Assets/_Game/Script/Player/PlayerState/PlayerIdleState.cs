using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState<Player>
{
    public void OnEnter(Player player)
    {
        player.ChangeAnim("Idle");
    }

    public void OnExecute(Player player)
    {
        if(Mathf.Abs(player.input.horizontal) > 0.1f)
        {
            player.ChangeState(player.runState);
            return;
        }
        if(player.input.jumpKeyPressed)
        {
            player.ChangeState(player.jumpState);
            return;
        }
        if(player.input.dashKeyPressed && player.canDash)
        {
            player.ChangeState(player.dashState);
            return;
        }
        if (player.haveSword)
        {
            if (player.input.throwKeyPressed)
            {
                player.ChangeState(player.throwSwordState);
            }
            if(player.input.attackKeyPressed && player.canStartCombo)
            {
                player.ChangeState(player.attack1State);
            }
        }
    }

    public void OnFixedExecute(Player player)
    {

    }

    public void OnExit(Player player)
    {

    }
}
