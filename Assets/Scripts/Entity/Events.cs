using System;
using BloodWork.Entity.EventParams;
using BloodWork.Entity.EventParams.Ability;
using BloodWork.Entity.EventParams.Attack;

namespace BloodWork.Entity
{
    public struct Events
    {
        #region Entity

        public Action<EntityEnvironmentStateParams> OnEntityEnvironmentStateChange;
        public Action<EntityWallStateParams> OnWallState;

        #endregion

        #region Move

        public Action<PerformMoveParams>         OnPerformMove;
        public Action<MoveBehaviourStateParams> OnMoveDirectionChange;

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

        #region Attack

        #region Attack | Melee

        //TODO

        #endregion

        #region Attack | Range

        public Action<PerformBloodOrbAttackParams> OnPerformBloodOrbAttack;

        #endregion

        #endregion
    }
}
