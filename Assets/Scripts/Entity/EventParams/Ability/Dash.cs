
using BloodWork.Commons;
using UnityEngine;

namespace BloodWork.Entity.EventParams.Ability
{
    public struct PerformDashParams
    {
        public TriggerState State;

        public PerformDashParams(TriggerState state)
        {
            State = state;
        }
    }
}