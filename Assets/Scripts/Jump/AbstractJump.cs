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
        [SerializeField] protected float JumpForce            = 5f;
        [SerializeField] protected float ExtendJumpTimeLimit  = 0.2f;

        protected BoxCollider2D  BoxCollider  { get; private set; }
        protected LayerMask      GroundLayer  { get; private set; }

        protected bool           ApplyJumpForce;
        protected bool           IsJumpHolder;
        protected float          JumpTime;
        protected TriggerState   TriggerState;
        protected JumpState      JumpState;

        private float m_VerticalCheckDistance;
        private Vector2 m_BoxColliderLocalSize;

        protected override void Awake()
        {
            base.Awake();

            GroundLayer = LayerMask.GetMask("Ground");
            BoxCollider = GetComponent<BoxCollider2D>();

            m_BoxColliderLocalSize  = BoxCollider.size * transform.localScale;
            m_VerticalCheckDistance = m_BoxColliderLocalSize.y / 2 + m_LayerTolerance;

            SetTriggerState(new PerformJumpParams(TriggerState.Default));
            SetJumpState(new JumpStateParams(JumpState.Default));
        }

        protected virtual void OnEnable()
        {
            Entity.Events.OnPerformJumpEvent += SetTriggerState;
            Entity.Events.OnJumpStateEvent   += SetJumpState;
        }

        protected virtual void OnDisable()
        {
            Entity.Events.OnPerformJumpEvent -= SetTriggerState;
            Entity.Events.OnJumpStateEvent   -= SetJumpState;
        }

        protected virtual void SetTriggerState(PerformJumpParams performJumpParams)
        {
            TriggerState = performJumpParams.TriggerState;
        }

        protected virtual void SetJumpState(JumpStateParams jumpStateParams)
        {
            JumpState      = jumpStateParams.JumpState;
            IsJumpHolder   = jumpStateParams.InstanceID == GetInstanceID() &&
                             jumpStateParams.JumpState != JumpState.Default;
            ApplyJumpForce = IsJumpHolder && ApplyJumpForce;
        }

        protected virtual void NotifyCurrentState()
        {
            if (Utils.IsChanged(ref JumpState, JumpStates.GetState(Entity.Rigidbody.velocity))) 
                Entity.Events.OnJumpStateEvent.Invoke(new JumpStateParams(JumpState, GetInstanceID()));
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
