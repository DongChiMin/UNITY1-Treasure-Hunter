using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerBaseState<Player>
{
    bool canParticle = true;
    Coroutine crt;
    public void OnEnter(Player player)
    {
        if (canParticle)
        {
            canParticle = false;
            PoolManager.Instance.poolGround.GetFromPool(player.transform.position + Vector3.down * 0.05f, Quaternion.identity, player.transform.localScale);
            player.StartCoroutine(particleDelay());
        }
        player.canDoubleJump = true;
        player.canStartAirCombo = true;
        crt = player.StartCoroutine(WaitForAnimation(player));

    }

    public void OnExecute(Player player)
    {

    }

    public void OnFixedExecute(Player player)
    {

    }

    public void OnExit(Player player)
    {
        if (crt != null) player.StopCoroutine(crt);
    }

    IEnumerator WaitForAnimation(Player player)
    {
        if (Mathf.Abs(player.input.horizontal) > 0.1f)
        {
            player.ChangeState(player.runState);
            yield break;
        }
        player.ChangeAnim("Ground");

        yield return new WaitForEndOfFrame();
        int index = -1;
        if (player.haveSword)
        {
            index = 1;
        }
        else
        {
            index = 0;
        }
        float animLength = player.animator.GetCurrentAnimatorStateInfo(index).length;
        float timer = 0f;
        while(timer < animLength)
        {
            timer+= Time.deltaTime;
            if(Mathf.Abs(player.input.horizontal) > 0.1f)
            {
                player.ChangeState(player.runState);
                yield break;
            }
            if(player.input.jumpKeyPressed)
            {
                player.ChangeState(player.jumpState);
                yield break;
            }
            yield return null;
        }
        player.ChangeState(player.idleState);
    }

    IEnumerator particleDelay()
    {
        yield return new WaitForSeconds(0.1f);
        canParticle = true;
    }
}
