using UnityEngine;

public class PlayerIdleState : PlayerBaseState<PlayerContext>
{
    private PlayerMovement playerMovement;
    private PlayerStateMachine playerStateMachine;
    private PlayerItemPickup playerItemPickup;
    private PlayerCombat playerCombat;
    private PlayerInput input;

    public void OnInit(PlayerContext player)
    {
        playerMovement = player.playerMovement;
        playerStateMachine = player.playerStateMachine;
        playerItemPickup = player.playerItemPickup;
        playerCombat = player.playerCombat;
        input = player.playerInput;
    }
    public void OnEnter()
    {
        playerMovement.ChangeAnim("Idle");
    }

    public void OnExecute()
    {
        //Nhin xuong neu nguoi choi nhan phim s
        if (input.lookDownKeyPressed)
        {
            CameraManager.Instance.LookDownCamera(playerMovement.GetLookDownDistance());
        }
        else
        {
            CameraManager.Instance.LookNormalCamera();
        }
        if (Mathf.Abs(input.horizontal) > 0.1f)
        {
            playerStateMachine.ChangeState(playerStateMachine.runState);
            return;
        }
        if(input.jumpKeyPressed)
        {
            playerStateMachine.ChangeState(playerStateMachine.jumpState);
            return;
        }
        if(input.dashKeyPressed && playerMovement.canDash)
        {
            playerStateMachine.ChangeState(playerStateMachine.dashState);
            return;
        }
        if (playerItemPickup.GetHaveSword())
        {
            if (input.throwKeyPressed)
            {
                playerStateMachine.ChangeState(playerStateMachine.throwSwordState);
            }
            if(input.attackKeyPressed && playerCombat.GetCanStartCombo())
            {
                playerStateMachine.ChangeState(playerStateMachine.attack1State);
            }
        }
    }

    public void OnFixedExecute()
    {

    }

    public void OnExit()
    {

    }
}
