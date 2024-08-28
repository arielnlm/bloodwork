using BloodWork.Commons;

namespace BloodWork.Entity.EventParams
{
    public struct EntityEnvironmentStateParams
    {
        public EntityEnvironmentState EntityEnvironmentState;

        public EntityEnvironmentStateParams(EntityEnvironmentState entityEnvironmentState)
        {
            EntityEnvironmentState = entityEnvironmentState;
        }
    }
}
