using BloodWork.Commons;

namespace BloodWork.Entity.EventParams.Ability
{
    public struct PerformGlideParams
    {
        public TriggerState State;

        public PerformGlideParams(TriggerState state)
        {
            State = state;
        }
    }
}