using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerRunState : PlayerBaseState<PlayerMovement>
{
    bool particleOn;
    Coroutine crt;
    public void OnEnter(PlayerMovement player)
    {
        player.ChangeAnim("Run");
        particleOn = true;
    }

    public void OnExecute(PlayerMovement player)
    {
        float horizontal = player.input.horizontal;

        if (player.input.jumpKeyPressed)
        {
            player.ChangeState(player.jumpState);
            return;
        }
        if (player.input.dashKeyPressed && player.canDash)
        {
            player.ChangeState(player.dashState);
            return;
        }
        if (player.haveSword)
        {
            if (player.input.throwKeyPressed)
            {
                player.ChangeState(player.throwSwordState);
                return;
            }
            if (player.input.attackKeyPressed && player.canStartCombo)
            {
                player.ChangeState(player.attack1State);
            }
        }
        if (Mathf.Abs(horizontal) < 0.1f)
        {
            player.ChangeState(player.idleState);
            return;
        }
        if(player.rb.velocity.y < -0.1f)
        {
            player.ChangeState(player.fallState);
            return;
        }
        player.FlipPlayer();
        if (particleOn && Mathf.Abs(player.input.horizontal) > 0.5f)
        {
            particleOn = false;
            crt = player.StartCoroutine(RunParticleDelay(player));
        }
    }

    public void OnFixedExecute(PlayerMovement player)
    {
        player.MovePlayer();
    }

    public void OnExit(PlayerMovement player)
    {
        player.rb.velocity = Vector2.zero;
        if(crt != null) player.StopCoroutine(crt);
    }

    IEnumerator RunParticleDelay(PlayerMovement player)
    {
        float time = 0f;

        time = Random.Range(0.3f, 0.5f);
        yield return new WaitForSeconds(time);

        PoolManager.Instance.poolRun.GetFromPool(
        new Vector2(player.transform.position.x - 0.2f * player.transform.localScale.x, player.transform.position.y),
                    Quaternion.identity,
                    player.transform.localScale);

        time = Random.Range(0.2f, 0.3f);
        yield return new WaitForSeconds(time);

        PoolManager.Instance.poolRun.GetFromPool(
        new Vector2(player.transform.position.x - 0.2f * player.transform.localScale.x, player.transform.position.y),
                    Quaternion.identity,
                    player.transform.localScale);

        particleOn = true;

    }
}
