using System;
using BloodWork.Commons;
using BloodWork.Entity.EventParams.Ability;

namespace BloodWork.Ability
{
    public class Glide : AbstractAbility
    {
        private TriggerState m_TriggerState;
        private float m_OriginalGravity = 0f;

        private void OnEnable()
        {
            Entity.Events.OnPerformGlide += SetTriggerState;
        }

        private void OnDisable()
        {
            Entity.Events.OnPerformGlide -= SetTriggerState;
        }

        private void SetTriggerState(PerformGlideParams performGlideParams)
        {
            m_TriggerState = performGlideParams.State;
        }
    }
}
