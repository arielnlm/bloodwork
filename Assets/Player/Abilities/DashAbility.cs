using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public float dashVelocity;
    private GameObject _parent;
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rigidBody2d;

    public override void Activate(GameObject parent)
    {
        //TODO For some reason, if i want to save _playerMovement script so i dont have to get a reference every time, it doesnt work
        _playerMovement = parent.GetComponent<PlayerMovement>();

        //TODO maybe dash in direction that player is loooking
        _playerMovement.CurrentSpeed = dashVelocity;
    }
}
