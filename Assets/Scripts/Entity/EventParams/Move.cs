using BloodWork.Commons;

namespace BloodWork.Entity.EventParams
{
    public struct PerformMoveParams
    {
        public MoveDirection Direction;

        public PerformMoveParams(MoveDirection direction)
        {
            Direction = direction;
        }
    }

    public struct MoveBehaviourStateParams
    {
        public BehaviourState State;

        public MoveBehaviourStateParams(BehaviourState state)
        {
            State = state;
        }
    }
}
