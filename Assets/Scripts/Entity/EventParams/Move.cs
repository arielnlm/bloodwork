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

    public struct MoveDirectionChangeParams
    {
        public BehaviourState State;

        public MoveDirectionChangeParams(BehaviourState state)
        {
            State = state;
        }
    }
}
