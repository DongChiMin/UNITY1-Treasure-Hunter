using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerAttack1State : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerCombat playerCombat;
    private PlayerInput input;
    bool goNextCombo;
    float previousMoveSpeed;
    public void OnEnter(PlayerContext player)
    {
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerCombat = player.playerCombat;
        input = player.playerInput;

        playerMovement.rb.velocity = Vector3.zero;
        previousMoveSpeed = playerMovement.moveSpeed;
        playerMovement.moveSpeed = playerMovement.GetMoveSpeedWhenAttacking();
        playerCombat.SetCanStartCombo(false);
        goNextCombo = false;
        playerMovement.ChangeAnim("Attack 1");
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
        PoolManager.Instance.poolAttack1.GetFromPool(
        new Vector2(playerMovement.transform.position.x + 1f * playerMovement.transform.localScale.x, playerMovement.transform.position.y),
                    Quaternion.identity,
                    playerMovement.transform.localScale);

        //Doi 1 frame sau do lay do dai cua animation dang chay
        yield return new WaitForEndOfFrame();
        float animLength = playerMovement.animator.GetCurrentAnimatorStateInfo(1).length;
        float timer = 0f;
        
        //Sau 5% animation: neu nhan phim attack thi se go next Combo
        yield return new WaitForSeconds(animLength*0.05f);
        while (timer < animLength*0.95f)
        {
            timer += Time.deltaTime;
            if(input.attackKeyPressed)
            {
                goNextCombo = true;
            }
            if(input.jumpKeyPressed)
            {
                playerMovement.StartCoroutine(StartComboDelay(playerMovement));
                playerStateMachine.ChangeState(playerStateMachine.jumpState);
                yield break;
            }
            if(input.dashKeyPressed)
            {
                playerMovement.StartCoroutine(StartComboDelay(playerMovement));
                playerStateMachine.ChangeState(playerStateMachine.dashState);
                yield break;
            }
            yield return null;
        }
        //Nếu đã nhấn atttack: Chuyển sang attack2
        if (goNextCombo)
        {
            playerStateMachine.ChangeState(playerStateMachine.attack2State);
            yield break;
        }
        //Delay 1 khoang thoi gian moi duoc start Attack tiep
        playerMovement.StartCoroutine(StartComboDelay(playerMovement));

        if (Mathf.Abs(input.horizontal) > 0.1f)
        {
            playerStateMachine.ChangeState(playerStateMachine.runState);
            yield break;
        }
        playerStateMachine.ChangeState(playerStateMachine.idleState);

    }

    IEnumerator StartComboDelay(PlayerMovement player)
    {
        yield return new WaitForSeconds(playerCombat.GetStartComboCooldown());
        playerCombat.SetCanStartCombo(true);
    }
}
