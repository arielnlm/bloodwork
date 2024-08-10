using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams;
using UnityEngine;

namespace BloodWork.Movement
{
    public class Movement : EntityBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 300f;

        private MoveDirection m_Direction;

        private void OnEnable()
        {
            Entity.Events.OnMove += SetDirection;
        }

        private void OnDisable()
        {
            Entity.Events.OnMove -= SetDirection;
        }

        private void SetDirection(MoveParams moveParams)
        {
            m_Direction = moveParams.Direction;
        }

        private void FixedUpdate()
        {
            Entity.Rigidbody.velocity = new Vector2(m_Direction.GetValue() * m_MaxSpeed * Time.fixedDeltaTime, Entity.Rigidbody.velocity.y);
        }
    }
}
