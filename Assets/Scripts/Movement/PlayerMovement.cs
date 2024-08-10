using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D player;

    [Header("Movement Speed")]
    [SerializeField] float maxSpeed = 300f;
    [SerializeField] float accelaration = 200f;
    [SerializeField] float deceleration = 200f;

    [Header("Jumping")]
    [SerializeField] GameObject feet;
    [SerializeField] float jumpForce = 300f;
    [SerializeField] LayerMask groundLayer;

    [Header("Special Jumping Techs")]
    [SerializeField] float holdJumpTime = 0.5f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float fallDownGravitiy = 3; // make gravitiy stronger when falling down
    [SerializeField] float jumpBufferTime = 0.2f;

    //Speed
    private float _currentSpeed = 0f;
    private float moveLeft = 0f;
    private float moveRight = 0f;

    //Jumping
    private bool _jumping = false;
    private bool _maybeJump = false;
    private bool _quickJumpPromise = false;
    BoxCollider2D _feetBoxCollider;
    private float _originalGravity;
    private float _direction;

    //Jumping techs
    private float _coyoteTimeCounter = 0f;
    private float _holdJumpTimeCounter = 0f;
    private float _jumpBufferTimeCounter = 0f;

    private Entity _entity;

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = maxSpeed;
        _originalGravity = player.gravityScale;
        _feetBoxCollider = GetComponent<BoxCollider2D>();
        _entity = GetComponent<Entity>();

        _entity.Events.OnMove += HandleMovementSub;

    }

    private void HandleMovementSub(float direction)
    {
        _direction = direction;
    }

    private void Update()
    {
        CalculateSpeed();
        HandleJump();
        HandleDirection();
    }

    private void FixedUpdate()
    {
        player.velocity = new Vector2((moveLeft + moveRight) * _currentSpeed * Time.fixedDeltaTime, player.velocity.y);
        HandleJumpFixed();
    }

    private bool IsPlayerOnGround()
    {
        return Physics2D.OverlapBox((Vector2)feet.transform.position + _feetBoxCollider.offset, _feetBoxCollider.size, 0, groundLayer);
    }

    /// <summary>
    /// Handles conditions of jump and jump techs
    /// </summary>
    private void HandleJump()
    {
        bool isOnGround = IsPlayerOnGround();

        _jumpBufferTimeCounter = Input.GetKeyDown(KeyCode.Space) ? 0f : _jumpBufferTimeCounter + Time.deltaTime;
        _coyoteTimeCounter     = isOnGround                      ? 0f : _coyoteTimeCounter     + Time.deltaTime;
        _holdJumpTimeCounter   += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space)) _maybeJump = true;
        if (Input.GetKeyUp(KeyCode.Space))   _maybeJump = false;

        //Jump
        if ((_maybeJump || _jumpBufferTimeCounter < jumpBufferTime) && (isOnGround || _coyoteTimeCounter < coyoteTime))
        {
            _holdJumpTimeCounter = 0f;
            _coyoteTimeCounter = coyoteTime;
            _jumpBufferTimeCounter = jumpBufferTime;
            _jumping = true;
            _quickJumpPromise = true;
        }

        //Cancel Jump
        if (_jumping && (!_maybeJump || _holdJumpTimeCounter > holdJumpTime))
        {
            _maybeJump = false;
            _jumping = false;
        }

    }

    private void HandleDirection()
    {
        moveLeft  = _direction < 0f ? moveLeft  - (accelaration * Time.deltaTime) : moveLeft  + (deceleration * Time.deltaTime);
        moveRight = _direction > 0f ? moveRight + (accelaration * Time.deltaTime) : moveRight - (deceleration * Time.deltaTime);

        moveLeft = Mathf.Clamp(moveLeft, -1f, 0f);
        moveRight = Mathf.Clamp(moveRight, 0f, 1f);
    }

    private void HandleJumpFixed()
    {
        player.gravityScale = player.velocity.y < 0f ? fallDownGravitiy : _originalGravity;

        if (_jumping || _quickJumpPromise)
        {
            _quickJumpPromise = false;
            player.velocity = new Vector2(player.velocity.x, jumpForce * Time.fixedDeltaTime);
        }
    }

    private void CalculateSpeed()
    {
        if (_currentSpeed > maxSpeed)
        {
            Debug.Log("Test");
            _currentSpeed -= deceleration * Time.deltaTime;

            if (_currentSpeed < 0f)
                _currentSpeed = 0f;
        }
        else if (0f <= _currentSpeed && _currentSpeed < maxSpeed + 5f)
            _currentSpeed = maxSpeed;
    }

    public float CurrentSpeed
    {
        get { return _currentSpeed; }
        set { _currentSpeed = value; }
    }
}
