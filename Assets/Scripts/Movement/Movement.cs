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

        private MoveDirection m_Direction;
        protected BehaviourState State;

        private float m_XSpeed;
        private float m_YSpeed;
        private float m_TLerp;

        protected override void Awake()
        {
            base.Awake();
            ChangeState(new MoveBehaviourStateParams(BehaviourState.Enable));
        }

        private void Start()
        {
            m_XSpeed = m_MaxSpeed;
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

        private void ChangeMovementSpeed(ChangeMovementSpeedParams changeMovementSpeedParams)
        {
            m_XSpeed = changeMovementSpeedParams.XSpeed;
            m_YSpeed = changeMovementSpeedParams.YSpeed;
            m_TLerp  = changeMovementSpeedParams.TValueLerp;
        }

        private void ChangeState(MoveBehaviourStateParams moveBehaviourStateParams)
        {
            State = moveBehaviourStateParams.State;
        }


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
            Entity.Rigidbody.velocity = new Vector2(m_Direction.GetValue() * m_XSpeed * Time.fixedDeltaTime, Entity.Rigidbody.velocity.y);

            Debug.Log(m_XSpeed);
            if (Mathf.Abs(m_XSpeed - m_MaxSpeed) > 1)
                m_XSpeed = Mathf.Lerp(m_XSpeed, m_MaxSpeed, m_TLerp);
        }
    }
}
