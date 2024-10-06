namespace BloodWork.Commons
{
    public enum EntityEnvironmentState
    {
        Initial        = 0,
        Constant       = 1 << 0,
        OnGround       = 1 << 1,
        OnCeiling      = 1 << 2,
        OnWallLeft     = 1 << 3,
        OnWallRight    = 1 << 4,
        Rising         = 1 << 5,
        Falling        = 1 << 6,
    }
}
