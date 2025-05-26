using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirAttack2State : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;

    float previousGravityScale;
    public void OnEnter(PlayerContext player)
    {
        this.playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;

        previousGravityScale = playerMovement.rb.gravityScale;
        playerMovement.ChangeAnim("Air Attack 2");
        playerMovement.StartCoroutine(WaitForAnimation());
        playerMovement.rb.gravityScale = 0.25f;
        playerMovement.rb.velocity = Vector2.zero;
    }

    public void OnExecute(PlayerContext player)
    {

    }

    public void OnFixedExecute(PlayerContext player)
    {

    }

    public void OnExit(PlayerContext player)
    {
        playerMovement.rb.gravityScale = previousGravityScale;
    }

    IEnumerator WaitForAnimation()
    {
        //Particle
        PoolManager.Instance.poolAirAttack1.GetFromPool(
            new Vector2(playerMovement.transform.position.x + 1f * playerMovement.transform.localScale.x, playerMovement.transform.position.y - 0.5f),
            Quaternion.identity,
            playerMovement.transform.localScale);

        //Doi 1 frame sau do lay do dai cua animation dang chay
        yield return new WaitForEndOfFrame();
        float animLength = playerMovement.animator.GetCurrentAnimatorStateInfo(1).length;

        float timer = 0f;
        while(timer < animLength)
        {
            timer += Time.deltaTime;
            if (playerMovement.input.dashKeyPressed)
            {
                playerStateMachine.ChangeState(playerStateMachine.dashState);
                yield break;
            }
            yield return null;
        }
        //Sau 10% animation: neu nhan phim attack thi se go next Combo
        playerStateMachine.ChangeState(playerStateMachine.fallState);
    }
}

