using BloodWork.Assets.Scripts.Commons;
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

    public struct EntityWallStateParams
    {
        public EntityWallState EntityWallState;
        public int             InstanceID;

        public EntityWallStateParams(int instanceID, EntityWallState entityWallState)
        {
            InstanceID = instanceID;
            EntityWallState = entityWallState;
        }
    }
}
