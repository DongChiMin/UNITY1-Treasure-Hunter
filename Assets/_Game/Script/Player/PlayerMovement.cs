using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerMovement : MonoBehaviour
{
    //--------------------  -------------------//
    //Cac bien thay doi gia tri ngoai class: rb, canDoubleJump, input.horizontal, canStartCombo, canStartAirCombo
    [Header("Editable")]
    [SerializeField] private float jumpForce;
    public float maxFallSpeed;
    [SerializeField] public float dashForce;
    public float moveSpeed;
    public float moveSpeedWhenAttacking;
    public float dashCooldown;
    public float lookDownDistance;

    [Header("Drag Variable")]
    public Animator animator;
    public Rigidbody2D rb;
    public PlayerInput input;

    private string currentAnim;

    [Header("Debug")] 
    [SerializeField] private PlayerContext playerContext;
    public bool canDoubleJump;
    public bool canDash;
    public bool isLookDown;
    void Start()
    {
        OnInit();    
    }

    void OnInit()
    {
        canDash = true;
        canDoubleJump = true;
        isLookDown = false;
    }

    private void Update()
    {
        if(input.lookDownKeyPressed)
        {
            isLookDown = true;
        }
        else
        {
            isLookDown = false;
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

    public void ChangeAnim(string anim)
    {
        string newAnim = anim;
        if(playerContext.playerItemPickup.GetHaveSword())
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

    public void ResetAnim()
    {
        animator.Play(currentAnim);
    }
}
