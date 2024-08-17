using System;
using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams;
using BloodWork.Entity.EventParams.Ability;

namespace BloodWork.Controller
{
    public abstract class AbstractController : EntityBehaviour
    {
        protected Move               Move;
        protected PerformJumpParams  PerformJump;
        protected PerformDashParams  PerformDash;
        protected PerformGlideParams PerformGlide;

        protected BehaviourState    State;


        protected override void Awake()
        {
            base.Awake();
            State = BehaviourState.Enable;
        }

        private void OnEnable()
        {
            Entity.Events.OnControllerState += ChangeState;
        }

        private void OnDisable()
        {
            Entity.Events.OnControllerState -= ChangeState;
        }

        private void ChangeState(ControllerStateParams controllerStateParams)
        {
            State = controllerStateParams.State;
        }

        protected virtual void Update()
        {
            if (State == BehaviourState.Disable)
                return;

            if (Utils.IsChanged(ref Move, UpdateMove()))
                Entity.Events.OnPerformMove?.Invoke(Move);

            if (Utils.IsChanged(ref PerformJump, UpdatePerformJump()))
                Entity.Events.OnPerformJump?.Invoke(PerformJump);

            if (Utils.IsChanged(ref PerformDash, UpdateDash()))
                Entity.Events.OnPerformDash?.Invoke(PerformDash);

            if (Utils.IsChanged(ref PerformGlide, UpdateGlide()))
                Entity.Events.OnPerformGlide?.Invoke(PerformGlide);
        }

        protected virtual Move UpdateMove() => new();

        protected virtual PerformJumpParams UpdatePerformJump() => new();

        protected virtual PerformDashParams UpdateDash() => new();

        protected virtual PerformGlideParams UpdateGlide() => new();
    }
}
