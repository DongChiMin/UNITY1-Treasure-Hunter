using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2State : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerCombat playerCombat;
    private PlayerInput input;
    private DamageDealer playerDamageDealer;

    bool goNextCombo;
    float previousMoveSpeed;

    public void OnInit(PlayerContext player)
    {
        //Gán các component
        this.playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerCombat = player.playerCombat;
        input = player.playerInput;
        playerDamageDealer = player.playerDamageDealer;
    }

    public void OnEnter()
    {
        //Set các giá trị của component
        playerMovement.rb.velocity = Vector3.zero;
        playerMovement.ChangeAnim("Attack 2");
        playerMovement.StartCoroutine(WaitForAnimation());

        playerCombat.SetCanStartCombo(false);

        playerDamageDealer.SetAttackBox(true, AttackType.PlayerAttack);

        //Set giá trị các biến
        previousMoveSpeed = playerMovement.moveSpeed;
        goNextCombo = false;
        playerMovement.moveSpeed = playerMovement.GetMoveSpeedWhenAttacking();
    }

    public void OnExecute()
    {
        playerMovement.FlipPlayer();
    }

    public void OnFixedExecute()
    {
        playerMovement.MovePlayer();
    }

    public void OnExit()
    {
        playerMovement.rb.velocity = Vector2.zero;
        playerMovement.moveSpeed = previousMoveSpeed;

        playerDamageDealer.SetAttackBox(false, AttackType.PlayerAttack);
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
            if (input.attackKeyPressed)
            {
                goNextCombo = true;
            }
            if (input.jumpKeyPressed)
            {
                playerMovement.StartCoroutine(StartComboDelay());
                playerStateMachine.ChangeState(playerStateMachine.jumpState);
                yield break;
            }
            if (input.dashKeyPressed)
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

        if (Mathf.Abs(input.horizontal) > 0.1f)
        {
            playerStateMachine.ChangeState(playerStateMachine.runState);
            yield break;
        }
        playerStateMachine.ChangeState(playerStateMachine.idleState);

    }

    IEnumerator StartComboDelay()
    {
        yield return new WaitForSeconds(playerCombat.GetStartComboCooldown());
        playerCombat.SetCanStartCombo(true);
    }
}

