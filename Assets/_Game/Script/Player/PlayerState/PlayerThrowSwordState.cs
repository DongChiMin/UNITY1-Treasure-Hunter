using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerThrowSwordState : PlayerBaseState<Player>
{
    float previousGravityScale; 

    public void OnEnter(Player player)
    {
        previousGravityScale = player.rb.gravityScale;
        player.rb.gravityScale = 0.25f;
        player.rb.velocity = Vector2.zero;
        player.ChangeAnim("Throw");
        player.StartCoroutine(WaitForAnimation(player));
    }

    public void OnExecute(Player player)
    {
    }

    public void OnFixedExecute(Player player)
    {
    }

    public void OnExit(Player player)
    {
        player.rb.velocity = Vector2.zero;
        player.rb.gravityScale = previousGravityScale;
    }

    IEnumerator WaitForAnimation(Player player)
    {
        yield return new WaitForEndOfFrame();
        float animLength = player.animator.GetCurrentAnimatorStateInfo(1).length;
        float timer = 0f;
        while (timer < animLength * 0.25f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        GameObject obj = PoolManager.Instance.poolSwordThrowPool.GetFromPool( new Vector2(player.transform.position.x, player.transform.position.y + 0.2f), Quaternion.identity, player.transform.localScale);

        while (timer < animLength*0.75f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        player.haveSword = false;
        player.animator.SetLayerWeight(1, 0);

        if(!Physics2D.Raycast(player.transform.position + Vector3.down * 0.2f, Vector2.down, 0.2f, LayerMask.GetMask("Ground")))
        {
            player.ChangeState(player.fallState);
            yield break;
        }
        else
        {
            player.canDoubleJump = true;
        }
        if (Mathf.Abs(player.input.horizontal) > 0.1f)
        {

            player.ChangeState(player.runState);
            yield break;
        }
        if (player.input.jumpKeyPressed)
        {
            player.ChangeState(player.jumpState);
            yield break;
        }
        player.ChangeState(player.idleState);
    }
}
