using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using BloodWork.Entity.EventParams.Ability;
using UnityEngine;

namespace BloodWork.Controller
{
    public class PlayerController : AbstractController
    {
        protected override Move UpdateMove()
        {
            return new Move(MoveDirections.ValueOf(Input.GetAxisRaw("Horizontal")));
        }

        protected override PerformJumpParams UpdatePerformJump()
        {
            return new PerformJumpParams(TriggerStates.ValueOf(KeyStates.GetState(KeyCode.Space)
                                                                        .GetValue()));
        }

        protected override PerformDashParams UpdateDash()
        {
            return new PerformDashParams(TriggerStates.ValueOf(KeyStates.GetState(KeyCode.X)
                .GetValue()));
        }

        protected override PerformGlideParams UpdateGlide()
        {
            return new PerformGlideParams(TriggerStates.ValueOf(KeyStates.GetState(KeyCode.Space)
                .GetValue()));
        }
    }
}
