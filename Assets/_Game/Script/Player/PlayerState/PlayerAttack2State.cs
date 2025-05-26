using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2State : PlayerBaseState<PlayerMovement>
{
    bool goNextCombo;
    float previousMoveSpeed;
    public void OnEnter(PlayerMovement player)
    {
        player.rb.velocity = Vector3.zero;
        previousMoveSpeed = player.moveSpeed;
        player.moveSpeed = player.moveSpeedWhenAttacking;
        player.canStartCombo = false;
        goNextCombo = false;
        player.ChangeAnim("Attack 2");
        player.StartCoroutine(WaitForAnimation(player));
    }

    public void OnExecute(PlayerMovement player)
    {
        player.FlipPlayer();
    }

    public void OnFixedExecute(PlayerMovement player)
    {
        player.MovePlayer();
    }

    public void OnExit(PlayerMovement player)
    {
        player.rb.velocity = Vector2.zero;
        player.moveSpeed = previousMoveSpeed;
    }

    IEnumerator WaitForAnimation(PlayerMovement player)
    {
        //Particle
        PoolManager.Instance.poolAttack2.GetFromPool(
        new Vector2(player.transform.position.x + 1f * player.transform.localScale.x, player.transform.position.y),
                    Quaternion.identity,
                    player.transform.localScale);

        //Doi 1 frame sau do lay do dai cua animation dang chay
        yield return new WaitForEndOfFrame();
        float animLength = player.animator.GetCurrentAnimatorStateInfo(1).length;
        float timer = 0f;

        //Sau 10% animation: neu nhan phim attack thi se go next Combo
        yield return new WaitForSeconds(animLength * 0.05f);
        while (timer < animLength * 0.95f)
        {
            timer += Time.deltaTime;
            if (player.input.attackKeyPressed)
            {
                goNextCombo = true;
            }
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

        //Nếu đã nhấn attack: chuyển sang attack3
        if (goNextCombo)
        {
            player.ChangeState(player.attack3State);
            yield break;
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

    IEnumerator StartComboDelay(PlayerMovement player)
    {
        yield return new WaitForSeconds(player.startComboCooldown);
        player.canStartCombo = true;
    }
}

