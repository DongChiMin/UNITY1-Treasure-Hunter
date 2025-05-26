using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerCombat playerCombat;
    public void OnEnter(PlayerContext player)
    {
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerCombat = player.playerCombat;

        playerCombat.SetImortal(true);
        playerMovement.ChangeAnim("Hit");
        player.StartCoroutine(WaitForAnimation());
    }

    public void OnExecute(PlayerContext player)
    {
        playerMovement.FlipPlayer();
    }

    public void OnFixedExecute(PlayerContext player)
    {
        playerMovement.MovePlayer();
    }

    public void OnExit(PlayerContext player)
    {

    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(playerMovement.animator.GetCurrentAnimatorStateInfo(1).length);
        playerCombat.SetImortal(false);
        if(playerMovement.rb.velocity.y < -0.1f)
        {
            playerStateMachine.ChangeState(playerStateMachine.fallState);
            yield break;
        }
        if(Mathf.Abs(playerMovement.rb.velocity.x) > 0.1f)
        {
            playerStateMachine.ChangeState(playerStateMachine.runState);
            yield break;
        }

        playerStateMachine.ChangeState(playerStateMachine.idleState);

    }
}

