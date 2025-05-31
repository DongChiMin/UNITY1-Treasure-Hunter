using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerCombat playerCombat;
    private PlayerContext playerContext;

    private Coroutine crt;

    public void OnInit(PlayerContext player)
    {
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerCombat = player.playerCombat;
        playerContext = player;
    }
    public void OnEnter()
    {
        playerCombat.SetImortal(true);
        //Reset biến current Anim để Animation Hit có thể Play lại từ đầu kể cả khi đang chạy dở
        playerMovement.ResetAnim();
        playerMovement.ChangeAnim("Hit");
        crt = playerContext.StartCoroutine(WaitForAnimation());
    }

    public void OnExecute()
    {
        //playerMovement.FlipPlayer();
    }

    public void OnFixedExecute()
    {
        //playerMovement.MovePlayer();
    }

    public void OnExit()
    {
        if(crt != null)
        {
            playerContext.StopCoroutine(crt);
        }
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

