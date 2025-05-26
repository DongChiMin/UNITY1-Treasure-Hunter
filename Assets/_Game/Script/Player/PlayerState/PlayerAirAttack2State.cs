using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirAttack2State : PlayerBaseState<PlayerMovement>
{
    float previousGravityScale;
    public void OnEnter(PlayerMovement player)
    {
        previousGravityScale = player.rb.gravityScale;
        player.ChangeAnim("Air Attack 2");
        player.StartCoroutine(WaitForAnimation(player));
        player.rb.gravityScale = 0.25f;
        player.rb.velocity = Vector2.zero;
    }

    public void OnExecute(PlayerMovement player)
    {

    }

    public void OnFixedExecute(PlayerMovement player)
    {

    }

    public void OnExit(PlayerMovement player)
    {
        player.rb.gravityScale = previousGravityScale;
    }

    IEnumerator WaitForAnimation(PlayerMovement player)
    {
        //Particle
        PoolManager.Instance.poolAirAttack1.GetFromPool(
            new Vector2(player.transform.position.x + 1f * player.transform.localScale.x, player.transform.position.y - 0.5f),
            Quaternion.identity,
            player.transform.localScale);

        //Doi 1 frame sau do lay do dai cua animation dang chay
        yield return new WaitForEndOfFrame();
        float animLength = player.animator.GetCurrentAnimatorStateInfo(1).length;

        float timer = 0f;
        while(timer < animLength)
        {
            timer += Time.deltaTime;
            if (player.input.dashKeyPressed)
            {
                player.ChangeState(player.dashState);
                yield break;
            }
            yield return null;
        }
        //Sau 10% animation: neu nhan phim attack thi se go next Combo
        player.ChangeState(player.fallState);
    }
}

