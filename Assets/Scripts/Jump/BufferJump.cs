using BloodWork.Commons;
using UnityEngine;

namespace BloodWork.Jump
{
    public sealed class BufferJump : AbstractJump
    {
        [SerializeField] private float m_BufferTimeLimit = 0.12f;

        private float m_BufferTime;

        private void Update()
        {
            m_BufferTime = TriggerState == TriggerState.Start ? 0 : m_BufferTime + Time.deltaTime;

            if (JumpBehaviourState == BehaviourState.Disable || !IsJumpOwner && JumpState != JumpState.Default)
                return;

            if (Utils.IsChangedTo(ref ApplyJumpForce, ShouldApplyJumpForce(), true))
                JumpTime = 0;
        }

        private bool ShouldApplyJumpForce()
        {
            return ApplyJumpForce && !IsCeilingHit() && (!IsMinimumJumpTimePassed() || (TriggerState == TriggerState.Continue && !IsExtendedTimePassed())) ||
                   !ApplyJumpForce && IsOnGround() && !IsBufferTimePassed();
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

        private bool IsBufferTimePassed()
        {
            return m_BufferTime > m_BufferTimeLimit;
        }
    }
}
