using System;
using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams;
using UnityEngine;

namespace BloodWork.Movement
{
    public class Movement : EntityBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 300f;
        [SerializeField] private float m_Accelaration = 100f;
        [SerializeField] private float m_Deceleration = 100f;

        private MoveDirection m_Direction;
        private float m_Speed;
        protected BehaviourState State;

        private void Start()
        {
            m_Speed = m_MaxSpeed;
        }

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
        private void OnDisable()
        {
            Entity.Events.OnPerformMove         -= SetDirection;
            Entity.Events.OnMoveChangeState     -= ChangeState;
        }

        private void ChangeState(MoveBehaviourStateParams moveBehaviourStateParams)
        {
            State = moveBehaviourStateParams.State;
        }

        //TODO: Bug, if movement is disabled m_Direction could be left and then idle and Movement would never register left even after enabling
        private void SetDirection(ChangeDirectionParams changeDirectionParams)
        {
            m_Direction = changeDirectionParams.Direction;
        }


        private void SetLookDirection()
        {
            if (m_Direction == MoveDirection.Idle || m_Direction == MoveDirections.ValueOf(Entity.transform.right.x))
                return;
            Vector3 lookDirection = Entity.transform.right;
            Entity.transform.right = new Vector3(m_Direction.GetValue(), lookDirection.y, lookDirection.z);
        }

        private void FixedUpdate()
        {
            if (State == BehaviourState.Disable)
                return;

            SetLookDirection();
            Entity.Rigidbody.velocity = new Vector2(m_Direction.GetValue() * m_Speed * Time.fixedDeltaTime, Entity.Rigidbody.velocity.y);
            VelocityAdjustment();
        }

        private void VelocityAdjustment()
        {
            if (m_Speed < m_MaxSpeed)
                m_Speed = Mathf.Min(m_MaxSpeed, m_Speed + m_Accelaration * Time.fixedDeltaTime);
            else if (m_Speed > m_MaxSpeed)
                m_Speed = Mathf.Max(m_MaxSpeed, m_Speed - m_Deceleration * Time.fixedDeltaTime);
        }
    }
}
