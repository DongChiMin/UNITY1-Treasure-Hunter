using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState<PlayerMovement>
{
    float previousGravityScale;
    float dashEndTime;
    public void OnEnter(PlayerMovement player)
    {
        previousGravityScale = player.rb.gravityScale;
        dashEndTime = Time.time + 0.25f;

        player.ChangeAnim("Fall");

        player.rb.velocity = Vector2.zero;
        player.rb.gravityScale = 0f;
        player.canDash = false;
        player.DashPlayer();
        player.StartCoroutine(GhostDashEffect(player));
    }

    public void OnExecute(PlayerMovement player)
    {
        if(Time.time >= dashEndTime)
        {
            if (player.rb.velocity.y < -0.1f)
            {
                player.ChangeState(player.fallState);
                return;
            }
            player.ChangeState(player.runState);
        }
    }

    public void OnFixedExecute(PlayerMovement player)
    {

    }

    public void OnExit(PlayerMovement player)
    {
        player.rb.gravityScale = previousGravityScale;
        player.StartCoroutine(CanDashReset(player));
    }

    IEnumerator CanDashReset(PlayerMovement player)
    {
        yield return new WaitForSeconds(player.dashCooldown);
        player.canDash = true;
    }

    IEnumerator GhostDashEffect(PlayerMovement player)
    {
        int cnt = 1;
        while(cnt <= 5)
        {
            cnt++;
            yield return new WaitForSeconds(0.25f / 5);
            PoolManager.Instance.poolGhost.GetFromPool(player.transform.position, Quaternion.identity, player.transform.localScale);
        }
            
    }
}
