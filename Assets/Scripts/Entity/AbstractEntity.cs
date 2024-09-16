using System;
using System.Text;
using BloodWork.Commons;
using BloodWork.Commons.Types;
using BloodWork.Entity.EventParams;
using BloodWork.Utils;
using UnityEngine;

namespace BloodWork.Entity
{
    public abstract class AbstractEntity : MonoBehaviour
    {
        [SerializeField] private float m_LayerGapTolerance  = 0.025f;
        [SerializeField] private float m_RigidBodyTolerance = 0f;
        [SerializeField] private float m_FallDownGravity    = 4f;

        public Events Events;

        public Rigidbody2D   Rigidbody   { get; private set; }
        public BoxCollider2D BoxCollider { get; private set; }
        public LayerMask     GroundLayer { get; private set; }

        public Gravity     Gravity;
        public Environment Environment;

        protected EntityEnvironmentStateParams EntityEnvironmentStateParams;

        protected EntityEnvironmentState EntityEnvironmentState;
        
        private float   m_VerticalCheckDistance;
        private float   m_HorizontalCheckDistance;
        private Vector2 m_BoxColliderLocalSize;
        private Signum  m_VelocitySign;

        #region Unity Pipeline

        protected virtual void Awake()
        {
            Rigidbody   = GetComponent<Rigidbody2D>();
            BoxCollider = GetComponent<BoxCollider2D>();
            GroundLayer = LayerMask.GetMask("Ground");

            Gravity     = new Gravity(Rigidbody);
            Environment = new Environment(Rigidbody);

            m_BoxColliderLocalSize    = BoxCollider.size * transform.localScale;
            m_VerticalCheckDistance   = m_BoxColliderLocalSize.y / 2 + m_LayerGapTolerance;
            m_HorizontalCheckDistance = m_BoxColliderLocalSize.x / 2 + m_LayerGapTolerance;

            EntityEnvironmentStateParams = new EntityEnvironmentStateParams(EntityEnvironmentState.Initial);

            Events.OnEntityEnvironmentStateChange?.Invoke(EntityEnvironmentStateParams);
        }

        protected void OnEnable()
        {
            Events.OnEntityEnvironmentStateChange += UpdateFallDownGravity;
        }

        protected void OnDisable()
        {
            Events.OnEntityEnvironmentStateChange -= UpdateFallDownGravity;
        }

        private void UpdateFallDownGravity(EntityEnvironmentStateParams entityEnvironmentStateParams)
        {
            var changeReference = ChangeReference.Of(ref EntityEnvironmentState, entityEnvironmentStateParams.EntityEnvironmentState);

            if (changeReference.IsChangedTo(EntityEnvironmentState.Falling))
                Gravity += (Priority.Medium, m_FallDownGravity, GetInstanceID());
                    
            if (changeReference.IsChangedFrom(EntityEnvironmentState.Falling))
                Gravity -= GetInstanceID();
        }

        protected virtual void FixedUpdate()
        {
            if (ChangeReference.IsChanged(ref EntityEnvironmentStateParams, UpdateEntityEnvironmentState()))
                Events.OnEntityEnvironmentStateChange?.Invoke(EntityEnvironmentStateParams);
        }

        protected virtual EntityEnvironmentStateParams UpdateEntityEnvironmentState()
        {
            return new EntityEnvironmentStateParams(Environment.Get());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (1 << collision.gameObject.layer != GroundLayer.value)
                return;

            var contactPoints = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contactPoints);

            //TODO: Check whether contact count is always two
            if (collision.contactCount != 2)
            {
                var stringBuilder = new StringBuilder($"Contact points: {collision.contactCount}\n");

                for (var index = 0; index < collision.contactCount; index++)
                    stringBuilder.Append($"Contact Point {index}: {contactPoints[index].point}\n");

                throw new Exception(stringBuilder.ToString());
            }

            if (contactPoints[0].point.x - contactPoints[1].point.x == 0)
                Environment += (collision.gameObject.GetInstanceID(), EntityPlatformState.OnWall);
            else if (contactPoints[0].point.y - contactPoints[1].point.y == 0)
                Environment += (collision.gameObject.GetInstanceID(),
                                (contactPoints[0].point.y - transform.position.y) switch
                                { 
                                    > 0 => EntityPlatformState.OnCeiling, 
                                    < 0 => EntityPlatformState.OnGround,
                                    _   => throw new ArgumentOutOfRangeException()
                                });
            else throw new Exception($"All contact point coordinates are different. Point 1: {contactPoints[0].point}, Point 2: {contactPoints[1].point}");
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (1 << collision.gameObject.layer != GroundLayer)
                return;

            Environment -= collision.gameObject.GetInstanceID();
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
            return Physics2D.Raycast(transform.position - new Vector3(0, m_BoxColliderLocalSize.y / 2 - m_RigidBodyTolerance), Vector2.right, m_HorizontalCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3(0, m_BoxColliderLocalSize.y / 2 - m_RigidBodyTolerance), Vector2.right, m_HorizontalCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position - new Vector3(0, m_BoxColliderLocalSize.y / 2 - m_RigidBodyTolerance), Vector2.left,  m_HorizontalCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3(0, m_BoxColliderLocalSize.y / 2 - m_RigidBodyTolerance), Vector2.left,  m_HorizontalCheckDistance, GroundLayer);
        }

        public virtual bool IsOnCeiling()
        {
            return Physics2D.Raycast(transform.position - new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.up, m_VerticalCheckDistance, GroundLayer) ||
                   Physics2D.Raycast(transform.position + new Vector3(m_BoxColliderLocalSize.x / 2 - m_RigidBodyTolerance, 0), Vector2.up, m_VerticalCheckDistance, GroundLayer);
        }

        #endregion
    }
}
