using BloodWork.Entity.EventParams;
using System;
using BloodWork.Commons;
using BloodWork.Entity.EventParams.Ability;

namespace BloodWork.Entity
{
    public struct Events
    {
        // Movement
        public Action<Move> OnPerformMove;
        public Action<MoveStateParams> OnMoveStateEvent;

        // Jump
        public Action<BehaviourState>    OnToggleJump;
        public Action<PerformJumpParams> OnPerformJumpEvent;
        public Action<JumpStateParams>   OnJumpStateEvent;

        //Ability
        public Action<PerformDashParams> OnDashTrigger;

        //Controller
        public Action<ControllerStateParams> OnControllerStateEvent;
    }
}
