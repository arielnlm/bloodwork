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
        protected BehaviourState State;

        protected override void Awake()
        {
            base.Awake();
            ChangeState(new MoveDirectionChangeParams(BehaviourState.Enable));
        }

        private void OnEnable()
        {
            Entity.Events.OnPerformMove         += SetDirection;
            Entity.Events.OnMoveDirectionChange += ChangeState;
        }

        private void ChangeState(MoveDirectionChangeParams moveDirectionChangeParams)
        {
            State = moveDirectionChangeParams.State;
        }

        private void OnDisable()
        {
            Entity.Events.OnPerformMove -= SetDirection;
        }

        private void SetDirection(PerformMoveParams performMoveParams)
        {
            m_Direction = performMoveParams.Direction;
            SetLookDirection();
        }


        private void SetLookDirection()
        {
            if (m_Direction == MoveDirection.Idle)
                return;

            Vector3 lookDirection = Entity.transform.right;
            Entity.transform.right = new Vector3(m_Direction.GetValue(), lookDirection.y, lookDirection.z);
        }

        private void FixedUpdate()
        {
            if (State == BehaviourState.Disable)
                return;

            Entity.Rigidbody.velocity = new Vector2(m_Direction.GetValue() * m_MaxSpeed * Time.fixedDeltaTime, Entity.Rigidbody.velocity.y);
        }
    }
}
