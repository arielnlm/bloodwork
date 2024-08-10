namespace BloodWork.Commons
{
    public static class MoveDirections
    {
        public static sbyte GetValue(this MoveDirection direction)
        {
            return (sbyte)direction;
        }

        public static MoveDirection ValueOf(float value)
        {
            if (value < 0)
                return MoveDirection.Left;

            if (value > 0)
                return MoveDirection.Right;

            return MoveDirection.Idle;
        }
    }

    public enum MoveDirection : sbyte
    {
        Idle  =  0,
        Left  = -1,
        Right =  1
    }
}
