namespace BloodWork.Commons
{
    public enum EntityPlatformState
    {
        OnGround       = 1 << 1,
        OnCeiling      = 1 << 2,
        OnWallLeft     = 1 << 3,
        OnWallRight    = 1 << 4,
    }
}
