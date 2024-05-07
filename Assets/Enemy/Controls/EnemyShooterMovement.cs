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
    [SerializeField] private GameObject _bulletGameObject;
    [SerializeField] private Transform _gunTranform;
    [Range(1, 5)][SerializeField] private float _timePassToShoot;
    [SerializeField] private float _bulletSpeed;
    [Range(0, 1)][SerializeField] private float timeToReact;
    [SerializeField] private GameObject warning;

    [Header("Enemy Movement Range")]
    [SerializeField] private Rigidbody2D _maxLeft;
    [SerializeField] private Rigidbody2D _maxRight;
    public FieldOfView fieldOfView;

    private float _currentWaitTime = -1f;
    private float _currentSearchWaitTime = 0;
    private EnemyMode _enemyMode = EnemyMode.RegularSearch;
    private float _speed;
    private float _timePass;

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
            case EnemyMode.FoundPlayer: FoundPlayer(); break;
            case EnemyMode.LostPlayer: LostPlayer(); break;
            case EnemyMode.RegularSearch: RegularSearch(); break;
        }
    }


    private void FoundPlayer()
    {
        float range = _playerBody2D.position.x - _enemyBody2D.position.x;
        bool playerIsRight = range > 0;

        if (!fieldOfView.canSeePLayer)
        {
            stopShooting();
            return;
        }


        if (fieldOfView.directionRight != playerIsRight)
        {
            ChangeDirection();
        }

        if (_timePass < 0)
        {
            Shoot();
            warning.SetActive(false);
            _timePass = _timePassToShoot;
            return;
        }

        if (_timePass < timeToReact)
        {
            warning.SetActive(true);
        }
        _timePass -= Time.deltaTime;
    }

    private void Shoot()
    {
        _bulletGameObject.SetActive(true);
        _bulletGameObject.GetComponent<Rigidbody2D>().position = _gunTranform.position;
        Vector2 direction = _playerBody2D.position - _enemyBody2D.position;
        _bulletGameObject.GetComponent<Rigidbody2D>().velocity = direction * _bulletSpeed;
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
        _enemyBody2D.transform.localScale *= new Vector2(-1, 1);
        UpdateSpeed(_speed);
        _currentWaitTime = _idleTime;
    }

    private void startShooting()
    {
        _speed = 0;
        _timePass = _timePassToShoot;
        UpdateSpeed(_speed);
        fieldOfView.ChaseMode();
        _enemyMode = EnemyMode.FoundPlayer;
    }
    private void stopShooting()
    {
        warning.SetActive(false);
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
        FoundPlayer,
        LostPlayer,
        RegularSearch

    }
}
