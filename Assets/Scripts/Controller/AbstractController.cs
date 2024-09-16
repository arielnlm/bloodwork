using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams;
using BloodWork.Entity.EventParams.Ability;
using BloodWork.Entity.EventParams.Attack;
using BloodWork.Utils;
using UnityEngine;

namespace BloodWork.Controller
{
    public abstract class AbstractController : EntityBehaviour
    {
        protected GamePauseParams             GamePause;
        protected PerformMoveParams           PerformMove;
        protected PerformJumpParams           PerformJump;
        protected PerformDashParams           PerformDash;
        protected PerformGlideParams          PerformGlide;
        protected PerformBloodOrbAttackParams PerformBloodOrbAttack;

        protected BehaviourState State;


        protected override void Awake()
        {
            base.Awake();
            State = BehaviourState.Enable;
        }

        private void OnEnable()
        {
            Entity.Events.OnControllerStateChange += ChangeState;
        }

        private void OnDisable()
        {
            Entity.Events.OnControllerStateChange -= ChangeState;
        }

        private void ChangeState(ControllerStateParams controllerStateParams)
        {
            State = controllerStateParams.State;
        }

        protected virtual void Update()
        {
            if (State == BehaviourState.Disable)
                return;

            if (ChangeReference.IsChanged(ref GamePause, UpdatePause()))
                GameManager.Events.OnGamePause?.Invoke(GamePause);

            if (GameManager.IsGamePaused())
                return;

            if (ChangeReference.IsChanged(ref PerformMove, UpdateMove()))
                Entity.Events.OnPerformMove?.Invoke(PerformMove);

            if (ChangeReference.IsChanged(ref PerformJump, UpdatePerformJump()))
                Entity.Events.OnPerformJump?.Invoke(PerformJump);

            if (ChangeReference.IsChanged(ref PerformDash, UpdateDash()))
                Entity.Events.OnPerformDash?.Invoke(PerformDash);

            if (ChangeReference.IsChanged(ref PerformGlide, UpdateGlide()))
                Entity.Events.OnPerformGlide?.Invoke(PerformGlide);

            if (ChangeReference.IsChanged(ref PerformBloodOrbAttack, UpdateBloodOrbAttack()))
                Entity.Events.OnPerformBloodOrbAttack?.Invoke(PerformBloodOrbAttack);
        }

        protected virtual PerformMoveParams UpdateMove() => new();

        protected virtual PerformJumpParams UpdatePerformJump() => new();

        protected virtual PerformDashParams UpdateDash() => new();

        protected virtual PerformGlideParams UpdateGlide() => new();

        protected virtual PerformBloodOrbAttackParams UpdateBloodOrbAttack() => new();

        protected virtual GamePauseParams UpdatePause() => new();
    }
}
