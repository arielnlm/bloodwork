using UnityEngine;

public class PlayerMovementOriginal : MonoBehaviour
{
    [SerializeField] Rigidbody2D player;

    [Header("Movement Speed")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelaration;
    [SerializeField] private float deceleration;

    [Header("Jumping")]
    [SerializeField] private GameObject feet;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Special Jumping Techs")]
    [SerializeField] private float holdJumpTime     = 0.3f;
    [SerializeField] private float coyoteTime       = 0.08f;
    [SerializeField] private float bufferTime       = 0.1f;
    [SerializeField] private float fallDownGravitiy = 3; // make gravity stronger when falling down

    private BoxCollider2D m_FeetBoxCollider;

    private float m_CurrentSpeed = 0f;
    private float m_OriginalGravity;
    private float m_Direction;

    private void Start()
    {
        m_OriginalGravity = player.gravityScale;
        m_FeetBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        m_Direction = Input.GetAxisRaw("Horizontal");
        CalculateSpeed();
        HandleJump();
    }

    private void FixedUpdate()
    {
        player.velocity = new Vector2(m_Direction * m_CurrentSpeed * Time.fixedDeltaTime, player.velocity.y);
        HandleJumpFixed();
    }

    private bool m_ApplyJumpForce = false;
    private bool m_Jumping        = false;

    private float m_JumpTime   = 0f;
    private float m_CoyoteTime = 0f;
    private float m_BufferTime = 0f;

    private void HandleJump()
    {
        var isOnGround     = IsPlayerOnGround();
        var isSpacePressed = Input.GetKeyDown(KeyCode.Space);
        var isSpaceHeld    = Input.GetKey(KeyCode.Space) && !isSpacePressed;

        m_Jumping    = !isOnGround && (m_Jumping || m_ApplyJumpForce);
        m_CoyoteTime = isOnGround     ? 0f : m_CoyoteTime + Time.deltaTime;
        m_BufferTime = isSpacePressed ? 0f : m_BufferTime + Time.deltaTime;

        var minJumpTimeApplies    = m_JumpTime <= Time.fixedDeltaTime;
        var extendJumpTimeApplies = m_JumpTime <= holdJumpTime;
        var coyoteTimeApplies     = m_CoyoteTime <= coyoteTime;
        var bufferTimeApplies     = m_BufferTime <= bufferTime;

        m_ApplyJumpForce = m_ApplyJumpForce && (minJumpTimeApplies || (extendJumpTimeApplies && isSpaceHeld)) ||
                           !m_ApplyJumpForce && (isOnGround && (bufferTimeApplies || isSpacePressed) ||
                                                 !m_Jumping && isSpacePressed && coyoteTimeApplies);
    }

    private bool IsPlayerOnGround()
    {
        return Physics2D.OverlapBox((Vector2)feet.transform.position + m_FeetBoxCollider.offset, m_FeetBoxCollider.size, 0, groundLayer);
    }

    private void HandleJumpFixed()
    {
        m_JumpTime = !m_Jumping ? 0f : m_JumpTime + Time.fixedDeltaTime;

        player.gravityScale = player.velocity.y < 0f ? fallDownGravitiy : m_OriginalGravity;

        if (m_ApplyJumpForce)
            player.velocity = new Vector2(player.velocity.x, jumpForce);
    }

    private void CalculateSpeed()
    {
        m_CurrentSpeed = Mathf.Abs(m_Direction) > 0f ?
            m_CurrentSpeed + (accelaration * Time.deltaTime):
            m_CurrentSpeed - (deceleration * Time.deltaTime);

        m_CurrentSpeed = Mathf.Clamp(m_CurrentSpeed, 0f, maxSpeed);
    }

}
