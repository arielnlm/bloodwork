namespace BloodWork.Commons
{
    public enum EntityEnvironmentState
    {
        Initial   = 0,
        Constant  = 1 << 0,
        OnGround  = 1 << 1,
        OnCeiling = 1 << 2,
        OnWall    = 1 << 3,
        Rising    = 1 << 4,
        Falling   = 1 << 5,
    }
}
