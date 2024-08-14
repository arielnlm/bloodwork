using UnityEngine;

namespace BloodWork.Commons
{
    public static class JumpStates
    {
        public static JumpState GetState(Vector2 velocity)
        {
            if (velocity.y > 0)
                return JumpState.Jumping;

            if (velocity.y < 0)
                return JumpState.Falling;

            return JumpState.Default;
        }
    }

    public enum JumpState : byte
    {
        Default,
        Jumping,
        Falling
    }
}
