using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerItemPickup playerItemPickup;
    public PlayerStateMachine playerStateMachine;
    public PlayerCombat playerCombat;
    public PlayerInput playerInput;
}
