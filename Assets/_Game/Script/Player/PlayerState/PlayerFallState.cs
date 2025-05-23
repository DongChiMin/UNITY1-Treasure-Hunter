using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerFallState : PlayerBaseState<Player>
{
    LayerMask groundMask;
    public void OnEnter(Player player)
    {
        groundMask = LayerMask.GetMask("Ground");
        player.ChangeAnim("Fall");
    }

    public void OnExecute(Player player)
    {
        if (player.input.jumpKeyPressed && player.canDoubleJump)
        {
            player.ChangeState(player.doubleJumpState);
            return;
        }
        if (player.input.dashKeyPressed && player.canDash)
        {
            player.ChangeState(player.dashState);
            return;
        }
        if (player.haveSword)
        {
            if (player.input.throwKeyPressed)
            {
                player.ChangeState(player.throwSwordState);
                return;
            }
            if (player.input.attackKeyPressed && player.canStartAirCombo)
            {
                player.ChangeState(player.airAttack1State);
            }
        }
        if (Physics2D.Raycast(player.transform.position + Vector3.down * 0.2f, Vector2.down, 0.2f, groundMask))
        {
            player.ChangeState(player.groundState);
            return;
        }
        player.FlipPlayer();
    }

    public void OnFixedExecute(Player player)
    {
        player.MovePlayer();
    }

    public void OnExit(Player player)
    {
        player.rb.velocity = Vector2.zero;
    }
}
