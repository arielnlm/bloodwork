using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams;

namespace BloodWork.Controller
{
    public abstract class AbstractController : EntityBehaviour
    {
        protected MoveParams        Move;
        protected PerformJumpParams PerformJump;

        protected virtual void Update()
        {
            if (Utils.IsChanged(ref Move, UpdateMove()))
                Entity.Events.OnMove.Invoke(Move);

            if (Utils.IsChanged(ref PerformJump, UpdatePerformJump()))
                Entity.Events.OnPerformJumpEvent.Invoke(PerformJump);
        }

        protected abstract MoveParams UpdateMove();

        protected abstract PerformJumpParams UpdatePerformJump();
    }
}
