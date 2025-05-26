using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerItemPickup playerItemPickup;
    private PlayerCombat playerCombat;

    bool canParticle = true;
    Coroutine crt;
    public void OnEnter(PlayerContext player)
    {
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerItemPickup = player.playerItemPickup;
        playerCombat = player.playerCombat;

        if (canParticle)
        {
            canParticle = false;
            PoolManager.Instance.poolGround.GetFromPool(player.transform.position + Vector3.down * 0.05f, Quaternion.identity, player.transform.localScale);
            player.StartCoroutine(particleDelay());
        }
        playerMovement.canDoubleJump = true;
        playerCombat.SetCanStartAirCombo(true);
        crt = player.StartCoroutine(WaitForAnimation());

    }

    public void OnExecute(PlayerContext player)
    {

    }

    public void OnFixedExecute(PlayerContext player)
    {

    }

    public void OnExit(PlayerContext player)
    {
        if (crt != null) player.StopCoroutine(crt);
    }

    IEnumerator WaitForAnimation()
    {
        if (Mathf.Abs(playerMovement.input.horizontal) > 0.1f)
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
            if(Mathf.Abs(playerMovement.input.horizontal) > 0.1f)
            {
                playerStateMachine.ChangeState(playerStateMachine.runState);
                yield break;
            }
            if(playerMovement.input.jumpKeyPressed)
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
