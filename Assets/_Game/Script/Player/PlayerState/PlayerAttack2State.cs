using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2State : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerCombat playerCombat;

    bool goNextCombo;
    float previousMoveSpeed;
    public void OnEnter(PlayerContext player)
    {
        this.playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerCombat = player.playerCombat;

        playerMovement.rb.velocity = Vector3.zero;
        previousMoveSpeed = playerMovement.moveSpeed;
        playerMovement.moveSpeed = playerMovement.moveSpeedWhenAttacking;
        playerCombat.SetCanStartCombo(false);
        goNextCombo = false;
        playerMovement.ChangeAnim("Attack 2");
        playerMovement.StartCoroutine(WaitForAnimation());
    }

    public void OnExecute(PlayerContext player)
    {
        playerMovement.FlipPlayer();
    }

    public void OnFixedExecute(PlayerContext player)
    {
        playerMovement.MovePlayer();
    }

    public void OnExit(PlayerContext player)
    {
        playerMovement.rb.velocity = Vector2.zero;
        playerMovement.moveSpeed = previousMoveSpeed;
    }

    IEnumerator WaitForAnimation()
    {
        //Particle
        PoolManager.Instance.poolAttack2.GetFromPool(
        new Vector2(playerMovement.transform.position.x + 1f * playerMovement.transform.localScale.x, playerMovement.transform.position.y),
                    Quaternion.identity,
                    playerMovement.transform.localScale);

        //Doi 1 frame sau do lay do dai cua animation dang chay
        yield return new WaitForEndOfFrame();
        float animLength = playerMovement.animator.GetCurrentAnimatorStateInfo(1).length;
        float timer = 0f;

        //Sau 10% animation: neu nhan phim attack thi se go next Combo
        yield return new WaitForSeconds(animLength * 0.05f);
        while (timer < animLength * 0.95f)
        {
            timer += Time.deltaTime;
            if (playerMovement.input.attackKeyPressed)
            {
                goNextCombo = true;
            }
            if (playerMovement.input.jumpKeyPressed)
            {
                playerMovement.StartCoroutine(StartComboDelay());
                playerStateMachine.ChangeState(playerStateMachine.jumpState);
                yield break;
            }
            if (playerMovement.input.dashKeyPressed)
            {
                playerMovement.StartCoroutine(StartComboDelay());
                playerStateMachine.ChangeState(playerStateMachine.dashState);
                yield break;
            }
            yield return null;
        }

        //Nếu đã nhấn attack: chuyển sang attack3
        if (goNextCombo)
        {
            playerStateMachine.ChangeState(playerStateMachine.attack3State);
            yield break;
        }

        //Delay 1 khoang thoi gian moi duoc start Attack tiep
        playerMovement.StartCoroutine(StartComboDelay());

        if (Mathf.Abs(playerMovement.input.horizontal) > 0.1f)
        {
            playerStateMachine.ChangeState(playerStateMachine.runState);
            yield break;
        }
        playerStateMachine.ChangeState(playerStateMachine.idleState);

    }

    IEnumerator StartComboDelay()
    {
        yield return new WaitForSeconds(playerCombat.startComboCooldown);
        playerCombat.SetCanStartCombo(true);
    }
}

