using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    Transform tr;
    Rigidbody2D rb;
    BoxCollider2D col;
    public int moveSpeed;
    public float jumpForce;
    Vector2 move; // direction vector( 방향 벡터 ) , 여기서는 크기가 1이 넘어가도 사용함.

    // direction (방향)
    int _direction;
    int direction
    {
        set
        {
            _direction = value;
            if (_direction < 0)
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            else if (_direction > 0)
                transform.eulerAngles = Vector3.zero;
        }
        get { return _direction; }
    }
    public int directionInit = 1;

    // States ( 상태 )
    public PlayerState playerState;
    public JumpState jumpState;
    public RunState runState;
    public AttackState attackState;
    public DashState dashState;
    public DashAttackState dashAttackState; 
    public bool isAttacking
    {
        get
        {
            return (attackState != AttackState.Idle ||
                        dashAttackState != DashAttackState.Idle);
        }
    }

    // Detectors ( 탐지 )
    PlayerGroundDetector groundDetector;

    // Animation ( 모션 )
    Animator animator;
    float animationTimeElapsed;
    float attackTime;
    float dashTime;
    float dashAttackTime;
    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        groundDetector = GetComponent<PlayerGroundDetector>();
        animator = GetComponentInChildren<Animator>();

        direction = directionInit;
        attackTime = GetAnimationTime("Attack");
        dashTime = GetAnimationTime("Dash");
        dashAttackTime = GetAnimationTime("DashAttack");
    }
    float GetAnimationTime(string Name)
    {
        float time = 0f;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if(ac.animationClips[i].name == Name)
            {
                time = ac.animationClips[i].length; break;
            }
        }
        return time;
    }

    void Update()
    {
        // 키보드 입력을 받아 좌우로 게임오브젝트를 움직이는 기능
        float h = Input.GetAxis("Horizontal");

        if (IsChangeDirectionPossible())
        {
            if (h < 0)
                direction = -1;
            else if (h > 0)
                direction = 1;
        }
        if (IsHorizontalMovePossible())
        {
            move.x = h;

            if (groundDetector.isGrounded &&
           jumpState == JumpState.Idle &&
           isAttacking == false)
            {
                if (Mathf.Abs(h) > 0.1f) // 수평입력의 절댓값이 0보다 클때
                {
                    if (playerState != PlayerState.Run) // 플레이어 상태가 달리고 있지 않은 상태면
                    {
                        playerState = PlayerState.Run; // 플레이어 상태를 달리기로 바꿈
                        runState = RunState.PrepareToRun; // 달리기 상태를 달리기 준비로 바꿈 
                    }
                }
                else // 수평입력이 0일때
                {
                    h = 0;
                    if (playerState != PlayerState.Idle) // 플레이어 상태가 Idle이아니면 
                    {
                        playerState = PlayerState.Idle; // 플레이어 상태를 Idle로 바꿈
                        animator.Play("Idle");
                    }
                }
            }
        }      
      
        #region *** rb.velocity 사용 시 주의사항 ***
        // rb.velocity = new Vector2(h * moveSpeed, 0);
        // Rigidbody.velocity 를 물리연산 주기마다 실행할 경우
        // 비정상적인 동작을 일으킬 가능성이 있으므로
        // 주기함수에서는 velocity가 아니라 position을 변경하는 방식으로 움직인다.
        // velocity를 직접 수정하는 경우는
        // 점프하는 순간 등의 경우에 순간적으로 속도가 바뀌어야 할때 또는
        // 특정 동작에서 다른 동작으로 넘어가는 순간 속도를 재설정 해야할때 직접 수정
        #endregion 

        if (playerState != PlayerState.Jump && Input.GetKeyDown(KeyCode.C))
        {
            ChangePlayerState(PlayerState.Jump);
        }

        if (playerState != PlayerState.Dash && Input.GetKeyDown(KeyCode.Z)) 
        {
            ChangePlayerState(PlayerState.Dash);
        }

        // Attack actions
        if(isAttacking == false && Input.GetKeyDown(KeyCode.X))
        {
            PlayerState tmpStateToChange = PlayerState.Attack;
            if(dashState != DashState.Idle)
            {
                tmpStateToChange = PlayerState.DashAttack;
            }
            ChangePlayerState(tmpStateToChange);
        }
        UpdatePlayerState();
    }

    private void FixedUpdate()
    {
        FixedUpdateMovement();
    }
    void FixedUpdateMovement()
    {
        Vector2 velocity = new Vector2(move.x * moveSpeed, move.y);
        rb.position += velocity * Time.fixedDeltaTime;
    }
        
    // 플레이어의 상태가 바뀔때
    // 초기화 해야하는 요소들을 초기화해준다
    void ChangePlayerState(PlayerState stateToChange)
    {
        animationTimeElapsed = 0;
        switch (playerState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Run:
                break;
            case PlayerState.Jump:
                jumpState = JumpState.Idle;
                break;
            case PlayerState.Attack:
                attackState = AttackState.Idle;
                break;
            case PlayerState.Dash:
                dashState = DashState.Idle;
                break;
            case PlayerState.DashAttack:
                dashAttackState= DashAttackState.Idle;
                break;
            default:
                break;
        }
        switch (stateToChange)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Run:
                break;
            case PlayerState.Jump:
                jumpState = JumpState.PrepareToJump;
                break;
            case PlayerState.Attack:
                attackState = AttackState.PrepareToAttack;
                break;
            case PlayerState.Dash:
                dashState = DashState.PrepareToDash;    
                break;
            case PlayerState.DashAttack:
                dashAttackState = DashAttackState.PrepareToDashAttack;
                break;
            default:
                break;
        }
        playerState = stateToChange;
    }
    void UpdatePlayerState()
    {
        switch (playerState)    
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Run:
                UpdateRunState();
                break;
            case PlayerState.Jump:
                UpdateJumpState();
                break;
            case PlayerState.Attack:
                UpdateAttackState();
                break;
            case PlayerState.Dash:
                UpdateAttackState();
                break;
            case PlayerState.DashAttack:
                UpdateAttackState();
                break;
            default:
                break;
        }
    }

    void UpdateRunState()
    {
        switch (runState)
        {
            case RunState.PrepareToRun:
                animator.Play("Run");
                break;
            case RunState.Running:
                break;
        }
    }
    void UpdateJumpState()
    {
        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                animator.Play("Jump");
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                jumpState = JumpState.Jumping;
                break;
            case JumpState.Jumping:
                if(groundDetector.isGrounded == false)
                {
                    jumpState = JumpState.InFlight;
                }
                break;
            case JumpState.InFlight:
                if (groundDetector.isGrounded)
                {
                    playerState = PlayerState.Idle;
                    jumpState = JumpState.Idle;
                    animator.Play("Idle");
                }
                break;
        }
    }
    void UpdateAttackState()
    {
        switch (attackState)
        {
            case AttackState.PrepareToAttack:
                animator.Play("Attack");
                attackState = AttackState.Attacking;
                break;
            case AttackState.Attacking:
                if(animationTimeElapsed > attackTime)
                {
                    attackState = AttackState.Attacked;
                }
                animationTimeElapsed += Time.deltaTime;
                break;
            case AttackState.Attacked:
                playerState = PlayerState.Idle;
                attackState = AttackState.Idle;
                animationTimeElapsed = 0;
                animator.Play("Idle");
                break;
        }
    }
    void UpdateDashState()
    {
        switch (dashState)
        {
            case DashState.Idle:
                break;
            case DashState.PrepareToDash:
                animator.Play("Dash");
                dashState = DashState.Dashing;
                break;
            case DashState.Dashing:
                if(animationTimeElapsed < dashTime * 3/4)
                {
                    move.x = direction * moveSpeed * 1.5f;
                }
                else
                {
                    dashState = DashState.Dashed;
                }
                animationTimeElapsed += Time.deltaTime;
                break;
            case DashState.Dashed:
                if(animationTimeElapsed > dashTime)
                {
                    playerState = PlayerState.Idle;
                    dashState = DashState.Idle;
                    animationTimeElapsed = 0;
                    animator.Play("Idle");
                }
                else
                {
                    move.x = direction * moveSpeed / 4f;
                }
                animationTimeElapsed += Time.deltaTime;
                break;
            default:
                break;
        }
    }
    void UpdateDashAttackState()
    {
        switch (dashAttackState)
        {
            case DashAttackState.Idle:
                break;
            case DashAttackState.PrepareToDashAttack:
                animator.Play("DashAttack");
                dashAttackState = DashAttackState.DashAttacking;
                break;
            case DashAttackState.DashAttacking:
                if (animationTimeElapsed > dashAttackTime * 1 / 4)
                {
                    move.x = direction * moveSpeed / 4;
                }
                else if(animationTimeElapsed < dashAttackTime * 3/4)
                {
                    move.x = direction * moveSpeed * 1.5f;
                }
                else
                {
                    dashAttackState = DashAttackState.DashAttacked;
                }
                animationTimeElapsed += Time.deltaTime;
                break;
            case DashAttackState.DashAttacked:
                if(animationTimeElapsed < dashAttackTime)
                {
                    move.x = direction * moveSpeed / 4;
                }
                else
                {
                    playerState = PlayerState.Idle;
                    dashAttackState = DashAttackState.Idle;
                    animationTimeElapsed = 0;
                    animator.Play("Idle");
                }
                animationTimeElapsed += Time.deltaTime;
                break;
            default:
                break;
        }
    }

    private bool IsChangeDirectionPossible()
    {
        bool isOK = false;
        if(playerState == PlayerState.Idle ||
           playerState == PlayerState.Run ||
           playerState == PlayerState.Jump)
        {
            isOK = true;
        }
        return isOK;
    }
    private bool IsHorizontalMovePossible()
    {
        bool isOK = false;
        if (playerState == PlayerState.Idle ||
            playerState == PlayerState.Run)
        {
            isOK = true;
        }
        return isOK;
    }

    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Attack,
        Dash,
        DashAttack
    }
    public enum JumpState
    {
        Idle,
        PrepareToJump, // Jump 에 필요한 파라미터 세팅, 애니메이션 전환 등 
        Jumping, // Jump 물리연산을 시작하는 단계
        InFlight // Jump 물리연산이 끝나고 공중에 캐릭터가 떠있는 상태
    }
    public enum RunState
    {
        Idle,
        PrepareToRun,
        Running
    }
    public enum AttackState
    {
        Idle,
        PrepareToAttack,
        Attacking,
        Attacked
    }
    public enum DashState
    {
        Idle,
        PrepareToDash,
        Dashing,
        Dashed
    }
    public enum DashAttackState
    {
        Idle,
        PrepareToDashAttack,
        DashAttacking,
        DashAttacked
    }
}