using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D player;

    [Header("Movement Speed")]
    [SerializeField] float maxSpeed;
    [SerializeField] float accelaration;
    [SerializeField] float deceleration;

    [Header("Jumping")]
    [SerializeField] GameObject feet;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] LayerMask groundLayer;

    [Header("Special Jumping Techs")]
    [SerializeField] float holdJumpTime = 0.5f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float fallDownGravitiy = 3; // make gravitiy stronger when falling down
    [SerializeField] float jumpBufferTime = 0.2f;

    //Speed
    private float _currentSpeed = 0f;

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

    // Start is called before the first frame update
    void Start()
    {
        _originalGravity = player.gravityScale;
        _feetBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");
        CalculateSpeed();
        HandleJump();
    }

    private void FixedUpdate()
    {
        player.velocity = new Vector2(_direction * _currentSpeed, player.velocity.y);
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
        _holdJumpTimeCounter  += Time.deltaTime;

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

    private void HandleJumpFixed()
    {
        player.gravityScale = player.velocity.y < 0f ? fallDownGravitiy : _originalGravity;

        if (_jumping || _quickJumpPromise)
        {
            _quickJumpPromise = false;
            player.velocity = new Vector2(player.velocity.x, jumpForce);
        }
    }

    private void CalculateSpeed()
    {
        _currentSpeed = Mathf.Abs(_direction) > 0f ?
            _currentSpeed + (accelaration * Time.deltaTime):
            _currentSpeed - (deceleration * Time.deltaTime);

        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, maxSpeed);
    }

}
