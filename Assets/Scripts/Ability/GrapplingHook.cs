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
        [SerializeField] private float m_Speed = 1200;
        [SerializeField] private float m_Deceleration = 100f;

        private Vector2 m_Direction;
        private Camera m_Camera;
        private bool m_IsActive = false;
        private float m_CurrSpeed = 0f;

        private Vector2 m_OldMovement = Vector2.zero;
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

        private void OnChangeDirection(ChangeDirectionParams changeDirectionParams)
        {
            if (!m_IsActive)
                return;
            Vector3 tRight = Entity.transform.right;
            Entity.transform.right = new Vector3(MoveDirections.GetValue(changeDirectionParams.Direction), tRight.y, tRight.z);
        }


        private void OnDisable()
        {
            Entity.Events.OnPerformGrapplingHook -= OnPerformGrapplingHook;
        }
        private void OnPerformGrapplingHook(PerformGrapplingHookParams performGrapplingHookParams)
        {
            if (performGrapplingHookParams.State != TriggerState.Start)
                return;
            StartHooking();
        }

        private void FixedUpdate()
        {
            if (!m_IsActive)
                return;

            m_CurrSpeed -= m_Deceleration * Time.fixedDeltaTime;
            Entity.Rigidbody.velocity = new Vector2(Entity.transform.right.x * m_CurrSpeed * m_Direction.x, m_CurrSpeed * m_Direction.y);

            if ( m_CurrSpeed< Mathf.Abs(m_OldMovement.x))
                StopHooking();
        }

        private void StartHooking()
        {
            Entity.Events.OnMoveChangeState?.Invoke(new MoveBehaviourStateParams(BehaviourState.Disable));
            m_IsActive = true;
            m_OldMovement = new Vector2(Entity.Rigidbody.velocity.x, 0);
            m_CurrSpeed = m_Speed * Time.fixedDeltaTime;
            m_Direction = CalculateDirectionNormalized();
            Entity.Rigidbody.velocity = m_CurrSpeed * m_Direction;
        }

        private void StopHooking()
        {
            Entity.Rigidbody.velocity = m_OldMovement;
            m_IsActive = false;
            Entity.Events.OnMoveChangeState?.Invoke(new MoveBehaviourStateParams(BehaviourState.Enable));
        }

        private Vector2 CalculateDirectionNormalized()
        {
            Vector2 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            return (mousePosition - (Vector2)gameObject.transform.position).normalized;
        }
    }
}
