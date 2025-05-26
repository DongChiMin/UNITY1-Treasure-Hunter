using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    //--------------------Player States-------------------//
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerRunState runState = new PlayerRunState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerFallState fallState = new PlayerFallState();
    public PlayerGroundState groundState = new PlayerGroundState();
    public PlayerDoubleJumpState doubleJumpState = new PlayerDoubleJumpState();
    public PlayerHitState hitState = new PlayerHitState();
    public PlayerThrowSwordState throwSwordState = new PlayerThrowSwordState();
    public PlayerAttack1State attack1State = new PlayerAttack1State();
    public PlayerAttack2State attack2State = new PlayerAttack2State();
    public PlayerAttack3State attack3State = new PlayerAttack3State();
    public PlayerAirAttack1State airAttack1State = new PlayerAirAttack1State();
    public PlayerAirAttack2State airAttack2State = new PlayerAirAttack2State();
    public PlayerDashState dashState = new PlayerDashState();

    [Header("Debug")]
    [SerializeField] PlayerContext playerContext;
    public PlayerBaseState<PlayerContext> currentState;

    private void Start()
    {
        playerContext = GetComponent<PlayerContext>();
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
            currentState.OnExecute(playerContext);
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedExecute(playerContext);
        }
    }

    public void ChangeState(PlayerBaseState<PlayerContext> newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(playerContext);
        }
        currentState = newState;
        currentState.OnEnter(playerContext);
    }
}
