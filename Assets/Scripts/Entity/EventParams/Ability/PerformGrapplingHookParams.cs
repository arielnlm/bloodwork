using BloodWork.Commons;

namespace BloodWork.Entity.EventParams.Ability
{
    public struct PerformGrapplingHookParams
    {
        public TriggerState State;

        public PerformGrapplingHookParams(TriggerState state)
        {
            State = state;
        }
    }
}
