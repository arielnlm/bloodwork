using System;
using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams;
using BloodWork.Entity.EventParams.Ability;

namespace BloodWork.Controller
{
    public abstract class AbstractController : EntityBehaviour
    {
        protected Move        Move;
        protected PerformJumpParams PerformJump;
        protected PerformDashParams PerformDash;

        protected BehaviourState    State;


        protected override void Awake()
        {
            base.Awake();
            State = BehaviourState.Enable;
        }

        private void OnEnable()
        {
            Entity.Events.OnControllerStateEvent += ChangeState;
        }

        private void OnDisable()
        {
            Entity.Events.OnControllerStateEvent -= ChangeState;
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
                Entity.Events.OnPerformMove.Invoke(Move);

            if (Utils.IsChanged(ref PerformJump, UpdatePerformJump()))
                Entity.Events.OnPerformJumpEvent.Invoke(PerformJump);

            if (Utils.IsChanged(ref PerformDash, UpdateDash()))
                Entity.Events.OnDashTrigger.Invoke(PerformDash);
        }

        protected virtual Move UpdateMove() => new();

        protected virtual PerformJumpParams UpdatePerformJump() => new();

        protected virtual PerformDashParams UpdateDash() => new();
    }
}
