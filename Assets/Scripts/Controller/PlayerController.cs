using System;
using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using UnityEngine;

namespace BloodWork.Controller
{
    public class PlayerController : AbstractController
    {
        protected override MoveParams UpdateMove()
        {
            return new MoveParams(MoveDirections.ValueOf(Input.GetAxisRaw("Horizontal")));
        }

        protected override PerformJumpParams UpdatePerformJump()
        {
            return new PerformJumpParams(KeyStates.GetState(KeyCode.Space));
        }
    }
}
