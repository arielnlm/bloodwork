using BloodWork.Commons;
using UnityEngine;

namespace BloodWork.Jump
{
    public sealed class CoyoteJump : AbstractJump
    {
        [SerializeField] private float m_CoyoteTimeLimit = 0.04f;

        private float m_CoyoteTime;

        private void Update()
        {
            m_CoyoteTime = IsOnGround() ? 0f : m_CoyoteTime + Time.deltaTime;

            if (JumpBehaviourState == BehaviourState.Disable || !IsJumpOwner && (TriggerState != TriggerState.Start || JumpState != JumpState.Default))
                return;

            if (Utils.IsChangedTo(ref ApplyJumpForce, ShouldApplyJumpForce(), true))
                JumpTime = 0;
        }

        private bool ShouldApplyJumpForce()
        {
            return ApplyJumpForce && !IsCeilingHit() && (!IsMinimumJumpTimePassed() || (TriggerState == TriggerState.Continue && !IsExtendedTimePassed())) ||
                   !ApplyJumpForce && (TriggerState == TriggerState.Start && !IsCoyoteTimePassed());
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

        private bool IsCoyoteTimePassed()
        {
            return m_CoyoteTime > m_CoyoteTimeLimit;
        }
    }
}
