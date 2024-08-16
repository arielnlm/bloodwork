using BloodWork.Commons;
using UnityEngine;

namespace BloodWork.Jump
{
    public sealed class GroundJump : AbstractJump
    {
        private void Update()
        {
            if (JumpBehaviourState == BehaviourState.Disable || !IsJumpOwner && (TriggerState != TriggerState.Start || JumpState != JumpState.Default))
                return;

            if (Utils.IsChangedTo(ref ApplyJumpForce, ShouldApplyJumpForce(), true))
                JumpTime = 0;
        }

        private bool ShouldApplyJumpForce()
        {
            return ApplyJumpForce && !IsCeilingHit() && (!IsMinimumJumpTimePassed() || (TriggerState == TriggerState.Continue && !IsExtendedTimePassed())) ||
                   !ApplyJumpForce && TriggerState == TriggerState.Start && IsOnGround();
        }

        private void FixedUpdate()
        {
            if (JumpState == JumpState.Default && !ApplyJumpForce)
                return;

            if (ApplyJumpForce)
                Entity.Rigidbody.velocity = new Vector2(Entity.Rigidbody.velocity.x, JumpForce);

            JumpTime += Time.fixedDeltaTime;

            NotifyCurrentState();
        }
    }
}
