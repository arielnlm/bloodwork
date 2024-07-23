using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D player;
    [SerializeField] Transform feet;
    [SerializeField] float speed;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float jumpForce = 5f;

    private float _direction;
    private bool _wantsToJump = false;
    private int _num = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");
        HandleJump();
    }

    private void FixedUpdate()
    {
        player.velocity = new Vector2(_direction * speed, player.velocity.y);

        if (_wantsToJump )
        {
            player.velocity = new Vector2(player.velocity.x, jumpForce);
            _wantsToJump = false;
        }
    }

    private bool CheckIsPlayerOnGround()
    {
        return Physics2D.OverlapCircle(feet.position, 0.4f, groundLayer);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CheckIsPlayerOnGround())
        {
            _wantsToJump = true;
        }
    }

}
