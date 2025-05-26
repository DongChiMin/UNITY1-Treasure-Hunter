using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState<PlayerMovement>
{
    public void OnEnter(PlayerMovement player)
    {
        player.imortal = true;
        player.ChangeAnim("Hit");
        player.StartCoroutine(WaitForAnimation(player));
    }

    public void OnExecute(PlayerMovement player)
    {
        player.FlipPlayer();
    }

    public void OnFixedExecute(PlayerMovement player)
    {
        player.MovePlayer();
    }

    public void OnExit(PlayerMovement player)
    {

    }

    IEnumerator WaitForAnimation(PlayerMovement player)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(player.animator.GetCurrentAnimatorStateInfo(1).length);
        player.imortal = false;
        if(player.rb.velocity.y < -0.1f)
        {
            player.ChangeState(player.fallState);
            yield break;
        }
        if(Mathf.Abs(player.rb.velocity.x) > 0.1f)
        {
            player.ChangeState(player.runState);
            yield break;
        }

        player.ChangeState(player.idleState);

    }
}

