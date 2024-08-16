using BloodWork.Entity.EventParams;
using System;
using BloodWork.Commons;
using BloodWork.Entity.EventParams.Ability;

namespace BloodWork.Entity
{
    public struct Events
    {
        #region Movement

        public Action<Move> OnPerformMove;
        public Action<MoveStateParams> OnMoveStateEvent;

        #endregion

        #region Jump

        public Action<JumpBehaviourState> OnJumpBehaviourState;
        public Action<PerformJumpParams> OnPerformJump;
        public Action<JumpStateParams> OnJumpState;
        
        #endregion

        #region Ability

        #region Dash
       
        public Action<PerformDashParams> OnPerformDash;

        #endregion

        #region Glide
       
        public Action<PerformGlideParams> OnPerformGlide;

        #endregion

        #endregion

        #region Controller

        public Action<ControllerStateParams> OnControllerState;

        #endregion
    }
}
