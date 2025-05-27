using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerRunState : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerItemPickup playerItemPickup;
    private PlayerCombat playerCombat;
    private PlayerInput input;

    bool particleOn;
    Coroutine crt;
    public void OnEnter(PlayerContext player)
    {
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerItemPickup = player.playerItemPickup;
        playerCombat = player.playerCombat;
        input = player.playerInput;

        playerMovement.ChangeAnim("Run");
        particleOn = true;
    }

    public void OnExecute(PlayerContext player)
    {
        float horizontal = input.horizontal;

        if (input.jumpKeyPressed)
        {
            playerStateMachine.ChangeState(playerStateMachine.jumpState);
            return;
        }
        if (input.dashKeyPressed && playerMovement.canDash)
        {
            playerStateMachine.ChangeState(playerStateMachine.dashState);
            return;
        }
        if (playerItemPickup.GetHaveSword())
        {
            if (input.throwKeyPressed)
            {
                playerStateMachine.ChangeState(playerStateMachine.throwSwordState);
                return;
            }
            if (input.attackKeyPressed && playerCombat.GetCanStartCombo())
            {
                playerStateMachine.ChangeState(playerStateMachine.attack1State);
            }
        }
        if (Mathf.Abs(horizontal) < 0.1f)
        {
            playerStateMachine.ChangeState(playerStateMachine.idleState);
            return;
        }
        if(playerMovement.rb.velocity.y < -0.1f)
        {
            playerStateMachine.ChangeState(playerStateMachine.fallState);
            return;
        }
        playerMovement.FlipPlayer();
        if (particleOn && Mathf.Abs(input.horizontal) > 0.5f)
        {
            particleOn = false;
            crt = playerMovement.StartCoroutine(RunParticleDelay());
        }
    }

    public void OnFixedExecute(PlayerContext player)
    {
        playerMovement.MovePlayer();
    }

    public void OnExit(PlayerContext player)
    {
        playerMovement.rb.velocity = Vector2.zero;
        if(crt != null) playerMovement.StopCoroutine(crt);
    }

    IEnumerator RunParticleDelay()
    {
        float time = 0f;

        time = Random.Range(0.3f, 0.5f);
        yield return new WaitForSeconds(time);

        PoolManager.Instance.poolRun.GetFromPool(
        new Vector2(playerMovement.transform.position.x - 0.2f * playerMovement.transform.localScale.x, playerMovement.transform.position.y),
                    Quaternion.identity,
                    playerMovement.transform.localScale);

        time = Random.Range(0.2f, 0.3f);
        yield return new WaitForSeconds(time);

        PoolManager.Instance.poolRun.GetFromPool(
        new Vector2(playerMovement.transform.position.x - 0.2f * playerMovement.transform.localScale.x, playerMovement.transform.position.y),
                    Quaternion.identity,
                    playerMovement.transform.localScale);

        particleOn = true;

    }
}
