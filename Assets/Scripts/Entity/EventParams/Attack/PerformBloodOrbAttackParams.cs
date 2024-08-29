using BloodWork.Commons;

namespace BloodWork.Entity.EventParams.Attack
{
    public struct PerformBloodOrbAttackParams
    {
        public TriggerState State;

        public PerformBloodOrbAttackParams(TriggerState state)
        {
            State = state;
        }
    }
}