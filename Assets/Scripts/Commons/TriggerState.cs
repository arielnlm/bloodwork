using System;

namespace BloodWork.Commons
{
    public static class TriggerStates
    {
        public static TriggerState ValueOf(byte value)
        {
            if (!Enum.IsDefined(typeof(TriggerState), value))
                throw new ArgumentException($"Enum TriggerState does not have a name associated with value {value}.");

            return (TriggerState)value;
        }

        public static byte GetValue(this TriggerState state) => (byte)state;

    }

    public enum TriggerState : byte
    {
        Default  = 0,
        Start    = 1 << 0,
        Continue = 1 << 1,
        Stop     = 1 << 2
    }
}
