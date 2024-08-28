namespace BloodWork.Commons
{
    public enum EntityPlatformState
    {
        OnGround  = 1 << 1,
        OnCeiling = 1 << 2,
        OnWall    = 1 << 3,
    }
}
