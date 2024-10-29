using BloodWork.Commons;

namespace BloodWork.Entity.EventParams
{
    public struct ChangeDirectionParams
    {
        public MoveDirection Direction;

        public ChangeDirectionParams(MoveDirection direction)
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

    public struct ChangeMovementSpeedParams
    {
        public float XSpeed;
        public float YSpeed;
        public float TValueLerp;

        public ChangeMovementSpeedParams(float xSpeed, float ySpeed, float tValueLerp)
        {
            XSpeed = xSpeed;
            YSpeed = ySpeed;
            TValueLerp = tValueLerp;
        }
    }

}
