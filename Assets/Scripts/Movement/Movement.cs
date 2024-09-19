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
            ChangeState(new MoveBehaviourStateParams(BehaviourState.Enable));
        }

        private void OnEnable()
        {
            Entity.Events.OnPerformMove         += SetDirection;
            Entity.Events.OnMoveChangeState     += ChangeState;
        }

        private void ChangeState(MoveBehaviourStateParams moveBehaviourStateParams)
        {
            State = moveBehaviourStateParams.State;
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
            if (m_Direction == MoveDirection.Idle || State == BehaviourState.Disable)
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
