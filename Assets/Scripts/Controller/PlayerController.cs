using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using BloodWork.Entity.EventParams.Ability;
using BloodWork.Entity.EventParams.Attack;
using UnityEngine;

namespace BloodWork.Controller
{
    public class PlayerController : AbstractController
    {
        protected override PerformMoveParams UpdateMove()
        {
            return new PerformMoveParams(MoveDirections.ValueOf(Input.GetAxisRaw("Horizontal")));
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

        protected override PerformBloodOrbAttackParams UpdateBloodOrbAttack()
        {
            return new PerformBloodOrbAttackParams(TriggerStates.ValueOf(KeyStates.GetState(KeyCode.Mouse0)
                .GetValue()));
        }
    }
}
