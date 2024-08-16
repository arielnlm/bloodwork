using System;
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
        [SerializeField] private float m_CooldownTime;
        [SerializeField] private float m_Speed;

        private TriggerState TriggerState;
        private float m_ActiveTimeCounter = 0f;
        private bool m_IsActive = false;
        

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


            m_IsActive = true;
            Entity.Events.OnMoveStateEvent.Invoke(new MoveStateParams(BehaviourState.Disable));
            Entity.Events.OnJumpBehaviourState.Invoke(new JumpBehaviourState(BehaviourState.Disable));
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
                Entity.Events.OnMoveStateEvent.Invoke(new MoveStateParams(BehaviourState.Enable));
                Entity.Events.OnJumpBehaviourState.Invoke(new JumpBehaviourState(BehaviourState.Enable));
            }
        }
    }
}