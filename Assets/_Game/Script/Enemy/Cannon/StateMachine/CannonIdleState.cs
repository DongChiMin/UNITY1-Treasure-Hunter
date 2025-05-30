using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonIdleState : EnemyBaseState<CannonContext> 
{
    private EnemyVision enemyVision;
    private CannonStateMachine cannonStateMachine;
    private CannonAttack cannonAttack;
    private CannonAnimationController cannonAnimationController;
    public void OnInit(CannonContext cannonContext)
    {
        enemyVision = cannonContext.enemyVision;
        cannonStateMachine = cannonContext.cannonStateMachine;
        cannonAttack = cannonContext.cannonAttack;
        cannonAnimationController = cannonContext.cannonAnimationController;
    }

    public void OnEnter()
    {
        cannonAnimationController.ChangeAnim("Idle");
    }

    public void OnExecute()
    {
        if (enemyVision.GetDetected() && cannonAttack.GetCanStartAttack())
        {
            cannonStateMachine.ChangeState(cannonStateMachine.fireState);
        }
    }

    public void OnFixedExecute()
    {

    }

    public void OnExit()
    {

    }

}
