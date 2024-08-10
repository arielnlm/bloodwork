using BloodWork.Entity.EventParams;
using System;

namespace BloodWork.Entity
{
    public struct Events
    {
        // Movement
        public Action<MoveParams> OnMove;

        // Jump
        public Action<PerformJumpParams> OnJumpKeyEvent;
        public Action<JumpStateParams>   OnJumpStateEvent;
    }
}
