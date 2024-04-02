using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _enemyBody2D;
    [SerializeField] private float _idleTime = 1f;
    
    [Header("Enemy Speed")]
    [SerializeField] private float _speed;
    [SerializeField] private float chaseSpeed;

    [Header("Enemy Movement Range")]
    [SerializeField] private Rigidbody2D _maxLeft;
    [SerializeField] private Rigidbody2D _maxRight;

    public FieldOfView fieldOfView;
    private float _currentWaitTime = -1f;



    // Start is called before the first frame update
    void Start()
    {
        _currentWaitTime = _idleTime;
        _enemyBody2D.velocity = new Vector2(fieldOfView.directionRight ? _speed : -_speed, _enemyBody2D.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (_enemyBody2D.position.x >= _maxRight.position.x && fieldOfView.directionRight ||
            _enemyBody2D.position.x <= _maxLeft.position.x  && !fieldOfView.directionRight)
        {
            _enemyBody2D.velocity = Vector2.zero;
            _currentWaitTime -= Time.deltaTime;

            if (_currentWaitTime < 0)
                ChangeDirection();
            
        }

    }

    private void ChangeDirection()
    {
        fieldOfView.directionRight = !fieldOfView.directionRight;

        _enemyBody2D.velocity = new Vector2(fieldOfView.directionRight ? _speed : -_speed, _enemyBody2D.velocity.y);
        _currentWaitTime = _idleTime;
    }

}
