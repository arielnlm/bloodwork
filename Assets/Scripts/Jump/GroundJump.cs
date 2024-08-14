﻿using BloodWork.Commons;
using UnityEngine;

namespace BloodWork.Jump
{
    public sealed class GroundJump : AbstractJump
    {
        private void Update()
        {
            if (!IsJumpHolder && (TriggerState != TriggerState.Start || JumpState != JumpState.Default))
                return;

            if (Utils.IsChanged(ref ApplyJumpForce, ShouldApplyJumpForce()))
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
