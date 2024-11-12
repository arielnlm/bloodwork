using BloodWork.Commons;
using UnityEngine;

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
        public float StartSpeed;
        public float EndSpeed;
        public Vector2 Direction;
        public float Acceleration;
        public float Deceleration;

        public ChangeMovementSpeedParams(float startSpeed, float endSpeed, Vector2 direction,  float acceleration, float deceleration)
        {
            StartSpeed = startSpeed;
            EndSpeed = endSpeed;
            Direction = direction;
            Acceleration = acceleration;
            Deceleration = deceleration;
        }
    }

}
