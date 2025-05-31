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
    private DamageDealer playerDamageDealer;
    bool goNextCombo;
    float previousMoveSpeed;

    private Coroutine crt;

    public void OnInit(PlayerContext player)
    {
        //Gán các component
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerCombat = player.playerCombat;
        input = player.playerInput;
        playerDamageDealer = player.playerDamageDealer;
    }
    public void OnEnter()
    {
        //Set các giá trị cho component
        playerMovement.rb.velocity = Vector3.zero;
        playerMovement.ChangeAnim("Attack 1");
        crt = playerMovement.StartCoroutine(WaitForAnimation());

        playerCombat.SetCanStartCombo(false);

        playerDamageDealer.SetAttackBox(true, AttackType.PlayerAttack);

        //Set các biến
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
        if(crt != null)
        {
            playerMovement.StopCoroutine(crt);
        }
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
