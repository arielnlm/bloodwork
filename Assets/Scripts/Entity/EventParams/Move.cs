using BloodWork.Commons;

namespace BloodWork.Entity.EventParams
{
    public struct Move
    {
        public MoveDirection Direction;

        public Move(MoveDirection direction)
        {
            Direction = direction;
        }
    }

    public struct MoveStateParams
    {
        public BehaviourState State;

        public MoveStateParams(BehaviourState state)
        {
            State = state;
        }
    }
}
