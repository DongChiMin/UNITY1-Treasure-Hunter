using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerMovement : MonoBehaviour
{
    //--------------------  -------------------//
    //Các biến private chỉ có getter. biến public sẵn getter và setter
    [Header("Editable")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float dashForce;
    [SerializeField] private float moveSpeedWhenAttacking;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float lookDownDistance;
    public float moveSpeed;

    [Header("Drag Variable")]
    public Animator animator;
    public Rigidbody2D rb;

    [Header("Debug")] 
    [SerializeField] private PlayerContext playerContext;
    [SerializeField] private PlayerInput input;
    public bool canDoubleJump;
    public bool canDash;

    private string currentAnim;

    void Start()
    {
        playerContext = GetComponent<PlayerContext>();
        input = playerContext.playerInput;
        OnInit();    
    }

    void OnInit()
    {
        canDash = true;
        canDoubleJump = true;
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

    public void ResetSwordAnim()
    {
        ChangeAnim(currentAnim);
    }

    public float GetLookDownDistance()
    {
        return lookDownDistance;
    }
    public float GetDashCooldown()
    {
        return dashCooldown;
    }
    public float GetMoveSpeedWhenAttacking()
    {
        return moveSpeedWhenAttacking;
    }
    public float GetMaxFallSpeed()
    {
        return maxFallSpeed;
    }
}
