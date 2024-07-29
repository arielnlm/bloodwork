using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D player;

    [Header("Movement Speed")]
    [SerializeField] float speed;

    [Header("Jumping")]
    [SerializeField] GameObject feet;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] LayerMask groundLayer;

    [Header("Special Jumping Techs")]
    [SerializeField] float holdJumpTime = 0.5f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float fallDownGravitiy = 3; // make gravitiy stronger when falling down
    [SerializeField] float jumpBufferTime = 0.2f;

    //Jumping
    private bool _jumping = false;
    BoxCollider2D _feetBoxCollider;
    private float _origingravity;
    private float _direction;

    //Jumping techs
    private float _coyoteTimeCounter = 0f;
    private float _holdJumpTimeCounter = 0f;
    private float _jumpBufferTimeCounter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _origingravity = player.gravityScale;
        _feetBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");
        HandleJump();
    }

    private void FixedUpdate()
    {
        HandleJumpFixed();
    }

    private bool CheckIsPlayerOnGround()
    {
        return Physics2D.OverlapBox((Vector2)feet.transform.position + _feetBoxCollider.offset, _feetBoxCollider.size, 0, groundLayer);
    }
    private void HandleJump()
    {
        bool isOnGround = CheckIsPlayerOnGround();

        _coyoteTimeCounter = isOnGround ? coyoteTime : _coyoteTimeCounter - Time.deltaTime;
        _jumpBufferTimeCounter = Input.GetKeyDown(KeyCode.Space) ? jumpBufferTime : _jumpBufferTimeCounter - Time.deltaTime;

        if ((Input.GetKeyDown(KeyCode.Space) || _jumpBufferTimeCounter > 0f ) && (isOnGround || _coyoteTimeCounter > 0))
        {
            _coyoteTimeCounter = 0f;
            _jumping = true;
            _holdJumpTimeCounter = 0;
            _jumpBufferTimeCounter = 0f;
        }

        if (_jumping)
        {
            _holdJumpTimeCounter += Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.Space) || _holdJumpTimeCounter > holdJumpTime)
            {
                _jumping = false;
            }
        }
    }

    private void HandleJumpFixed()
    {
        player.gravityScale = player.velocity.y < 0f ? fallDownGravitiy : _origingravity;

        player.velocity = new Vector2(_direction * speed, player.velocity.y);

        if (_jumping)
        {
            player.velocity = new Vector2(player.velocity.x, jumpForce);
        }
    }

}
