using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CannonFireState : EnemyBaseState<CannonContext>
{
    private CannonAttack cannonAttack;
    private CannonAnimationController cannonAnimationController;
    private CannonStateMachine cannonStateMachine;
    public void OnInit(CannonContext cannonContext)
    {
        cannonAttack = cannonContext.cannonAttack;
        cannonAnimationController = cannonContext.cannonAnimationController;
        cannonStateMachine = cannonContext.cannonStateMachine;
    }

    public void OnEnter()
    {
        cannonAnimationController.ChangeAnim("Fire");
        cannonAttack.StartCoroutine(WaitForAnimationEnd());
    }

    public void OnExecute()
    {

    }

    public void OnFixedExecute()
    {

    }

    public void OnExit()
    {

    }


    IEnumerator WaitForAnimationEnd()
    {
        //Doi 1 frame sau do lay do dai cua animation dang chay
        yield return new WaitForEndOfFrame();
        float animLength = cannonAnimationController.animator.GetCurrentAnimatorStateInfo(0).length;
        float timer = 0f;

        //Sau khi chạy 70% animation mới bắt đầu bắn đạn;
        while(timer < animLength * 0.7)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        cannonAttack.Attack();

        //Sau 30% animation còn lại: nếu chưa hết máu thì Chuyển về Idle State
        yield return new WaitForSeconds(animLength * 0.3f);
        if(!cannonStateMachine.GetIsDied())
        {
            cannonStateMachine.ChangeState(cannonStateMachine.idleState);
        }
        
    }
}
