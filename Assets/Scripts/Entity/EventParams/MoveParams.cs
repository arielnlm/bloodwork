using BloodWork.Commons;

namespace BloodWork.Entity.EventParams
{
    public struct MoveParams
    {
        public MoveDirection Direction;

        public MoveParams(MoveDirection direction)
        {
            Direction = direction;
        }
    }
}