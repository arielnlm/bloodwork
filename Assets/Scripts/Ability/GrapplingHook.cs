using System;
using BloodWork.Entity;
using System.Collections;
using System.Collections.Generic;
using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using BloodWork.Entity.EventParams.Ability;
using UnityEngine;

namespace BloodWork
{
    public class GrapplingHook : EntityBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 1200f;
        [SerializeField] private float m_MinSpeed = 500f;
        [SerializeField] private float m_Deceleration = 100f;

        private Vector2 m_Direction;
        private Camera m_Camera;
        private bool m_IsActive = false;
        private float m_CurrSpeed = 0f;

        // Start is called before the first frame update
        void Start()
        {
            m_Camera = Camera.main;
        }

        private void OnEnable()
        {
            Entity.Events.OnPerformGrapplingHook += OnPerformGrapplingHook;
            Entity.Events.OnPerformMove += OnChangeDirection;
        }



        private void OnDisable()
        {
            Entity.Events.OnPerformGrapplingHook -= OnPerformGrapplingHook;
        }
        private void OnChangeDirection(ChangeDirectionParams changeDirectionParams)
        {
            if (!m_IsActive)
                return;
        }
        private void OnPerformGrapplingHook(PerformGrapplingHookParams performGrapplingHookParams)
        {
            if (performGrapplingHookParams.State != TriggerState.Start)
                return;

            Entity.Events.OnChangeMovementSpeed(new ChangeMovementSpeedParams(m_MaxSpeed, m_MinSpeed, CalculateDirectionNormalized(), 0f, m_Deceleration));
            //StartHooking();
        }

        private void FixedUpdate()
        {
            //if (!m_IsActive)
            //    return;

            //m_CurrSpeed -= m_Deceleration * Time.fixedDeltaTime;
            //Entity.Rigidbody.velocity = new Vector2(Entity.transform.right.x * m_CurrSpeed * m_Direction.x, m_CurrSpeed * m_Direction.y);

            //if ( m_CurrSpeed < m_MinSpeed)
            //    StopHooking();
        }

        private void StartHooking()
        {
            Entity.Events.OnMoveChangeState?.Invoke(new MoveBehaviourStateParams(BehaviourState.Disable));
            m_IsActive = true;
            //m_CurrSpeed = m_MaxSpeed * Time.fixedDeltaTime;
            m_Direction = CalculateDirectionNormalized();
            Entity.Rigidbody.velocity = m_CurrSpeed * m_Direction;
        }

        private void StopHooking()
        {
            m_IsActive = false;
            Entity.Events.OnMoveChangeState?.Invoke(new MoveBehaviourStateParams(BehaviourState.Enable));
           // Entity.Events.OnChangeMovementSpeed?.Invoke(new ChangeMovementSpeedParams(xSpeed: m_MinSpeed, ySpeed: m_MinSpeed, 0.3f));
        }

        private Vector2 CalculateDirectionNormalized()
        {
            Vector2 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            return (mousePosition - (Vector2)gameObject.transform.position).normalized;
        }
    }
}
