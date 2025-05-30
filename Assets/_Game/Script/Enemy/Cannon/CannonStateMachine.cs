using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonStateMachine : MonoBehaviour
{
    public CannonIdleState idleState = new CannonIdleState();
    public CannonFireState fireState = new CannonFireState();
    public CannonHitState hitState = new CannonHitState();

    private EnemyBaseState<CannonContext> currentState;
    private CannonContext cannonContext;

    private void Start()
    {
        cannonContext = GetComponent<CannonContext>();

        idleState.OnInit(cannonContext);
        fireState.OnInit(cannonContext);
        hitState.OnInit(cannonContext);
        OnInit();
    }

    private void OnInit()
    {
        ChangeState(idleState);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute();
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedExecute();
        }
    }


    public void ChangeState(EnemyBaseState<CannonContext> newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = newState;
        currentState.OnEnter();
    }

    public void ChangeToHitState()
    {
        ChangeState(hitState);
    }
}
