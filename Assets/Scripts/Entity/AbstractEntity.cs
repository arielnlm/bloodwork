using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using BloodWork.Utils;
using UnityEngine;

namespace BloodWork.Entity
{
    public abstract class AbstractEntity : MonoBehaviour
    {
        [SerializeField] private float m_LayerGapTolerance = 0.025f;
        [SerializeField] private float m_RigidBodyTolerance = 0f;
        [SerializeField] private float m_FallDownGravity = 4f;

        public Events Events;

        public Rigidbody2D Rigidbody { get; private set; }
        public BoxCollider2D BoxCollider { get; private set; }
        public LayerMask GroundLayer { get; private set; }

        public Gravity Gravity;

        protected EntityVerticalStateParams EntityVerticalState;

        protected VerticalState VerticalState;

        private float m_VerticalCheckDistance;
        private float m_HorizontalCheckDistance;
        private Vector2 m_BoxColliderLocalSize;

        #region Unity Pipeline

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            BoxCollider = GetComponent<BoxCollider2D>();
            GroundLayer = LayerMask.GetMask("Ground");

            Gravity = new Gravity(Rigidbody);

            m_BoxColliderLocalSize = BoxCollider.size * transform.localScale;
            m_VerticalCheckDistance = m_BoxColliderLocalSize.y / 2 + m_LayerGapTolerance;
            m_HorizontalCheckDistance = m_BoxColliderLocalSize.x / 2 + m_LayerGapTolerance;

            EntityVerticalState = new EntityVerticalStateParams(VerticalState.Initial);
            Events.OnEntityVerticalStateChange?.Invoke(EntityVerticalState);
        }

        protected void OnEnable()
        {
            Events.OnEntityVerticalStateChange += UpdateFallDownGravity;
        }

        protected void OnDisable()
        {
            Events.OnEntityVerticalStateChange -= UpdateFallDownGravity;
        }

        private void UpdateFallDownGravity(EntityVerticalStateParams entityVerticalStateParams)
        {
            var changeReference = ChangeReference.Of(ref VerticalState, entityVerticalStateParams.VerticalState);

            if (changeReference.IsChangedTo(VerticalState.Falling))
                Gravity += (Priority.Medium, m_FallDownGravity, GetInstanceID());

            if (changeReference.IsChangedFrom(VerticalState.Falling))
                Gravity -= GetInstanceID();
        }

        protected virtual void Update()
        {
            if (ChangeReference.IsChanged(ref EntityVerticalState, UpdateEntityMovementState()))
                Events.OnEntityVerticalStateChange?.Invoke(EntityVerticalState);
        }

        protected virtual EntityVerticalStateParams UpdateEntityMovementState()
        {
            return new EntityVerticalStateParams(VerticalStates.GetState(this));
        }

        #endregion

        #region Helper Methods

        public virtual bool IsOnGround()
        {
            return Physics2D.Raycast(transform.position - new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.down, m_VerticalCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.down, m_VerticalCheckDistance, GroundLayer);
        }

        public virtual bool IsOnWall()
        {
            return Physics2D.Raycast(transform.position - new Vector3(0, m_BoxColliderLocalSize.y / 2), Vector2.right, m_HorizontalCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3(0, m_BoxColliderLocalSize.y / 2), Vector2.right, m_HorizontalCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position - new Vector3(0, m_BoxColliderLocalSize.y / 2), Vector2.left,  m_HorizontalCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3(0, m_BoxColliderLocalSize.y / 2), Vector2.left,  m_HorizontalCheckDistance, GroundLayer);
        }

        public virtual bool IsCeilingHit()
        {
            return Physics2D.Raycast(transform.position - new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.up, m_VerticalCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.up, m_VerticalCheckDistance, GroundLayer);
        }

        #endregion
    }
}
