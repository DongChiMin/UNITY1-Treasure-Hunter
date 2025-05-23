using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerJumpState : PlayerBaseState<Player>
{
    public void OnEnter(Player player)
    {
        PoolManager.Instance.poolJump.GetFromPool(player.transform.position + Vector3.down*0.05f, Quaternion.identity, player.transform.localScale);
        player.ChangeAnim("Jump");
        player.JumpPlayer();
    }

    public void OnExecute(Player player)
    {
        if(player.input.jumpKeyPressed && player.canDoubleJump)
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
            if(player.input.attackKeyPressed && player.canStartAirCombo)
            {
                player.ChangeState(player.airAttack1State);
            }
        }
        if (player.rb.velocity.y < -0.1f)
        {
            player.ChangeState(player.fallState);
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

    }
}
