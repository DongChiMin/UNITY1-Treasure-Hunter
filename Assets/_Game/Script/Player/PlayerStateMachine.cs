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
    [SerializeField] private PlayerContext playerContext;
    [SerializeField] private PlayerBaseState<PlayerContext> currentState;

    private void Start()
    {
        playerContext = GetComponent<PlayerContext>();
        idleState.OnInit(playerContext);
        runState.OnInit(playerContext);
        jumpState.OnInit(playerContext);
        fallState.OnInit(playerContext);
        groundState.OnInit(playerContext);
        doubleJumpState.OnInit(playerContext);
        hitState.OnInit(playerContext);
        throwSwordState.OnInit(playerContext);
        attack1State.OnInit(playerContext);
        attack2State.OnInit(playerContext);
        attack3State.OnInit(playerContext);
        airAttack1State.OnInit(playerContext);
        airAttack2State.OnInit(playerContext);
        dashState.OnInit(playerContext);

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

    public void ChangeState(PlayerBaseState<PlayerContext> newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = newState;
        currentState.OnEnter();
    }

    public PlayerBaseState<PlayerContext> GetCurrentState()
    {
        return currentState;
    }
}
