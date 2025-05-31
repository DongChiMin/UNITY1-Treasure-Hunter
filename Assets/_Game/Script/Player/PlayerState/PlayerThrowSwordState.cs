using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerThrowSwordState : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerItemPickup playerItemPickup;
    private PlayerInput input;

    float previousGravityScale;
    private Coroutine crt;

    public void OnInit(PlayerContext player)
    {
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerItemPickup = player.playerItemPickup;
        input = player.playerInput;
    }

    public void OnEnter()
    {
        previousGravityScale = playerMovement.rb.gravityScale;
        playerMovement.rb.gravityScale = 0.25f;
        playerMovement.rb.velocity = Vector2.zero;
        playerMovement.ChangeAnim("Throw");
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
        playerMovement.rb.velocity = Vector2.zero;
        playerMovement.rb.gravityScale = previousGravityScale;
        if(crt != null)
        {
            playerMovement.StopCoroutine(crt);
        }
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForEndOfFrame();
        float animLength = playerMovement.animator.GetCurrentAnimatorStateInfo(1).length;
        float timer = 0f;
        while (timer < animLength * 0.25f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        GameObject obj = PoolManager.Instance.poolSwordThrowPool.GetFromPool( new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y + 0.2f), Quaternion.identity, playerMovement.transform.localScale);

        while (timer < animLength*0.75f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        playerItemPickup.SetHaveSword(false);
        playerMovement.animator.SetLayerWeight(1, 0);

        if(!Physics2D.Raycast(playerMovement.transform.position + Vector3.down * 0.2f, Vector2.down, 0.2f, LayerMask.GetMask("Ground")))
        {
            playerStateMachine.ChangeState(playerStateMachine.fallState);
            yield break;
        }
        else
        {
            playerMovement.canDoubleJump = true;
        }
        if (Mathf.Abs(input.horizontal) > 0.1f)
        {

            playerStateMachine.ChangeState(playerStateMachine.runState);
            yield break;
        }
        if (input.jumpKeyPressed)
        {
            playerStateMachine.ChangeState(playerStateMachine.jumpState);
            yield break;
        }
        playerStateMachine.ChangeState(playerStateMachine.idleState);
    }
}
