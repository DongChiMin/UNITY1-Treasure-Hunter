using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerAirAttack1State : PlayerBaseState<Player>
{
    bool goNextCombo;
    float previousGravityScale;
    public void OnEnter(Player player)
    {
        player.canStartAirCombo = false;
        goNextCombo = false;
        previousGravityScale = player.rb.gravityScale;
        player.ChangeAnim("Air Attack 1");
        player.StartCoroutine(WaitForAnimation(player));
        player.rb.gravityScale = 0.25f;
        player.rb.velocity = Vector2.zero;
    }

    public void OnExecute(Player player)
    {

    }

    public void OnFixedExecute(Player player)
    {

    }

    public void OnExit(Player player)
    {
        player.rb.gravityScale = previousGravityScale;
    }

    IEnumerator WaitForAnimation(Player player)
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

        //Sau 10% animation: neu nhan phim attack thi se go next Combo
        yield return new WaitForSeconds(animLength * 0.1f);
        while (timer < animLength * 0.9f)
        {
            timer += Time.deltaTime;
            if (player.input.attackKeyPressed)
            {
                goNextCombo = true;
            }
            if (player.input.dashKeyPressed)
            {
                player.ChangeState(player.dashState);
                yield break;
            }
            yield return null;
        }

        if (goNextCombo)
        {
            player.ChangeState(player.airAttack2State);
            yield break;
        }
        player.ChangeState(player.fallState);
    }
}

