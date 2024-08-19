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

        protected bool           ApplyJumpForce;
        protected bool           IsJumpOwner;
        protected float          JumpTime;
        protected TriggerState   TriggerState;
        protected JumpState      JumpState;
        protected VerticalState  VerticalState;
        protected BehaviourState BehaviourState;

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
            Entity.Events.OnEntityVerticalStateChange += SetVerticalState;
            Entity.Events.OnJumpBehaviourStateChange  += SetJumpBehaviourState;
        }
        
        protected virtual void OnDisable()
        {
            Entity.Events.OnPerformJump               -= SetTriggerState;
            Entity.Events.OnJumpState                 -= SetJumpState;
            Entity.Events.OnEntityVerticalStateChange -= SetVerticalState;
            Entity.Events.OnJumpBehaviourStateChange  -= SetJumpBehaviourState;
        }

        protected virtual void SetTriggerState(PerformJumpParams performJumpParams)
        {
            TriggerState = performJumpParams.TriggerState;
        }

        protected virtual void SetJumpState(JumpStateParams jumpStateParams)
        {
            JumpState      = jumpStateParams.JumpState;
            IsJumpOwner    = jumpStateParams.InstanceID == GetInstanceID() &&
                             jumpStateParams.JumpState == JumpState.Jumping;
            ApplyJumpForce = IsJumpOwner && ApplyJumpForce;
        }

        private void SetVerticalState(EntityVerticalStateParams entityVerticalStateParams)
        {
            VerticalState = entityVerticalStateParams.VerticalState;

            if (IsJumpOwner && VerticalState is VerticalState.OnGround or VerticalState.OnWall)
                Entity.Events.OnJumpState?.Invoke(new JumpStateParams(JumpState.Default));
        }

        private void SetJumpBehaviourState(JumpBehaviourStateParams jumpBehaviourStateParams)
        {
            BehaviourState = jumpBehaviourStateParams.BehaviourState;
        }

        #endregion

        #region Helper Methods

        protected bool IsMinimumJumpTimePassed()
        {
            return JumpTime >= Time.fixedDeltaTime;
        }

        protected bool IsExtendedTimePassed()
        {
            return JumpTime >= ExtendJumpTimeLimit;
        }

        protected bool IsOnGround()
        {
            return VerticalState == VerticalState.OnGround;
        }

        protected bool IsCeilingHit()
        {
            return Entity.IsCeilingHit();
        }

        #endregion
    }
}
