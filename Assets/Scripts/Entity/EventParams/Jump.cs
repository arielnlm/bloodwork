using BloodWork.Commons;

namespace BloodWork.Entity.EventParams
{
    public struct PerformJumpParams
    {
        public int          InstanceID;
        public TriggerState TriggerState;

        public PerformJumpParams(TriggerState triggerState)
        {
            InstanceID   = 0;
            TriggerState = triggerState;
        }

        public PerformJumpParams(TriggerState triggerState, int instanceID)
        {
            InstanceID   = instanceID;
            TriggerState = triggerState;
        }
    }

    public struct JumpStateParams
    {
        public int       InstanceID;
        public JumpState JumpState;

        public JumpStateParams(JumpState jumpState)
        {
            InstanceID = 0;
            JumpState  = jumpState;
        }

        public JumpStateParams(JumpState jumpState, int instanceID)
        {
            InstanceID = instanceID;
            JumpState  = jumpState;
        }
    }

    public struct JumpBehaviourState
    {
        public BehaviourState State;

        public JumpBehaviourState(BehaviourState state)
        {
            State = state;
        }
    }
}
