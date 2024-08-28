using BloodWork.Entity.EventParams;
using BloodWork.Entity.EventParams.Ability;
using System;

namespace BloodWork.Entity
{
    public struct Events
    {
        #region Entity

        public Action<EntityEnvironmentStateParams> OnEntityVerticalStateChange;

        #endregion

        #region Move

        public Action<PerformMoveParams>         OnPerformMove;
        public Action<MoveDirectionChangeParams> OnMoveDirectionChange;

        #endregion

        #region Jump

        public Action<JumpBehaviourStateParams> OnJumpBehaviourStateChange;
        public Action<PerformJumpParams>        OnPerformJump;
        public Action<JumpStateParams>          OnJumpState;
        
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

        public Action<ControllerStateParams> OnControllerStateChange;

        #endregion
    }
}
