using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using BloodWork.Entity.EventParams.Ability;
using BloodWork.Utils;
using UnityEngine;

namespace BloodWork.Ability
{
    public class Glide : AbstractAbility
    {
        [SerializeField] private float m_Gravity = .5f;
        [SerializeField] private float m_StartVelocity = -1f;

        private TriggerState           m_TriggerState;
        private EntityEnvironmentState m_EnvironmentState;
        private bool                   m_ApplyGravity;

        private void OnEnable()
        {
            Entity.Events.OnPerformGlide              += SetTriggerState;
            Entity.Events.OnEntityVerticalStateChange += SetMovementState;
        }

        private void OnDisable()
        {
            Entity.Events.OnPerformGlide              -= SetTriggerState;
            Entity.Events.OnEntityVerticalStateChange -= SetMovementState;
        }

        private void SetMovementState(EntityEnvironmentStateParams entityEnvironmentStateParams)
        {
            m_EnvironmentState = entityEnvironmentStateParams.EntityEnvironmentState;
            
            UpdateGravity();
        }

        private void SetTriggerState(PerformGlideParams performGlideParams)
        {
            m_TriggerState = performGlideParams.TriggerState;

            UpdateGravity();
        }

        private void UpdateGravity()
        {
            if (!ChangeReference.IsChanged(ref m_ApplyGravity, ShouldApplyGravity()))
                return;


            if (m_ApplyGravity)
            {
                Entity.Rigidbody.velocity =  new Vector2(Entity.Rigidbody.velocity.x, m_StartVelocity);
                Entity.Gravity            += (Priority.High, m_Gravity, GetInstanceID());
            }
            else
                Entity.Gravity -= GetInstanceID();
        }

        private bool ShouldApplyGravity()
        {
            return m_TriggerState is TriggerState.Start or TriggerState.Continue && m_EnvironmentState is EntityEnvironmentState.Falling;
        }
    }
}
