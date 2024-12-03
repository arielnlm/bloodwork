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
        public float LockControllerTimer;
        public Vector2 Direction;
        public float Acceleration;
        public float Deceleration;

        public ChangeMovementSpeedParams(float startSpeed, float endSpeed, float lockControllerTimer, Vector2 direction,  float acceleration = 0f, float deceleration = 0f)
        {
            StartSpeed = startSpeed;
            LockControllerTimer = lockControllerTimer;
            EndSpeed = endSpeed;
            Direction = direction;
            Acceleration = acceleration;
            Deceleration = deceleration;
        }
    }

}
