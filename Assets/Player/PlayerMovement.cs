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
    [SerializeField] float maxHoldOfJump = 0.5f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float fallDownGravitiy = 3; // make gravitiy stronger when falling down

    private float coyoteTimerCounter = 0f;
    private float _direction;
    private bool _jumping = false;
    private float _timer = 0f;
    BoxCollider2D _feetBoxCollider;
    private float _origingravity;
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

        coyoteTimerCounter = isOnGround ? coyoteTime : coyoteTimerCounter - Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && (isOnGround || coyoteTimerCounter > 0))
        {
            coyoteTimerCounter = 0f;
            _jumping = true;
            _timer = 0;
        }

        if (_jumping)
        {
            _timer += Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.Space) || _timer > maxHoldOfJump)
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
