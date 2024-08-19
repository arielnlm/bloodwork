using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using BloodWork.Utils;
using UnityEngine;

namespace BloodWork.Jump
{
    public sealed class BufferJump : AbstractJump
    {
        [Header("Buffer Properties")]
        [SerializeField] private float m_BufferTimeLimit = 0.12f;

        private float m_BufferTime;

        private void Update()
        {
            m_BufferTime = TriggerState == TriggerState.Start ? 0 : m_BufferTime + Time.deltaTime;

            if (BehaviourState == BehaviourState.Disable || !IsJumpOwner && JumpState != JumpState.Default)
                return;

            if (!ChangeReference.IsChangedTo(ref ApplyJumpForce, ShouldApplyJumpForce(), true))
                return;

            JumpTime = 0;
            Entity.Events.OnJumpState?.Invoke(new JumpStateParams(JumpState.Jumping, GetInstanceID()));
        }

        private bool ShouldApplyJumpForce()
        {
            return ApplyJumpForce && !IsCeilingHit() && (!IsMinimumJumpTimePassed() || (TriggerState == TriggerState.Continue && !IsExtendedTimePassed())) ||
                   !ApplyJumpForce && IsOnGround() && !IsBufferTimePassed();
        }

        private bool IsBufferTimePassed()
        {
            return m_BufferTime > m_BufferTimeLimit;
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
