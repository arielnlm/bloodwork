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
        [SerializeField] private float m_Speed = 100f;
        [SerializeField] private float m_TValueLerp = 0.3f;

        private Vector2 m_Direction;
        private Camera m_Camera;
        // Start is called before the first frame update
        void Start()
        {
            m_Camera = Camera.main;
        }

        private void OnEnable()
        {
            Entity.Events.OnPerformGrapplingHook += OnPerformGrapplingHook;
        }


        private void OnDisable()
        {
            Entity.Events.OnPerformGrapplingHook -= OnPerformGrapplingHook;
        }
        private void OnPerformGrapplingHook(PerformGrapplingHookParams performGrapplingHookParams)
        {
            if (performGrapplingHookParams.State == TriggerState.Start)
                Entity.Events.OnChangeMovementSpeed?.Invoke(new ChangeMovementSpeedParams(m_Speed * m_Direction.x, m_Speed * m_Direction.y, m_TValueLerp));
        }

        // Update is called once per frame
        void Update()
        {
            CalculateDirection();
        }

        private void CalculateDirection()
        {
            Vector2 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            m_Direction = (mousePosition - (Vector2)gameObject.transform.position).normalized;

        }
    }
}
