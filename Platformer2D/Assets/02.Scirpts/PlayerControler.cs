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
    Vector2 move; // direction vector( ���� ���� ) , ���⼭�� ũ�Ⱑ 1�� �Ѿ�� �����.

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

    // States
    public PlayerState playerState;
    public JumpState jumpState;
    public RunState runState;
    public AttackState attackState;

    // Detectors
    PlayerGroundDetector groundDetector;

    // Animation
    Animator animator;
    float animationTimeElapsed;
    float attackTime;
    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        groundDetector = GetComponent<PlayerGroundDetector>();
        animator = GetComponentInChildren<Animator>();

        attackTime = GetAnimationTime("Attack");
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
        // Ű���� �Է��� �޾� �¿�� ���ӿ�����Ʈ�� �����̴� ���
        float h = Input.GetAxis("Horizontal");
        if (h < 0)
            direction = -1;
        else if (h > 0)
            direction = 1;

        move.x = h;

        if (groundDetector.isGrounded && 
            jumpState == JumpState.Idle &&
            attackState == AttackState.Idle)
        {
            if(Mathf.Abs(h) > 0.2f) // �����Է��� ������ 0���� ũ��
            {
                if (playerState != PlayerState.Run) // �÷��̾� ���°� �޸��� ���� ���� ���¸�
                {
                    playerState = PlayerState.Run; // �÷��̾� ���¸� �޸���� �ٲ�
                    runState = RunState.PrepareToRun; // �޸��� ���¸� �޸��� �غ�� �ٲ� 
                }               
            }
            else // �����Է��� 0�̸�
            {
                h = 0;
                if(playerState != PlayerState.Idle) // �÷��̾� ���°� Idle�̾ƴϸ� 
                {
                    playerState = PlayerState.Idle; // �÷��̾� ���¸� Idle�� �ٲ�
                    animator.Play("Idle");
                }                
            }
        }

        
        #region *** rb.velocity ��� �� ���ǻ��� ***
        // rb.velocity = new Vector2(h * moveSpeed, 0);
        // Rigidbody.velocity �� �������� �ֱ⸶�� ������ ���
        // ���������� ������ ����ų ���ɼ��� �����Ƿ�
        // �ֱ��Լ������� velocity�� �ƴ϶� position�� �����ϴ� ������� �����δ�.
        // velocity�� ���� �����ϴ� ����
        // �����ϴ� ���� ���� ��쿡 ���������� �ӵ��� �ٲ��� �Ҷ� �Ǵ�
        // Ư�� ���ۿ��� �ٸ� �������� �Ѿ�� ���� �ӵ��� �缳�� �ؾ��Ҷ� ���� ����
        #endregion 

        if (playerState != PlayerState.Jump && Input.GetKeyDown(KeyCode.C))
        {
            playerState = PlayerState.Jump;
            jumpState = JumpState.PrepareToJump;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            playerState = PlayerState.Attack;
            attackState = AttackState.PrepareToAttck;
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
            case AttackState.PrepareToAttck:
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
                animator.Play("Idle");
                break;
        }
    }

    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Attack
    }
    public enum JumpState
    {
        Idle,
        PrepareToJump, // Jump �� �ʿ��� �Ķ���� ����, �ִϸ��̼� ��ȯ �� 
        Jumping, // Jump ���������� �����ϴ� �ܰ�
        InFlight // Jump ���������� ������ ���߿� ĳ���Ͱ� ���ִ� ����
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
        PrepareToAttck,
        Attacking,
        Attacked
    }
}