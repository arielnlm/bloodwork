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

        private TriggerState  m_TriggerState;
        private VerticalState m_VerticalState;
        private bool          m_ApplyGravity;

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

        private void SetMovementState(EntityVerticalStateParams entityVerticalStateParams)
        {
            m_VerticalState = entityVerticalStateParams.VerticalState;
            
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
                Entity.Gravity += (Priority.High, m_Gravity, GetInstanceID());
            else
                Entity.Gravity -= GetInstanceID();
        }

        private bool ShouldApplyGravity()
        {
            return m_TriggerState is TriggerState.Start or TriggerState.Continue && m_VerticalState is VerticalState.Falling ||
                   m_ApplyGravity && m_VerticalState is not (VerticalState.OnGround or VerticalState.OnWall)
                                  && (m_VerticalState is not VerticalState.Falling || m_TriggerState is not TriggerState.Stop);
        }
    }
}
