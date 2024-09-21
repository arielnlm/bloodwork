﻿using System;
using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using BloodWork.Entity.EventParams.Ability;
using UnityEngine;
using UnityEngine.Serialization;

namespace BloodWork.Ability
{
    public class Dash : AbstractAbility
    {
        
        [SerializeField] private float m_ActiveTime;
        [SerializeField] private float m_Speed;

        private TriggerState TriggerState;
        private float m_ActiveTimeCounter;
        private bool m_IsActive;
        

        private void OnEnable()
        {
            Entity.Events.OnPerformDash += SetTriggerState;
        }

        private void OnDisable()
        {
            Entity.Events.OnPerformDash -= SetTriggerState;
        }

        private void SetTriggerState(PerformDashParams dashState)
        {
            TriggerState = dashState.State;
            if (TriggerState != TriggerState.Start)
                return;

            if (IsOnCoolDown)
                return;

            m_IsActive = true;
            Entity.Events.OnMoveChangeState?.Invoke(new MoveBehaviourStateParams(BehaviourState.Disable));
            Entity.Events.OnJumpBehaviourStateChange?.Invoke(new JumpBehaviourStateParams(BehaviourState.Disable));
            StartCoroutine(Cooldown());
        }

        private void FixedUpdate()
        {
            m_ActiveTimeCounter = !m_IsActive ? 0f : m_ActiveTimeCounter + Time.fixedDeltaTime;

            if (!m_IsActive)
                return;

            Entity.Rigidbody.velocity = new Vector2(Entity.transform.right.x * m_Speed * Time.fixedDeltaTime, 0f);

            m_IsActive = m_ActiveTimeCounter <= m_ActiveTime;
            if (!m_IsActive)
            {
                Entity.Events.OnMoveChangeState?.Invoke(new MoveBehaviourStateParams(BehaviourState.Enable));
                Entity.Events.OnJumpBehaviourStateChange?.Invoke(new JumpBehaviourStateParams(BehaviourState.Enable));
            }
        }
    }
}