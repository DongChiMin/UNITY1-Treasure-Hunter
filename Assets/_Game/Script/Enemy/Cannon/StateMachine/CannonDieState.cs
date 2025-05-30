using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonDieState : EnemyBaseState<CannonContext>
{
    private CannonAnimationController cannonAnimationController;
    private FragmentsEffect fragmentsEffect;
    public void OnInit(CannonContext cannonContext)
    {
        cannonAnimationController = cannonContext.cannonAnimationController;
        fragmentsEffect = cannonContext.fragmentsEffect;
    }

    public void OnEnter()
    {
        cannonAnimationController.ChangeAnim("Destroyed");
        fragmentsEffect.Explode();
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
}
