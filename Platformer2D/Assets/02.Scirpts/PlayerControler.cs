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

    // Detectors
    PlayerGroundDetector groundDetector;

    // Animation
    float animationTimeElapsed;
    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        groundDetector = GetComponent<PlayerGroundDetector>();
    }

    void Update()
    {
        // Ű���� �Է��� �޾� �¿�� ���ӿ�����Ʈ�� �����̴� ���
        float h = Input.GetAxis("Horizontal");
        if (h < 0)
            direction = -1;
        else if (h > 0)
            direction = 1;


        rb.position += new Vector2(h * moveSpeed * Time.deltaTime, 0);
        #region *** �ӵ��� Force(��) ��� �� ���ǻ��� ***
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
        UpdatePlayerState();
    }

    void UpdatePlayerState()
    {
        switch (playerState)    
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Run:
                break;
            case PlayerState.Jump:
                UpdateJumpState();
                break;
            default:
                break;
        }
    }
    void UpdateJumpState()
    {
        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                // todo => changeAnimation
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
                }
                break;
        }
    }

    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
    }
    public enum JumpState
    {
        Idle,
        PrepareToJump, // Jump �� �ʿ��� �Ķ���� ����, �ִϸ��̼� ��ȯ �� 
        Jumping, // Jump ���������� �����ϴ� �ܰ�
        InFlight, // Jump ���������� ������ ���߿� ĳ���Ͱ� ���ִ� ����
    }
}
