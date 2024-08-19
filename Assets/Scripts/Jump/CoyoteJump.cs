using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using BloodWork.Utils;
using UnityEngine;

namespace BloodWork.Jump
{
    public sealed class CoyoteJump : AbstractJump
    {
        [Header("Coyote Properties")]
        [SerializeField] private float m_CoyoteTimeLimit = 0.04f;

        private float m_CoyoteTime;

        private void Update()
        {
            m_CoyoteTime = IsOnGround() ? 0f : m_CoyoteTime + Time.deltaTime;

            if (BehaviourState == BehaviourState.Disable || !IsJumpOwner && (TriggerState != TriggerState.Start || JumpState == JumpState.Jumping))
                return;

            if (!ChangeReference.IsChangedTo(ref ApplyJumpForce, ShouldApplyJumpForce(), true))
                return;

            JumpTime = 0;
            Entity.Events.OnJumpState?.Invoke(new JumpStateParams(JumpState.Jumping, GetInstanceID()));
        }

        private bool ShouldApplyJumpForce()
        {
            return ApplyJumpForce && !IsCeilingHit() && (!IsMinimumJumpTimePassed() || (TriggerState == TriggerState.Continue && !IsExtendedTimePassed())) ||
                   !ApplyJumpForce && (TriggerState == TriggerState.Start && !IsCoyoteTimePassed());
        }

        private bool IsCoyoteTimePassed()
        {
            return m_CoyoteTime > m_CoyoteTimeLimit;
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
