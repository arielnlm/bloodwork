using System;
using BloodWork.Entity;

namespace BloodWork.Commons
{
    public static class VerticalStates
    {
        public static VerticalState GetState(AbstractEntity entity)
        {
            return entity.Rigidbody.velocity.y switch
            {
               > 0                        => VerticalState.Rising,
               < 0                        => VerticalState.Falling,
               0 when entity.IsOnGround() => VerticalState.OnGround,
               0 when entity.IsOnWall()   => VerticalState.OnWall,
               0                          => VerticalState.Constant,
               _                          => throw new NotFiniteNumberException()
            };
        }
    }

    public enum VerticalState : byte
    {
        Initial,
        Constant,
        OnGround,
        OnWall,
        Rising,
        Falling
    }
}