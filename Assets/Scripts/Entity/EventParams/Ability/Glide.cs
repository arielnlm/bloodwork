using BloodWork.Commons;

namespace BloodWork.Entity.EventParams.Ability
{
    public struct PerformGlideParams
    {
        public TriggerState TriggerState;

        public PerformGlideParams(TriggerState triggerState)
        {
            TriggerState = triggerState;
        }
    }
}