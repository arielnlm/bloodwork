using BloodWork.Commons;

namespace BloodWork.Entity.EventParams
{
    public struct EntityVerticalStateParams
    {
        public VerticalState VerticalState;

        public EntityVerticalStateParams(VerticalState verticalState)
        {
            VerticalState = verticalState;
        }
    }
}
