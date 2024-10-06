using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams;
using UnityEngine;

namespace BloodWork.Jump
{
    public abstract class AbstractJump : EntityBehaviour
    {
        [Header("Shared Properties")]
        [SerializeField] protected float JumpForce           = 10f;
        [SerializeField] protected float ExtendJumpTimeLimit = 0.2f;

        protected bool                   ApplyJumpForce;
        protected bool                   IsJumpOwner;
        protected float                  JumpTime;
        protected TriggerState           TriggerState;
        protected JumpState              JumpState;
        protected EntityEnvironmentState EntityEnvironmentState;
        protected BehaviourState         BehaviourState;

        #region Unity Pipeline

        protected override void Awake()
        {
            base.Awake();

            SetJumpBehaviourState(new JumpBehaviourStateParams(BehaviourState.Enable));
            SetTriggerState(new PerformJumpParams(TriggerState.Default));
            SetJumpState(new JumpStateParams(JumpState.Default));
        }

        protected virtual void OnEnable()
        {
            Entity.Events.OnPerformJump               += SetTriggerState;
            Entity.Events.OnJumpState                 += SetJumpState;
            Entity.Events.OnEntityEnvironmentStateChange += SetVerticalState;
            Entity.Events.OnJumpBehaviourStateChange  += SetJumpBehaviourState;
        }
        
        protected virtual void OnDisable()
        {
            Entity.Events.OnPerformJump               -= SetTriggerState;
            Entity.Events.OnJumpState                 -= SetJumpState;
            Entity.Events.OnEntityEnvironmentStateChange -= SetVerticalState;
            Entity.Events.OnJumpBehaviourStateChange  -= SetJumpBehaviourState;
        }

        /// <summary>
        /// Sets current key state (press, hold, let go, ...)
        /// </summary>
        /// <param name="performJumpParams">Holds the current key state</param>
        protected virtual void SetTriggerState(PerformJumpParams performJumpParams)
        {
            TriggerState = performJumpParams.TriggerState;
        }

        /// <summary>
        /// Sets current jump state ( not jumping, jumping)
        /// </summary>
        /// <param name="jumpStateParams">Holds the current jump state</param>
        protected virtual void SetJumpState(JumpStateParams jumpStateParams)
        {
            JumpState      = jumpStateParams.JumpState;
            IsJumpOwner    = jumpStateParams.InstanceID == GetInstanceID() &&
                             jumpStateParams.JumpState == JumpState.Jumping;
            ApplyJumpForce = IsJumpOwner && ApplyJumpForce;
        }

        private void SetVerticalState(EntityEnvironmentStateParams entityEnvironmentStateParams)
        {
            EntityEnvironmentState = entityEnvironmentStateParams.EntityEnvironmentState;

            if (IsJumpOwner &&
                EntityEnvironmentState is EntityEnvironmentState.OnGround or EntityEnvironmentState.OnWallLeft or EntityEnvironmentState.OnWallRight)
            {
                Entity.Events.OnJumpState?.Invoke(new JumpStateParams(JumpState.Default));
            }
        }

        private void SetJumpBehaviourState(JumpBehaviourStateParams jumpBehaviourStateParams)
        {
            BehaviourState = jumpBehaviourStateParams.BehaviourState;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Checks if at least one physics frame has passed (so that player cant press jump and let go before physics is updated)
        /// </summary>
        /// <returns>true if one physics frame has passed since jump is pressed</returns>
        protected bool IsMinimumJumpTimePassed()
        {
            return JumpTime >= Time.fixedDeltaTime;
        }

        /// <summary>
        /// Checks if jump is held longer than its possible
        /// </summary>
        /// <returns>true if jump is held longer than its possible</returns>
        protected bool IsExtendedTimePassed()
        {
            return JumpTime >= ExtendJumpTimeLimit;
        }

        protected bool IsOnGround()
        {
            return EntityEnvironmentState == EntityEnvironmentState.OnGround;
        }

        protected bool IsCeilingHit()
        {
            return Entity.IsOnCeiling();
        }

        #endregion
    }
}
