using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerItemPickup playerItemPickup;
    private PlayerCombat playerCombat;
    private PlayerInput input;

    bool canParticle = true;
    Coroutine crt;

    public void OnInit(PlayerContext player)
    {
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerItemPickup = player.playerItemPickup;
        playerCombat = player.playerCombat;
        input = player.playerInput;
    }
    public void OnEnter()
    {
        if (canParticle)
        {
            canParticle = false;
            PoolManager.Instance.poolGround.GetFromPool(playerMovement.transform.position + Vector3.down * 0.05f, Quaternion.identity, playerMovement.transform.localScale);
            playerMovement.StartCoroutine(particleDelay());
        }
        playerMovement.canDoubleJump = true;
        playerCombat.SetCanStartAirCombo(true);
        crt = playerMovement.StartCoroutine(WaitForAnimation());

    }

    public void OnExecute()
    {

    }

    public void OnFixedExecute()
    {

    }

    public void OnExit()
    {
        if (crt != null) playerMovement.StopCoroutine(crt);
    }

    IEnumerator WaitForAnimation()
    {
        if (Mathf.Abs(input.horizontal) > 0.1f)
        {
            playerStateMachine.ChangeState(playerStateMachine.runState);
            yield break;
        }
        playerMovement.ChangeAnim("Ground");

        yield return new WaitForEndOfFrame();
        int index = -1;
        if (playerItemPickup.GetHaveSword())
        {
            index = 1;
        }
        else
        {
            index = 0;
        }
        float animLength = playerMovement.animator.GetCurrentAnimatorStateInfo(index).length;
        float timer = 0f;
        while(timer < animLength)
        {
            timer+= Time.deltaTime;
            if(Mathf.Abs(input.horizontal) > 0.1f)
            {
                playerStateMachine.ChangeState(playerStateMachine.runState);
                yield break;
            }
            if(input.jumpKeyPressed)
            {
                playerStateMachine.ChangeState(playerStateMachine.jumpState);
                yield break;
            }
            yield return null;
        }
        playerStateMachine.ChangeState(playerStateMachine.idleState);
    }

    IEnumerator particleDelay()
    {
        yield return new WaitForSeconds(0.1f);
        canParticle = true;
    }
}
