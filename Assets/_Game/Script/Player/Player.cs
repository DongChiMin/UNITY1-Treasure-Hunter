using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
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
    //--------------------  -------------------//
    //Cac bien thay doi gia tri ngoai class: rb, canDoubleJump, input.horizontal, canStartCombo, canStartAirCombo
    [Header("Editable")]
    [SerializeField] private float jumpForce;
    [SerializeField] public float dashForce;
    public float moveSpeed;
    public float moveSpeedWhenAttacking;
    public float startComboCooldown;
    public float dashCooldown;

    [Header("Public Variable")]
    public bool canDoubleJump;
    public bool canDash;
    public bool canStartCombo;
    public bool canStartAirCombo;
    public bool haveSword;
    public bool imortal;

    [Header("Drag Variable")]
    public Animator animator;
    public Rigidbody2D rb;
    public PlayerInput input;

    private string currentAnim;
    [Header("Debug")]
    public PlayerBaseState<Player> currentState;
    void Start()
    {
        OnInit();    
    }

    void OnInit()
    {
        imortal = false;
        canDash = true;
        canStartCombo = true;
        canStartAirCombo = true;
        canDoubleJump = true;
        ChangeState(idleState);
    }

    private void Update()
    {
        if(currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedExecute(this);
        }
    }

    public void FlipPlayer()
    {
        if(Mathf.Abs(input.horizontal) < 0.1f)
        {
            return;
        }
        if (transform.localScale.x != Mathf.Sign(input.horizontal))
        {
            Vector3 vector = transform.localScale;
            vector.x = Mathf.Sign(input.horizontal);
            transform.localScale = vector;
        }
    }

    public void MovePlayer()
    {
        if (Mathf.Abs(input.horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(input.horizontal * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
    }

    public void JumpPlayer()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void DashPlayer()
    {
        rb.AddForce(Vector2.right * dashForce * transform.localScale.x, ForceMode2D.Impulse);
        //rb.velocity = new Vector2(dashForce * transform.localScale.x, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Sword Destroy" && !haveSword)
        {
            //Chuyển sang hình ảnh cầm kiếm
            animator.SetLayerWeight(1, 1);
            haveSword = true;
            ChangeAnim(currentAnim);

            //Xử lý item kiếm
            Destroy(collision.gameObject);
            
        }
        else if (collision.tag == "Sword No Destroy" & !haveSword)
        {
            //Chuyển sang hình ảnh cầm kiếm
            animator.SetLayerWeight(1, 1);
            haveSword = true;
            ChangeAnim(currentAnim);
        }
        else if (collision.tag == "Water")
        {
            ChangeState(hitState);
            rb.velocity = new Vector2(rb.velocity.x, 12f);
        }
    }

    public void ChangeState(PlayerBaseState<Player> newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        currentState.OnEnter(this);
    }

    public void ChangeAnim(string anim)
    {
        string newAnim = anim;
        if(haveSword)
        {
            newAnim = anim + " Sword";
        }
        if (currentAnim == newAnim)
        {
            return;
        }
        animator.Play(newAnim);
        currentAnim = newAnim;
    }
}
