using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack3State : PlayerBaseState<Player>
{
    float previousMoveSpeed;
    public void OnEnter(Player player)
    {
        player.rb.velocity = Vector3.zero;
        previousMoveSpeed = player.moveSpeed;
        player.moveSpeed = player.moveSpeedWhenAttacking;
        player.canStartCombo = false;
        player.ChangeAnim("Attack 3");
        player.StartCoroutine(WaitForAnimation(player));
    }

    public void OnExecute(Player player)
    {
        player.FlipPlayer();
    }

    public void OnFixedExecute(Player player)
    {
        player.MovePlayer();
    }

    public void OnExit(Player player)
    {
        player.rb.velocity = Vector2.zero;
        player.moveSpeed = previousMoveSpeed;
    }

    IEnumerator WaitForAnimation(Player player)
    {
        //Particle
        PoolManager.Instance.poolAttack3.GetFromPool(
        new Vector2(player.transform.position.x + 1f * player.transform.localScale.x, player.transform.position.y),
                    Quaternion.identity,
                    player.transform.localScale);

        //Doi 1 frame sau do lay do dai cua animation dang chay
        yield return new WaitForEndOfFrame();
        float animLength = player.animator.GetCurrentAnimatorStateInfo(1).length;
        float timer = 0f;
        //Sau animation: Chuyen sang state khac
        while(timer < animLength)
        {
            timer += Time.deltaTime;
            if (player.input.jumpKeyPressed)
            {
                player.StartCoroutine(StartComboDelay(player));
                player.ChangeState(player.jumpState);
                yield break;
            }
            if (player.input.dashKeyPressed)
            {
                player.StartCoroutine(StartComboDelay(player));
                player.ChangeState(player.dashState);
                yield break;
            }
            yield return null;
        }

        //Delay 1 khoang thoi gian moi duoc start Attack tiep
        player.StartCoroutine(StartComboDelay(player));

        if (Mathf.Abs(player.input.horizontal) > 0.1f)
        {
            player.ChangeState(player.runState);
            yield break;
        }
        player.ChangeState(player.idleState);
    }

    IEnumerator StartComboDelay(Player player)
    {
        yield return new WaitForSeconds(player.startComboCooldown);
        player.canStartCombo = true;
    }
}

