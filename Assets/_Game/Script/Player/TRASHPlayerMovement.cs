using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRASHPlayerMovement : MonoBehaviour
{

    //-------------------------------------Cac enum State player-------------------------------------//
    enum PlayerState
    {
        None,
        Idle,
        Run,
        Jump,
        DoubleJump,
        Fall,
        Ground,
        Attack1,
        Attack2,
        Attack3,
        Air_Attack1,
        Air_Attack2,
        Throw_Sword,
        Hit 
    }

    //-------------------------------------Cac bien flag-------------------------------------//

    //notflipped = -1 neu quay sang trai, 1 neu quay sang phai
    private int isNotFlipped;
    //isJumping: chi addForce 1 lan
    private bool isJumping;
    //isDoubleJump: chi dc doubleJump 1 lan 
    private bool isDoubleJump;
    //isOnAir: phan biet Attack va JumpAttack
    private bool isOnAir;
    //isDamaged: bi tan cong
    private bool isDamaged;
    //canJump: dang bi tan cong: khong duoc nhay
    private bool canJump;
    //horizontal: di chuyen
    private float horizontal;
    //hai bien dung de doi animation
    private string currentAnim;
    private string currentAnimSword;

    //--------------------------------------------------------------------------//

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;

    [Header("Debug")]
    [SerializeField] private PlayerState currentState;
    [SerializeField] private bool hasSword;

    [Header("Sword")]
    //Neu qua thoi gian maxComboWait: khong duoc phep combo tiep nua + phai doi thoi gian attackDelay de duoc startComboAttack
    [SerializeField] private float attackDelay;
    //thoi gian toi da duoc phep thi trien combo tiep theo, neu het thoi gian thi quay ve attack1
    [SerializeField] private float maxComboWait;
    [SerializeField] private ObjectPooling swordThrowPool;
    //ATTACK
    private bool isAttacking;
    private bool goNextCombo = false;
    private bool secondMousePress = false;
    private bool canStartCombo = true;
    private bool canAirAttack = true;
    private int previousCombo;
    private bool canCombo;
    private bool isGrounding;

    void Start()
    {
        OnInit();
    }

    void OnInit()
    {
        isJumping = false;
        isDoubleJump = true;
        isOnAir = false;
        isNotFlipped = 1;
        isDamaged = false;
        canJump = true;
        isGrounding = false;

        currentAnimSword = "Idle Sword";
        currentAnim = "Idle";
        currentState = PlayerState.Idle;
    }

    void Update()
    {
        MoveInput();
        FlipPlayer();
        SwitchState();
    }

    void SwitchState()
    {
        switch (currentState)
        {
            case PlayerState.None:
                break;
            case PlayerState.Idle:
                IdleState();
                break;
            case PlayerState.Run:
                RunState();
                break;
            case PlayerState.Jump:
                JumpState();
                break;
            case PlayerState.Fall:
                FallState();
                break;
            case PlayerState.Ground:
                GroundState();
                break;
            case PlayerState.DoubleJump:
                DoubleJumpState();
                break;
            case PlayerState.Attack1:
                Attack1State();
                break;
            case PlayerState.Attack2:
                Attack2State();
                break;
            case PlayerState.Attack3:
                Attack3State();
                break;
            case PlayerState.Air_Attack1:
                AirAttack1State();
                break;
            case PlayerState.Air_Attack2:
                AirAttack2State();
                break;
            case PlayerState.Throw_Sword:
                ThrowSwordState();
                break;
            case PlayerState.Hit:
                HitState();
                break;
        }
    }

    void IdleState()
    {
        ChangeAnim(ref currentAnim,"Idle");
        ChangeAnim(ref currentAnimSword, "Idle Sword");

        //Neu nhan phim Space --> JumpState
        if (Input.GetKeyDown(KeyCode.Space) && !isDamaged && canJump)
        {
            isJumping = true;
            currentState = PlayerState.Jump;
            return;
        }

        //--------------------Change Player State--------------------//
        //Neu velocityY < 0 --> FallState
        else if(rb.velocity.y < -0.1f)
        {
            currentState = PlayerState.Fall;
        }
        //Neu velocity.x > 0 --> RunState
        else if(Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            currentState = PlayerState.Run;
        }
    }

    void RunState()
    {
        ChangeAnim(ref currentAnim, "Run");
        ChangeAnim(ref currentAnimSword, "Run Sword");

        //--------------------Change Player State--------------------//
        //Neu nhan phim Space --> JumpState
        if (Input.GetKeyDown(KeyCode.Space) && !isDamaged && canJump)
        {
            isJumping = true;
            isOnAir = true;
            currentState = PlayerState.Jump;
            return;
        }
        //Neu velocity.x = 0 --> IdleState
        if (Mathf.Abs(rb.velocity.x) < 0.1f)
        {
            currentState = PlayerState.Idle;
            return;
        }
        //Neu velocity.y < 0 --> FallState
        if (rb.velocity.y < -0.1f && 
            !Physics2D.Raycast(transform.position + Vector3.down * 0.2f, Vector2.down, 0.2f, LayerMask.GetMask("Ground")))
        {
            currentState = PlayerState.Fall;
            return;
        }
    }

    void JumpState()
    {
        ChangeAnim(ref currentAnim, "Jump");
        ChangeAnim(ref currentAnimSword, "Jump Sword");
        
        //Nhay 1 lan (AddForce)
        if(isJumping)
        {
            PoolManager.Instance.poolJump.GetFromPool(transform.position, Quaternion.identity, transform.localScale);
            rb.velocity = Vector2.zero;
            Debug.Log("Jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = false;
            isOnAir = true;
            StartCoroutine(CheckIdleDelay(3f));
        }

        //--------------------Change Player State--------------------//
        //Neu velocity.y < 0 --> Dang roi --> FallState
        if (rb.velocity.y < -0.1f)
        {
            currentState = PlayerState.Fall;
            return;
        }
        //Neu nhan phim Space --> Nhay DoubleJumpState
        if (Input.GetKeyDown(KeyCode.Space) && !isDamaged)
        {
            //Nhay 1 lan (AddForce)
            isJumping = true;
            currentState = PlayerState.DoubleJump;
            return;
        }
    }

    void DoubleJumpState()
    {
        ChangeAnim(ref currentAnim, "Jump");
        ChangeAnim(ref currentAnimSword, "Jump Sword");

        //Neu duoc phep Nhay && duoc phep Double Jump
        //Chay 1 lan
        if (isJumping && isDoubleJump)
        {
            //Particle
            PoolManager.Instance.poolJump.GetFromPool(transform.position, Quaternion.identity, transform.localScale);
            //Addforce
            rb.velocity = Vector2.zero;
            Debug.Log("Double jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //ResetFlag
            isJumping = false;
            isDoubleJump = false;
            StartCoroutine(CheckIdleDelay(3f));
        }

        //--------------------Change Player State--------------------//
        //Neu velocity.y < 0 --> FallState
        if (rb.velocity.y < -0.1f)
        {
            currentState = PlayerState.Fall;
            return;
        }
    }

    IEnumerator CheckIdleDelay(float time)
    {
        yield return new WaitForSeconds(time);
        //Dung Raycast check neu cham vao Ground --> Dang o mat dat --> GroundState
        LayerMask groundMask = LayerMask.GetMask("Ground");
        if (Physics2D.Raycast(transform.position + Vector3.down * 0.2f, Vector2.down, 0.2f, groundMask))
        {
            currentState = PlayerState.Idle;
        }
    }

    void FallState()
    {
        ChangeAnim(ref currentAnim, "Fall");
        ChangeAnim(ref currentAnimSword, "Fall Sword");

        //--------------------Change Player State--------------------//
        //Dung Raycast check neu cham vao Ground --> Dang o mat dat --> GroundState
        LayerMask groundMask = LayerMask.GetMask("Ground");
        if (Physics2D.Raycast(transform.position + Vector3.down * 0.2f, Vector2.down, 0.2f, groundMask))
        {
            isGrounding = true;
            currentState = PlayerState.Ground;
            return;
        }
        //Neu nhan phim Space va van co the DoubleJump --> DoubleJumpState
        if (Input.GetKeyDown(KeyCode.Space) && !isDamaged && canJump)
        {
            isOnAir = true;
            isJumping = true;
            currentState = PlayerState.DoubleJump;
            return;
        }
    }

    void GroundState()
    {
        //Chi chay 1 lan
        if(isGrounding)
        {
            Debug.Log("Ground pool");
            isGrounding = false;
            StartCoroutine(GroundStateDelay());
        }
    }

    IEnumerator GroundStateDelay()
    {
        //Particle
        PoolManager.Instance.poolGround.GetFromPool(transform.position, Quaternion.identity, transform.localScale);
        //Reset Double Jump
        isDoubleJump = true;
        isOnAir = false;
        //Reset AirAttack
        canAirAttack = true;
        canJump = true;

        //Neu dang di chuyen: bo qua animation
        if (Mathf.Abs(horizontal) > 0.5f)
        {
            Vector2 vector = rb.velocity;
            vector.y = 0;
            rb.velocity = vector;

            currentState = PlayerState.Run;
            yield break;
        }

        ChangeAnim(ref currentAnim, "Ground");
        ChangeAnim(ref currentAnimSword, "Ground Sword");

        //--------------------Change Player State--------------------//
        //Sau khi hoan thanh Anim bat buoc phai chuyen State khac
        yield return new WaitForEndOfFrame();
        float time = 0;
        float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
        while (time < animLength / 2)
        {
            time += Time.deltaTime;
            yield return null;
        }
        while (time < animLength/2)
        {
            time += Time.deltaTime;
            if(currentState == PlayerState.Ground)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !isDamaged)
                {
                    currentState = PlayerState.Jump;
                    yield break;
                }
                if (Mathf.Abs(horizontal) > 0.3f)
                {
                    currentState = PlayerState.Run;
                    yield break;
                }
            }
            yield return null;
        }
        //wait cho den khi animation chay xong
        if(currentState == PlayerState.Ground)
        {
            currentState = PlayerState.Idle;
        }
    }

    IEnumerator CanComboDelay()
    {
        //Khi bat dau combo: khong duoc phep attack tiep
        canCombo = false;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(1).length);
        //Sau khi chay het animation attack roi: duoc phep combo sang attack tiep theo
        canCombo = true;

        yield return new WaitForSeconds(maxComboWait);
        //Neu qua thoi gian maxComboWait: khong duoc phep combo tiep nua + phai doi thoi gian attackDelay de duoc startComboAttack
        if (!isAttacking)
        {
            canCombo = false;
            StartCoroutine(CanStartComboDelay(attackDelay));
        }
    }

    void Attack1State()
    {
        if(!isAttacking)
        {
            canStartCombo = false;

            isAttacking = true;
            previousCombo = 1;
            PoolManager.Instance.poolAttack1.GetFromPool(
                new Vector2(transform.position.x + 1.2f * isNotFlipped, transform.position.y),
                Quaternion.identity,
                transform.localScale);
            ChangeAnim(ref currentAnimSword, "Attack 1");
            StartCoroutine(HandleAttack1());
            StartCoroutine(CanComboDelay());
        }
    }

    IEnumerator HandleAttack1()
    {
        Debug.Log("Attack 1");    
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(1).length);
        EndAttackCombo();
    }

    void Attack2State()
    {
        if (!isAttacking)
        {
            canStartCombo = false;
            isAttacking = true;
            previousCombo = 2;
            PoolManager.Instance.poolAttack2.GetFromPool(
                new Vector2(transform.position.x + 1.2f * isNotFlipped, transform.position.y),
                Quaternion.identity,
                transform.localScale);
            ChangeAnim(ref currentAnimSword, "Attack 2");
            StartCoroutine(HandleAttack2());
            StartCoroutine(CanComboDelay());
        }
    }

    IEnumerator HandleAttack2()
    {
        Debug.Log("Attack 2");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(1).length);
        EndAttackCombo();
    }

    void Attack3State()
    {
        if (!isAttacking)
        {
            canStartCombo = false;
            isAttacking = true;
            previousCombo = -1;
            PoolManager.Instance.poolAttack3.GetFromPool(
                new Vector2(transform.position.x + 1.2f * isNotFlipped, transform.position.y),
                Quaternion.identity,
                transform.localScale);
            ChangeAnim(ref currentAnimSword, "Attack 3");
            StartCoroutine(HandleAttack3());
        }
    }

    IEnumerator HandleAttack3()
    {
        Debug.Log("Attack 3");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(1).length);
        StartCoroutine(CanStartComboDelay(attackDelay));
        canCombo = false;
        EndAttackCombo();
    }

    void EndAttackCombo()
    {
        isAttacking = false;
        goNextCombo = false;
        if (rb.velocity == Vector2.zero)
        {
            currentState = PlayerState.Idle;
        }
        else
        {
            currentState = PlayerState.Run;
        }
    }

    IEnumerator CanStartComboDelay(float time)
    {
        yield return new WaitForSeconds(time);
        if (!canCombo)
        {
            canStartCombo = true;
        }
    }

    void AirAttack1State()
    {

        if (!isAttacking)
        {
            canAirAttack = false;
            isAttacking = true;
            rb.gravityScale = 0.25f;
            rb.velocity = Vector2.zero;

            PoolManager.Instance.poolAirAttack1.GetFromPool(
                new Vector2(transform.position.x + 1f * isNotFlipped, transform.position.y - 0.5f),
                Quaternion.identity,
                transform.localScale);
            ChangeAnim(ref currentAnimSword, "Air Attack 1");
            StartCoroutine(HandleAirAttack1());
        }

        if (Input.GetMouseButtonDown(0) && secondMousePress)
        {
            goNextCombo = true;
        }
        //sau khi ham nay chay 1 lan thi moi duoc phep go next combo
        secondMousePress = true;
    }

    IEnumerator HandleAirAttack1()
    {
        Debug.Log("Air Attack 1");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(1).length);
        if (goNextCombo)
        {
            isAttacking = false;
            goNextCombo = false;
            secondMousePress = false;
            currentState = PlayerState.Air_Attack2;
        }
        else
        {
            EndAirAttackCombo();
        }
    }

    void AirAttack2State()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            PoolManager.Instance.poolAirAttack2.GetFromPool(
                new Vector2(transform.position.x + 1f * isNotFlipped, transform.position.y - 0.5f),
                Quaternion.identity,
                transform.localScale);
            ChangeAnim(ref currentAnimSword, "Air Attack 2");
            StartCoroutine(HandleAirAttack2());
        }

        if (Input.GetMouseButtonDown(0) && secondMousePress)
        {
            goNextCombo = true;
        }
        //sau khi ham nay chay 1 lan thi moi duoc phep go next combo
        secondMousePress = true;
    }

    IEnumerator HandleAirAttack2()
    {
        Debug.Log("Air Attack 2");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(1).length);
        EndAirAttackCombo();
        rb.gravityScale = 3;
    }

    void EndAirAttackCombo()
    {
        rb.gravityScale = 3f;
        isAttacking = false;
        goNextCombo = false;
        secondMousePress = false;
        currentState= PlayerState.Fall;
        //StartCoroutine(CanStartComboDelay(attackDelay));
    }

    void ThrowSwordState()
    {
        //Sau khi het animation bat buoc chuyen sang state khac
        if(!isAttacking)
        {
            ChangeAnim(ref currentAnimSword, "Throw Sword");
            isAttacking = true;
            StartCoroutine(ThrowSword());
        }
    }

    IEnumerator ThrowSword()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(1).length*0.5f);
        
        //object pooling
        GameObject obj = PoolManager.Instance.poolSwordThrowPool.GetFromPool(
            new Vector2(transform.position.x, transform.position.y + 0.2f),
            Quaternion.identity,
            transform.localScale);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(1).length * 0.5f);
        isAttacking = false;
        hasSword = false;
        animator.SetLayerWeight(1, 0);

        //--------------------Change Player State--------------------//
        if (isOnAir)
        {
            currentState = PlayerState.Fall;
        }
        else if (rb.velocity == Vector2.zero)
        {
            currentState = PlayerState.Idle;
        }
        else
        {
            currentState = PlayerState.Run;
        }
    }

    void HitState()
    {
        //Sau khi chay animation bat buoc chuyen sang state khac
        if(!isDamaged)
        {
            ChangeAnim(ref currentAnimSword, "Hit Sword");
            ChangeAnim(ref currentAnim, "Hit");
            canJump = false;
            isDamaged = true;
            StartCoroutine(NoDamageDurationDelay());
        }
    }

    IEnumerator NoDamageDurationDelay()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(1).length);
        isDamaged = false;

        //--------------------Change Player State--------------------//
        if (rb.velocity.y < -0.1f)
        {
            currentState = PlayerState.Fall;
        }
        else currentState = PlayerState.Idle;
    }

    //Cap nhat horizontal
    void MoveInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (!hasSword) return;
        if (Input.GetMouseButtonDown(0) && !isDamaged)
        {
            if (canCombo)
            {
                if (previousCombo == 1)
                {
                    StopCoroutine("CanComboDelay");
                    isAttacking = false;
                    currentState = PlayerState.Attack2;
                }
                else if (previousCombo == 2)
                {
                    isAttacking = false;
                    StopCoroutine("CanComboDelay");
                    currentState = PlayerState.Attack3;
                }
            }
            else if (canStartCombo)
            {
                if (isOnAir && canAirAttack)
                {
                    currentState = PlayerState.Air_Attack1;
                }
                else if (!isOnAir)
                {
                    currentState = PlayerState.Attack1;
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && !isDamaged)
        {
            currentState = PlayerState.Throw_Sword;
        }
       
        //horizontal = Input.GetAxis("Hosrizontal");

        ////Xu ly input Attack
        //if (!hasSword) return;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (canCombo)
        //    {
        //        StopCoroutine("CanComboDelay");
        //        isAttacking = false;
        //        switch (previousCombo)
        //        {
        //            case 1:
        //                currentState = PlayerState.Attack2;
        //                break;
        //            case 2:
        //                currentState = PlayerState.Attack3;
        //                break;
        //        }
        //    }
        //    else if (canStartCombo)
        //    {
        //        //kiem tra xem attack hay airAttack
        //        if (isOnAir && canAirAttack)
        //        {
        //            currentState = PlayerState.Air_Attack1;
        //        }
        //        else if (!isOnAir)
        //        {
        //            currentState = PlayerState.Attack1;
        //        }
        //    }
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    currentState = PlayerState.Throw_Sword;
        //}
    }

    //cap nhat isFlipped, transfrom.localScale
    void FlipPlayer()
    {
        //2 truong hop nhan vat di chuyen sang trai hoac phai
        if (horizontal < -0.1f && isNotFlipped == 1 || horizontal > 0.1f && isNotFlipped == -1)
        {
            isNotFlipped *= -1;
            //Dao nguoc hinh anh nhan vat qua localScale
            Vector3 flipVector = transform.localScale;
            flipVector.x *= -1;
            transform.localScale = flipVector;
        }
    }

    private void FixedUpdate()
    {
        //neu dang tan cong thi toc do di chuyen giam 5 lan
        if (isAttacking)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed * 0.2f * Time.deltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);
        }   
    }

    private void ChangeAnim(ref string currentAnim, string anim)
    {
        //if (currentAnim != anim)
        //{
        //    animator.SetBool(currentAnim, false);
        //    currentAnim = anim;
        //    animator.SetBool(anim, true);
        //}
        //else
        //{
        //    animator.SetBool(currentAnim, false);
        //    animator.SetBool(currentAnim, true);
        //}
        if(currentAnim == anim)
        {
            return;
        }
        animator.Play(anim);
        currentAnim = anim;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Sword Destroy")
        {
            animator.SetLayerWeight(1, 1);
            hasSword = true;
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "Sword No Destroy")
        {
            animator.SetLayerWeight(1, 1);
            hasSword = true;
        }
        else if(collision.tag == "Water")
        {
            currentState = PlayerState.Hit;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 12f, ForceMode2D.Impulse);
        }
    }
}
