using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHitState : EnemyBaseState<CannonContext>
{
    private CannonAnimationController cannonAnimationController;
    private CannonStateMachine cannonStateMachine;
    public void OnInit(CannonContext cannonContext)
    {
        cannonAnimationController = cannonContext.cannonAnimationController;
        cannonStateMachine = cannonContext.cannonStateMachine;
    }

    public void OnEnter()
    {
        cannonAnimationController.ChangeAnim("Hit");
        cannonStateMachine.StartCoroutine(WaitForAnimationEnd());
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
        float animLength = cannonAnimationController.animator.GetCurrentAnimatorStateInfo(0).length; ;

        //Sau 5% animation: neu nhan phim attack thi se go next Combo
        yield return new WaitForSeconds(animLength);

        //Khi đã chạy hết animation và canon chưa hết máu 
        if (!cannonStateMachine.GetIsDied())
        {
            cannonStateMachine.ChangeState(cannonStateMachine.idleState);
        }

    }
}
