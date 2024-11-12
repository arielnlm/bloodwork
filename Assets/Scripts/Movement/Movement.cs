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

        private bool m_IsEventSpeed;
        private Vector2 m_EventDirection;
        private float m_EventEndSpeed;
        private float m_EventAcceleration;
        private float m_EventDeceleration;

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
            Entity.Events.OnChangeMovementSpeed += ChangeMovementSpeed;
        }


        private void OnDisable()
        {
            Entity.Events.OnPerformMove         -= SetDirection;
            Entity.Events.OnMoveChangeState     -= ChangeState;
            Entity.Events.OnChangeMovementSpeed -= ChangeMovementSpeed;
        }
        private void ChangeMovementSpeed(ChangeMovementSpeedParams changeMovementSpeed)
        {
            m_Speed = changeMovementSpeed.StartSpeed;
            m_EventEndSpeed = changeMovementSpeed.EndSpeed;
            m_EventDirection = changeMovementSpeed.Direction;
            m_EventAcceleration = changeMovementSpeed.Acceleration;
            m_EventDeceleration = changeMovementSpeed.Deceleration;
            m_IsEventSpeed = true;
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

            if (m_IsEventSpeed)
                PerformEventMovement();
            else
            {
                Entity.Rigidbody.velocity = new Vector2(m_Direction.GetValue() * m_Speed * Time.fixedDeltaTime, Entity.Rigidbody.velocity.y);
                VelocityAdjustment();
            }
        }

        private void PerformEventMovement()
        {
            Entity.Rigidbody.velocity = m_Speed * Time.fixedDeltaTime * m_EventDirection;
            VelocityAdjustmentForEvent();
        }

        private void VelocityAdjustmentForEvent()
        {
            if (m_IsEventSpeed && m_Speed < m_EventEndSpeed)
            {
                m_Speed = Mathf.Min(m_EventEndSpeed, m_Speed + m_EventAcceleration * Time.fixedDeltaTime);
                if (Mathf.Abs(m_Speed - m_EventEndSpeed) < 0.1)
                    m_IsEventSpeed = false;
            }
            else if (m_IsEventSpeed && m_Speed > m_EventEndSpeed)
            {
                m_Speed = Mathf.Max(m_EventEndSpeed, m_Speed - m_EventDeceleration * Time.fixedDeltaTime);
                if (Mathf.Abs(m_Speed - m_EventEndSpeed) < 0.1)
                    m_IsEventSpeed = false;
            }
        }

        private void VelocityAdjustment()
        {
            if (m_Speed < m_MaxSpeed)
                m_Speed = Mathf.Min(m_MaxSpeed, m_Speed + m_Accelaration * Time.fixedDeltaTime);
            else if (m_Speed > m_MaxSpeed)
                m_Speed = Mathf.Max(m_MaxSpeed, m_Speed - m_Deceleration * Time.fixedDeltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Vector2.zero, m_EventDirection);
        }
    }
}
