using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using BloodWork.Utils;
using UnityEngine;

namespace BloodWork.Jump
{
    public sealed class GroundJump : AbstractJump
    {
        private void Update()
        {
            if (BehaviourState == BehaviourState.Disable || !IsJumpOwner && (TriggerState != TriggerState.Start || JumpState != JumpState.Default))
                return;

            if (!ChangeReference.IsChangedTo(ref ApplyJumpForce, ShouldApplyJumpForce(), true)) 
                return;

            JumpTime = 0;
            Entity.Events.OnJumpState?.Invoke(new JumpStateParams(JumpState.Jumping, GetInstanceID()));
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
        }
    }
}
