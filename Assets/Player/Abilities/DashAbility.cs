using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Abilities/Dash")]
public class DashAbility : Ability
{
    public float dashVelocity;
    private GameObject _parent;
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rigidBody2d;


    private float _cooldownCounter = 0f;
    private float _activeCounter = 0f;


    public override void Use(GameObject parent)
    {
        //if (_parent == null) 
        //{
        //    _parent = parent;
        //    _playerMovement = parent.GetComponent<PlayerMovement>();
        //    _rigidBody2d = parent.GetComponent<Rigidbody2D>();
        //}

        Debug.Log(abilityState);
        if (abilityState == AbilityState.ready)
            Ready(parent);
        else if (abilityState == AbilityState.active)
            Active(parent);
        else
            CoolDown();
    }

    protected override void Ready(GameObject parent)
    {

        //TODO For some reason, if i want to save _playerMovement script so i dont have to get a reference every time, it doesnt work
        _playerMovement = parent.GetComponent<PlayerMovement>();

        //TODO maybe dash in direction that player is loooking
        _playerMovement.CurrentSpeed = dashVelocity;

        _activeCounter = 0f;
        abilityState = AbilityState.active;
    }

    protected override void Active(GameObject parent)
    {
        if (_activeCounter < activeTime)
        {
            _activeCounter += Time.deltaTime;
        }
        else
        {
            abilityState = AbilityState.cooldown;
            _cooldownCounter = 0f;
        }
    }

    protected override void CoolDown()
    {
        if (_cooldownCounter < cooldownTime)
            cooldownTime += Time.deltaTime;
        else
            abilityState = AbilityState.ready;
    }
}
