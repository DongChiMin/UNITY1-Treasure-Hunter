using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    float previousGravityScale;
    float dashEndTime;

    public void OnInit(PlayerContext player)
    {
        this.playerMovement = player.playerMovement;
        this.playerStateMachine = player.playerStateMachine;
    }
    public void OnEnter()
    {
        previousGravityScale = playerMovement.rb.gravityScale;
        dashEndTime = Time.time + 0.25f;

        playerMovement.ChangeAnim("Fall");

        playerMovement.rb.velocity = Vector2.zero;
        playerMovement.rb.gravityScale = 0f;
        playerMovement.canDash = false;
        playerMovement.DashPlayer();
        playerMovement.StartCoroutine(GhostDashEffect());
    }

    public void OnExecute()
    {
        if(Time.time >= dashEndTime)
        {
            if (playerMovement.rb.velocity.y < -0.1f)
            {
                playerStateMachine.ChangeState(playerStateMachine.fallState);
                return;
            }
            playerStateMachine.ChangeState(playerStateMachine.runState);
        }
    }

    public void OnFixedExecute()
    {

    }

    public void OnExit()
    {
        playerMovement.rb.gravityScale = previousGravityScale;
        playerMovement.StartCoroutine(CanDashReset());
    }

    IEnumerator CanDashReset()
    {
        yield return new WaitForSeconds(playerMovement.GetDashCooldown());
        playerMovement.canDash = true;
    }

    IEnumerator GhostDashEffect()
    {
        int cnt = 1;
        while(cnt <= 5)
        {
            cnt++;
            yield return new WaitForSeconds(0.25f / 5);
            PoolManager.Instance.poolGhost.GetFromPool(playerMovement.transform.position, Quaternion.identity, playerMovement.transform.localScale);
        }
            
    }
}
