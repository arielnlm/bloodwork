using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams;
using UnityEngine;

namespace BloodWork.Jump
{
    public abstract class AbstractJump : EntityBehaviour
    {
        [SerializeField] private float m_LayerTolerance     = 0.025f;
        [SerializeField] private float m_RigidBodyTolerance = 0.015f;
        [SerializeField] private float m_JumpForce          = 0f;
        [SerializeField] private float m_ExtendJumpTime     = 0f;

        private float   m_GroundCheckDistance;
        private Vector2 m_BoxColliderLocalSize;
        
        protected BoxCollider2D  BoxCollider  { get; private set; }
        protected LayerMask      GroundLayer  { get; private set; }
        protected KeyState       JumpKeyState { get; private set; }
        protected JumpState      JumpState    { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();

            GroundLayer  = LayerMask.GetMask("Ground");
            BoxCollider  = GetComponent<BoxCollider2D>();
            JumpKeyState = KeyState.None;

            m_BoxColliderLocalSize = BoxCollider.size * transform.localScale;
            m_GroundCheckDistance  = m_BoxColliderLocalSize.y / 2 + m_LayerTolerance;

            SetJumpKeyState(new PerformJumpParams(KeyState.None));
            SetJumpState(new JumpStateParams(JumpState.Default));
        }

        protected virtual void OnEnable()
        {
            Entity.Events.OnJumpKeyEvent   += SetJumpKeyState;
            Entity.Events.OnJumpStateEvent += SetJumpState;
        }

        protected virtual void OnDisable()
        {
            Entity.Events.OnJumpKeyEvent   -= SetJumpKeyState;
            Entity.Events.OnJumpStateEvent -= SetJumpState;
        }

        protected virtual void SetJumpKeyState(PerformJumpParams performJumpParams)
        {
            JumpKeyState = performJumpParams.KeyState;
        }

        protected virtual void SetJumpState(JumpStateParams jumpStateParams)
        {
            JumpState = jumpStateParams.JumpState;
        }

        protected bool IsOnGround()
        {
            return Physics2D.Raycast(transform.position - new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.down, m_GroundCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.down, m_GroundCheckDistance, GroundLayer);
        }
    }
}
