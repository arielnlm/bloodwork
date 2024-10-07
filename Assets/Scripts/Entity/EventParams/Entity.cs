using BloodWork.Assets.Scripts.Commons;
using BloodWork.Commons.Types;

namespace BloodWork.Entity.EventParams
{
    public struct EntityEnvironmentStateParams
    {
        public readonly int                    InstanceID;
        public readonly EntityEnvironmentValue EntityEnvironmentValue;

        public EntityEnvironmentStateParams(EntityEnvironmentValue entityEnvironmentValue = default, int instanceID = 0)
        {
            InstanceID             = instanceID;
            EntityEnvironmentValue = entityEnvironmentValue;
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
