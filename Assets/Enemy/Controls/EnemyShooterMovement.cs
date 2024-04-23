using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _enemyBody2D;
    [SerializeField] private Rigidbody2D _playerBody2D;
    [Range(0, 5)][SerializeField] private float _idleTime = 1f;
    [Range(0, 5)][SerializeField] private float _idleSearch = 1f;

    [Header("Enemy Speed")]
    [SerializeField] private float _searchSpeed;

    [Header("Enemy Shooting")]
    [SerializeField] private float _timePassToShoot;

    [Header("Enemy Movement Range")]
    [SerializeField] private Rigidbody2D _maxLeft;
    [SerializeField] private Rigidbody2D _maxRight;
    public FieldOfView fieldOfView;

    private float _currentWaitTime = -1f;
    private float _currentSearchWaitTime = 0;
    private EnemyMode _enemyMode = EnemyMode.RegularSearch;
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _speed = _searchSpeed;
        _currentWaitTime = _idleTime;
        _enemyBody2D.velocity = new Vector2(fieldOfView.directionRight ? _speed : -_speed, _enemyBody2D.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {

        switch (_enemyMode)
        {
            case EnemyMode.ShootPlayer: ShootPlayer(); break;
            case EnemyMode.LostPlayer: LostPlayer(); break;
            case EnemyMode.RegularSearch: RegularSearch(); break;
        }
    }


    private void ShootPlayer()
    {
        float range = _playerBody2D.position.x - _enemyBody2D.position.x;
        bool playerIsRight = range > 0;
        float distanceBetween = Math.Abs(range);

        if (!fieldOfView.canSeePLayer)
        {
            stopShooting();
            return;
        }


        if (fieldOfView.directionRight != playerIsRight)
        {
            ChangeDirection();
        }
    }

    private void LostPlayer()
    {
        if (fieldOfView.canSeePLayer)
        {
            startShooting();
            return;
        }

        _enemyBody2D.velocity = Vector2.zero;
        _currentSearchWaitTime -= Time.deltaTime;
        if (_currentSearchWaitTime < 0)
        {
            _enemyMode = EnemyMode.RegularSearch;
            UpdateSpeed(_speed);
        }
    }

    private void RegularSearch()
    {
        if (fieldOfView.canSeePLayer)
        {
            startShooting();
            return;
        }



        if (_enemyBody2D.position.x >= _maxRight.position.x && fieldOfView.directionRight ||
            _enemyBody2D.position.x <= _maxLeft.position.x && !fieldOfView.directionRight)
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

        UpdateSpeed(_speed);
        _currentWaitTime = _idleTime;
    }

    private void startShooting()
    {
        _speed = 0;
        UpdateSpeed(_speed);
        fieldOfView.ChaseMode();
        _enemyMode = EnemyMode.ShootPlayer;
    }
    private void stopShooting()
    {
        _speed = _searchSpeed;
        UpdateSpeed(_speed);
        fieldOfView.SearchMode();
        _enemyMode = EnemyMode.LostPlayer;

        // _enemyBody2D.velocity = Vector2.zero;
        _currentSearchWaitTime = _idleSearch;
    }

    private void UpdateSpeed(float speed)
    {
        _enemyBody2D.velocity = new Vector2(fieldOfView.directionRight ? speed : -speed, _enemyBody2D.velocity.y);
    }

    private enum EnemyMode
    {
        ShootPlayer,
        LostPlayer,
        RegularSearch

    }
}
