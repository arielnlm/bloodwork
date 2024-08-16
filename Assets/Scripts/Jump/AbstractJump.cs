using System;
using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams;
using UnityEngine;

namespace BloodWork.Jump
{
    public abstract class AbstractJump : EntityBehaviour
    {
        [SerializeField] private   float m_LayerTolerance     = 0.025f;
        [SerializeField] private   float m_RigidBodyTolerance = 0.015f;
        [SerializeField] private   float m_FallDownGravity    = 4f;
        [SerializeField] protected float JumpForce            = 10f;
        [SerializeField] protected float ExtendJumpTimeLimit  = 0.2f;

        protected BoxCollider2D  BoxCollider  { get; private set; }
        protected LayerMask      GroundLayer  { get; private set; }

        protected bool           ApplyJumpForce;
        protected bool           IsJumpOwner;
        protected float          JumpTime;
        protected TriggerState   TriggerState;
        protected JumpState      JumpState;
        protected BehaviourState JumpBehaviourState;

        private float   m_OriginalGravity;
        private float   m_VerticalCheckDistance;
        private Vector2 m_BoxColliderLocalSize;

        protected override void Awake()
        {
            base.Awake();

            GroundLayer = LayerMask.GetMask("Ground");
            BoxCollider = GetComponent<BoxCollider2D>();

            m_OriginalGravity       = Entity.Rigidbody.gravityScale;
            m_BoxColliderLocalSize  = BoxCollider.size * transform.localScale;
            m_VerticalCheckDistance = m_BoxColliderLocalSize.y / 2 + m_LayerTolerance;

            SetAvailabilityJump(new JumpBehaviourState(BehaviourState.Enable));
            SetTriggerState(new PerformJumpParams(TriggerState.Default));
            SetJumpState(new JumpStateParams(JumpState.Default));
        }

        protected virtual void OnEnable()
        {
            Entity.Events.OnPerformJump         += SetTriggerState;
            Entity.Events.OnJumpState           += SetJumpState;
            Entity.Events.OnJumpBehaviourState  += SetAvailabilityJump;
        }

        protected virtual void OnDisable()
        {
            Entity.Events.OnPerformJump         -= SetTriggerState;
            Entity.Events.OnJumpState           -= SetJumpState;
            Entity.Events.OnJumpBehaviourState  -= SetAvailabilityJump;
        }

        protected virtual void SetTriggerState(PerformJumpParams performJumpParams)
        {
            TriggerState = performJumpParams.TriggerState;
        }

        protected virtual void SetJumpState(JumpStateParams jumpStateParams)
        {
            JumpState      = jumpStateParams.JumpState;
            IsJumpOwner    = jumpStateParams.InstanceID == GetInstanceID() &&
                             jumpStateParams.JumpState != JumpState.Default;
            ApplyJumpForce = IsJumpOwner && ApplyJumpForce;

            if (IsJumpOwner)
                Entity.Rigidbody.gravityScale = JumpState == JumpState.Falling ? m_FallDownGravity : m_OriginalGravity;

        }

        private void SetAvailabilityJump(JumpBehaviourState jumpBehaviourState)
        {
            JumpBehaviourState = jumpBehaviourState.State;
        }

        protected virtual void NotifyCurrentState()
        {
            if (Utils.IsChanged(ref JumpState, JumpStates.GetState(Entity.Rigidbody.velocity))) 
                Entity.Events.OnJumpState.Invoke(new JumpStateParams(JumpState, GetInstanceID()));
        }

        protected bool IsMinimumJumpTimePassed()
        {
            return JumpTime >= Time.fixedDeltaTime;
        }

        protected bool IsExtendedTimePassed()
        {
            return JumpTime >= ExtendJumpTimeLimit;
        }

        protected bool IsCeilingHit()
        {
            return Physics2D.Raycast(transform.position - new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.up, m_VerticalCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.up, m_VerticalCheckDistance, GroundLayer);
        }

        protected bool IsOnGround()
        {
            return Physics2D.Raycast(transform.position - new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.down, m_VerticalCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.down, m_VerticalCheckDistance, GroundLayer);
        }
    }
}
