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
            if (IsChanged(ref Move, UpdateMove()))
                Entity.Events.OnMove.Invoke(Move);

            if (IsChanged(ref PerformJump, UpdatePerformJump()))
                Entity.Events.OnJumpKeyEvent.Invoke(PerformJump);
        }

        protected static bool IsChanged<Type>(ref Type variable, in Type newValue)
        {
            bool changed = !variable.Equals(newValue);

            variable = newValue;

            return changed;
        }

        protected abstract MoveParams UpdateMove();

        protected abstract PerformJumpParams UpdatePerformJump();
    }
}
